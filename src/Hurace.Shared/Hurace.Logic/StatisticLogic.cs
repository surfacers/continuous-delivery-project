using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Data;

namespace Hurace.Logic
{
    public class StatisticLogic : IStatisticLogic
    {
        private class RaceResult
        {
            public int SkierId { get; set; }
            public TimeSpan Time { get; set; }
        }

        private readonly IRaceDataManager raceDataManager;
        private readonly IStartListManager startListManager;

        public StatisticLogic(
            IRaceDataManager raceDataManager,
            IStartListManager startListManager)
        {
            this.raceDataManager = raceDataManager ?? throw new ArgumentNullException(nameof(raceDataManager));
            this.startListManager = startListManager ?? throw new ArgumentNullException(nameof(startListManager));
        }

        private async Task<IEnumerable<RaceResult>> GetRaceResults(int raceId, int runNumber, int sensorAmount)
        {
            IEnumerable<StartList> startList = await this.startListManager.GetByRaceIdAsync(raceId, runNumber);
            var raceData = await this.raceDataManager.GetByRaceIdAsync(raceId, runNumber);

            var raceResult = startList
                .Where(s => !s.IsDisqualified)
                .Where(s => raceData.Count(d => d.StartListId == s.Id) == sensorAmount)
                .Select(s => new
                {
                    SkierId = s.SkierId,
                    Start = raceData.Where(r => r.StartListId == s.Id).Min(r => r.TimeStamp),
                    End = raceData.Where(r => r.StartListId == s.Id).Max(r => r.TimeStamp),
                })
                .OrderBy(s => (s.End - s.Start).TotalMilliseconds)
                .Select((s, i) => new RaceResult
                {
                    SkierId = s.SkierId,
                    Time = s.End - s.Start
                })
                .ToList();

            return raceResult;
        }

        private IEnumerable<RaceStatisticEntry> GetRaceStatisticEntry(IEnumerable<RaceResult> raceResults)
        {
            var bestRaceResult = raceResults.OrderBy(r => r.Time).FirstOrDefault();
            return raceResults
                .OrderBy(r => r.Time)
                .Select((r, i) => new RaceStatisticEntry
                {
                    CurrentPosition = i + 1,
                    DeltaPosition = null,
                    SkierId = r.SkierId,
                    Time = r.Time,
                    DeltaTimeLeadership = r.Time - bestRaceResult.Time
                })
                .ToList();
        }

        public async Task<IEnumerable<RaceStatisticEntry>> GetRaceStatistics(int raceId, int runNumber, int sensorAmount)
        {
            if (runNumber != 1 && runNumber != 2)
            {
                throw new ArgumentException();
            }

            var raceResults1 = await this.GetRaceResults(raceId, runNumber: 1, sensorAmount);
            var statsRun1 = this.GetRaceStatisticEntry(raceResults1);

            if (runNumber == 1)
            {
                return statsRun1;
            }

            var raceResults2 = await this.GetRaceResults(raceId, runNumber: 2, sensorAmount);
            foreach (var raceResult in raceResults2)
            {
                var previousResult = raceResults1.Where(r => r.SkierId == raceResult.SkierId).First();
                raceResult.Time = raceResult.Time.Add(previousResult.Time);
            }

            var statsRun2 = this.GetRaceStatisticEntry(raceResults2);
            foreach (var stat in statsRun2)
            {
                var previousPosition = statsRun1.Where(s => s.SkierId == stat.SkierId).First().CurrentPosition;
                stat.DeltaPosition = previousPosition - stat.CurrentPosition;
            }

            return statsRun2;
        }
    }
}
