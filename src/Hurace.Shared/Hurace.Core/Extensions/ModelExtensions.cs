using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Enums;
using Hurace.Core.Models;

namespace Hurace.Core.Extensions
{
    public static class ModelExtensions
    {
        public static string FullName(this Skier skier)
        {
            return $"{skier?.FirstName} {skier?.LastName}".Trim();
        }

        public static bool HasSecondRun(this Race race)
        {
            return race.RaceType == RaceType.Slalom || race.RaceType == RaceType.GiantSlalom;
        }

        public static StartListState GetStartListState(
            this StartList startList,
            IEnumerable<RaceData> raceData,
            int sensorAmount,
            int runningStartListId)
        {
            if (startList.IsDisqualified)
            {
                return StartListState.Disqualified;
            }

            if (startList.Id == runningStartListId)
            {
                return StartListState.Running;
            }

            if (raceData.Any(d => d.SensorId == sensorAmount))
            {
                return StartListState.Done;
            }

            return StartListState.NotStarted;
        }
    }
}
