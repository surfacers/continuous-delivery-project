using System;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Api.Dtos;
using Hurace.Core.Logic.Models;
using Microsoft.AspNetCore.SignalR;

namespace Hurace.Api.Hubs
{
    public class RaceHub : Hub
    {
        private IMapper mapper;

        public RaceHub(
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task RunUpdate(LiveStatistic liveStatistic)
        {
            await this.Clients.All.SendAsync(
                "OnRunUpdate",
                this.mapper.Map<LiveStatisticDto>(liveStatistic));
        }

        public async Task CurrentRunChange(LiveStatistic liveStatistic)
        {
            await this.Clients.All.SendAsync(
                "OnCurrentRunChange", 
                this.mapper.Map<LiveStatisticDto>(liveStatistic));
        }

        public async Task RunStopped(string reason, LiveStatistic liveStatistic)
        {
            await this.Clients.All.SendAsync(
                "OnRunStopped", 
                reason,
                this.mapper.Map<LiveStatisticDto>(liveStatistic));
        }
    }
}
