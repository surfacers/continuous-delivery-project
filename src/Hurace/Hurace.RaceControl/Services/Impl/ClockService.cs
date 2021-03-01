using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Mvvm;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Hurace.Timer;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.Services.Impl
{
    public class ClockService : NotifyPropertyChanged, IClockService
    {
        private readonly IRaceDataLogic raceDataLogic;
        private readonly INotificationService notificationService;

        public ClockService(
            IRaceDataLogic raceDataLogic,
            INotificationService notificationService)
        {
            this.raceDataLogic = raceDataLogic ?? throw new ArgumentNullException(nameof(raceDataLogic));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public IList<RaceData> CurrentRunData { get; } = new List<RaceData>();

        private int lastSensorId;
        private int sensorAmount;
        private TimeSpan deltaTime = TimeSpan.FromSeconds(3);

        private IRaceClock raceClock;
        public IRaceClock RaceClock
        {
            get => raceClock;
            set
            {
                StopListen();
                raceClock = value;

                if (raceClock != null)
                {
                    RaceClock.TimingTriggered += TimingTriggered;
                }
            }
        }

        public int RaceId { get; private set; }
        public bool HasStarted { get; private set; }

        private StartListItemViewModel currentRun;
        public StartListItemViewModel CurrentRun
        {
            get => currentRun;
            set
            {
                currentRun = value;
                OnCurrentRunChange?.Invoke(value);
            }
        }

        public event Action<StartListItemViewModel> OnCurrentRunChange;
        public event Action<InvalidRaceDataReason, int, DateTime> OnInvalid;
        public event RunUpdate OnRunUpdate;
        public event RunFinished OnRunFinish;

        private bool isListening = false;
        public bool IsListening
        {
            get => isListening;
            private set => Set(ref isListening, value);
        }

        public void StartListen(
            int raceId,
            int sensorAmount,
            StartListItemViewModel currentRun)
        {
            lock (this)
            {
                StopListen();
                IsListening = true;
                HasStarted = false;

                RaceId = raceId;
                this.sensorAmount = sensorAmount;
                CurrentRun = currentRun;
            }
        }

        public IEnumerable<RaceData> Resume()
            => CurrentRunData;

        public void StopListen()
        {
            CurrentRun = null;
            RaceId = -1;
            CurrentRunData.Clear();
            HasStarted = false;
            IsListening = false;
        }

        private void TimingTriggered(int sensorId, DateTime time)
        {
            // Check if access was granted
            if (!IsListening)
            {
                OnInvalid?.Invoke(InvalidRaceDataReason.NotGranted, sensorId, time);
                return;
            }

            // Not enough time was between
            DateTime? lastTime = CurrentRunData.LastOrDefault()?.TimeStamp;
            if (lastTime != null && (time - (DateTime)lastTime) < deltaTime)
            {
                OnInvalid?.Invoke(InvalidRaceDataReason.NotEnoughTimeBetween, sensorId, time);
                return;
            }

            if (sensorId < 0 || sensorId >= sensorAmount || (HasStarted && sensorId <= lastSensorId))
            {
                OnInvalid?.Invoke(InvalidRaceDataReason.NotInSensorRange, sensorId, time);
                return;
            }

            // Start
            if (sensorId == 0)
            {
                HasStarted = true;
            }

            if (!HasStarted)
            {
                OnInvalid?.Invoke(InvalidRaceDataReason.NotStartedYet, sensorId, time);
                return;
            }

            lastSensorId = sensorId;
            HandleValidTimingTriggered(sensorId, time);
        }

        private void HandleValidTimingTriggered(int sensorId, DateTime time)
        {
            Console.WriteLine($"{sensorId}: {time.ToLongTimeString()}");

            var raceData = new Models.RaceData
            {
                Id = 0,
                SensorId = (byte)(sensorId + 1), // is 1-based in database but 0-based in IRaceTimer
                StartListId = CurrentRun.StartList.Id,
                TimeStamp = time
            };

            CurrentRunData.Add(raceData);
            OnRunUpdate?.Invoke(raceData);

            // End
            if (sensorId == (sensorAmount - 1))
            {
                Task.WaitAll(SaveRaceData());

                lock (this)
                {
                    Task.WaitAll(OnRunFinish?.Invoke(CurrentRun.StartList.Id) ?? Task.CompletedTask);
                }

                StopListen();
            }
        }

        private async Task SaveRaceData()
        {
            bool successful = await raceDataLogic.CreateAsync(CurrentRun.RaceData.Select(s => s.RaceData));
            if (!successful)
            {
                this.notificationService.ShowMessage("Save race data failed!");
            }
        }
    }
}
