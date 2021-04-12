using System;
using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Exceptions;
using Hurace.Simulator.Models;
using Hurace.Timer;

namespace Hurace.Simulator
{
    public class SimulatedRaceClock : IRaceClock
    {
        private enum InvokeType
        {
            Invalid = -1,
            Success = 0,
            DoNotTrigger = 1,
            WrongSensor = 2,
            WrongDeltaTime = 3,
            MultipleSensors = 4
        }

        public event TimingTriggeredHandler TimingTriggered;

        private System.Timers.Timer timer;

        private int sensorId;
        private DateTime time;

        private ClockSettings settings;
        private Random random = new Random();
        
        public bool IsRunning => this.timer != null;

        public void Start(ClockSettings settings)
        {
            this.Stop();
            this.settings = settings;
            this.sensorId = -1;
            this.time = DateTime.Now;

            this.timer = new System.Timers.Timer();
            this.timer.Interval = this.random.Next(settings.MinInterval, settings.MaxInterval);
            this.timer.Elapsed += (_, __) => this.Invoke();
            this.timer.Start();
        }

        private IDictionary<InvokeType, int> GetWeights(ClockSettings settings)
        {
            var weights = new Dictionary<InvokeType, int>
            {
                { InvokeType.Success, settings.TriggerSuccessWeight },
                { InvokeType.DoNotTrigger, settings.DoNotTriggerWeight },
                { InvokeType.WrongSensor, settings.SetWrongSensorIdWeight },
                { InvokeType.WrongDeltaTime, settings.SetWrongDeltaTimeWeight },
                { InvokeType.MultipleSensors, settings.TriggerMultipleWeight }
            };

            return weights.Where(w => w.Value != 0).ToDictionary(w => w.Key, w => w.Value);
        }

        private InvokeType GetInvokeType(IDictionary<InvokeType, int> weights, int forWeight)
        {
            int currentWeight = 0;
            foreach (var weight in weights)
            {
                if (forWeight >= currentWeight && forWeight < weight.Value)
                {
                    return weight.Key; 
                }

                currentWeight += weight.Value;
            }

            return InvokeType.Invalid;
        }

        private void Invoke()
        {
            this.timer.Interval = this.random.Next(this.settings.MinInterval, this.settings.MaxInterval);

            DateTime wrongDeltaTime;

            lock (this)
            {
                this.sensorId++;
                if (this.sensorId >= this.settings.SensorAmount)
                {
                    this.sensorId = 0;
                }

                wrongDeltaTime = this.time.AddSeconds(this.settings.MaxRealInterval * 2);
                this.time = this.time.AddMilliseconds(this.random.Next(this.settings.MinRealInterval, this.settings.MaxRealInterval));
            }

            IDictionary<InvokeType, int> weights = this.GetWeights(this.settings);
            int totalWeight = weights.Select(w => w.Value).Sum();
            int weight = this.random.Next(0, totalWeight);

            InvokeType invokeType = this.GetInvokeType(weights, weight);
            switch (invokeType)
            {
                case InvokeType.Success:
                    this.TimingTriggered?.Invoke(this.sensorId, this.time);
                    break;

                case InvokeType.DoNotTrigger:
                    break;

                case InvokeType.WrongSensor:
                    this.TimingTriggered?.Invoke(-1, this.time);
                    break;

                case InvokeType.WrongDeltaTime:
                    this.TimingTriggered?.Invoke(this.sensorId, wrongDeltaTime);
                    break;

                case InvokeType.MultipleSensors:
                    this.TimingTriggered?.Invoke(this.sensorId, this.time);
                    this.TimingTriggered?.Invoke(this.sensorId, this.time);
                    break;

                default: throw new CaseNotImplementedException<InvokeType>(invokeType);
            }
        }

        public void Stop()
        {
            this.timer?.Stop();
            this.timer?.Dispose();
            this.timer = null;
        }
    }
}
