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
        
        public bool IsRunning => timer != null;

        public void Start(ClockSettings settings)
        {
            Stop();
            this.settings = settings;
            this.sensorId = -1;
            this.time = DateTime.Now;

            timer = new System.Timers.Timer();
            timer.Interval = random.Next(settings.MinInterval, settings.MaxInterval);
            timer.Elapsed += (_, __) => Invoke();
            timer.Start();
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
            timer.Interval = random.Next(settings.MinInterval, settings.MaxInterval);

            DateTime wrongDeltaTime;

            lock (this)
            {
                sensorId++;
                if (sensorId >= settings.SensorAmount)
                {
                    sensorId = 0;
                }

                wrongDeltaTime = time.AddSeconds(settings.MaxRealInterval * 2);
                time = time.AddMilliseconds(random.Next(settings.MinRealInterval, settings.MaxRealInterval));
            }

            IDictionary<InvokeType, int> weights = GetWeights(settings);
            int totalWeight = weights.Select(w => w.Value).Sum();
            int weight = random.Next(0, totalWeight);

            InvokeType invokeType = GetInvokeType(weights, weight);
            switch(invokeType)
            {
                case InvokeType.Success:
                    TimingTriggered?.Invoke(sensorId, time);
                    break;

                case InvokeType.DoNotTrigger:
                    break;

                case InvokeType.WrongSensor:
                    TimingTriggered?.Invoke(-1, time);
                    break;

                case InvokeType.WrongDeltaTime:
                    TimingTriggered?.Invoke(sensorId, wrongDeltaTime);
                    break;

                case InvokeType.MultipleSensors:
                    TimingTriggered?.Invoke(sensorId, time);
                    TimingTriggered?.Invoke(sensorId, time);
                    break;

                default: throw new CaseNotImplementedException<InvokeType>(invokeType);
            }
        }

        public void Stop()
        {
            timer?.Stop();
            timer?.Dispose();
            timer = null;
        }
    }
}
