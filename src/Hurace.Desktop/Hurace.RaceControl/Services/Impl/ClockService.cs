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
            get => this.raceClock;
            set
            {
                this.StopListen();
                this.raceClock = value;

                if (this.raceClock != null)
                {
                    this.RaceClock.TimingTriggered += this.TimingTriggered;
                }
            }
        }

        public int RaceId { get; private set; }
        public bool HasStarted { get; private set; }

        private StartListItemViewModel currentRun;
        public StartListItemViewModel CurrentRun
        {
            get => this.currentRun;
            set
            {
                this.currentRun = value;
                this.OnCurrentRunChange?.Invoke(value);
            }
        }

        public event Action<StartListItemViewModel> OnCurrentRunChange;
        public event Action<InvalidRaceDataReason, int, DateTime> OnInvalid;
        public event RunUpdate OnRunUpdate;
        public event RunFinished OnRunFinish;

        private bool isListening = false;
        public bool IsListening
        {
            get => this.isListening;
            private set => this.Set(ref this.isListening, value);
        }

        public void StartListen(
            int raceId,
            int sensorAmount,
            StartListItemViewModel currentRun)
        {
            lock (this)
            {
                this.StopListen();
                this.IsListening = true;
                this.HasStarted = false;

                this.RaceId = raceId;
                this.sensorAmount = sensorAmount;
                this.CurrentRun = currentRun;
            }
        }

        public IEnumerable<RaceData> Resume()
            => this.CurrentRunData;

        public void StopListen()
        {
            this.CurrentRun = null;
            this.RaceId = -1;
            this.CurrentRunData.Clear();
            this.HasStarted = false;
            this.IsListening = false;
        }

        private void TimingTriggered(int sensorId, DateTime time)
        {
            // Check if access was granted
            if (!this.IsListening)
            {
                this.OnInvalid?.Invoke(InvalidRaceDataReason.NotGranted, sensorId, time);
                return;
            }

            // Not enough time was between
            DateTime? lastTime = this.CurrentRunData.LastOrDefault()?.TimeStamp;
            if (lastTime != null && (time - (DateTime)lastTime) < this.deltaTime)
            {
                this.OnInvalid?.Invoke(InvalidRaceDataReason.NotEnoughTimeBetween, sensorId, time);
                return;
            }

            if (sensorId < 0 || sensorId >= this.sensorAmount || (this.HasStarted && sensorId <= this.lastSensorId))
            {
                this.OnInvalid?.Invoke(InvalidRaceDataReason.NotInSensorRange, sensorId, time);
                return;
            }

            // Start
            if (sensorId == 0)
            {
                this.HasStarted = true;
            }

            if (!this.HasStarted)
            {
                this.OnInvalid?.Invoke(InvalidRaceDataReason.NotStartedYet, sensorId, time);
                return;
            }

            this.lastSensorId = sensorId;
            this.HandleValidTimingTriggered(sensorId, time);
        }

        private void HandleValidTimingTriggered(int sensorId, DateTime time)
        {
            Console.WriteLine($"{sensorId}: {time.ToLongTimeString()}");

            var raceData = new Models.RaceData
            {
                Id = 0,
                SensorId = (byte)(sensorId + 1), // is 1-based in database but 0-based in IRaceTimer
                StartListId = this.CurrentRun.StartList.Id,
                TimeStamp = time
            };

            this.CurrentRunData.Add(raceData);
            this.OnRunUpdate?.Invoke(raceData);

            // End
            if (sensorId == (this.sensorAmount - 1))
            {
                Task.WaitAll(this.SaveRaceData());

                lock (this)
                {
                    Task.WaitAll(this.OnRunFinish?.Invoke(this.CurrentRun.StartList.Id) ?? Task.CompletedTask);
                }

                this.StopListen();
            }
        }

        private async Task SaveRaceData()
        {
            bool successful = await this.raceDataLogic.CreateAsync(this.CurrentRun.RaceData.Select(s => s.RaceData));
            if (!successful)
            {
                this.notificationService.ShowMessage("Save race data failed!");
            }
        }
    }
}
