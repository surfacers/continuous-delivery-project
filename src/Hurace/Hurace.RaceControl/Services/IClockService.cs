using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Hurace.Timer;

namespace Hurace.RaceControl.Services
{
    public enum InvalidRaceDataReason
    {
        NotGranted = 0,
        NotEnoughTimeBetween = 1,
        NotInSensorRange = 2,
        NotStartedYet = 3,
    }

    public delegate void RunUpdate(RaceData raceData);
    public delegate Task RunFinished(int startListId);

    public interface IClockService
    {
        event Action<StartListItemViewModel> OnCurrentRunChange;
        event Action<InvalidRaceDataReason, int, DateTime> OnInvalid;
        event RunUpdate OnRunUpdate;
        event RunFinished OnRunFinish;

        public int RaceId { get; }
        public IList<RaceData> CurrentRunData { get; }
        public StartListItemViewModel CurrentRun { get; set; }
        public IRaceClock RaceClock { get; set; }
        public bool IsListening { get; }

        void StartListen(
            int raceId,
            int sensorAmount,
            StartListItemViewModel currentRun);

        IEnumerable<RaceData> Resume();

        void StopListen();
    }
}
