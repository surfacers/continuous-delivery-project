using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;

namespace Hurace.Logic
{
    public class SeasonLogic : ISeasonLogic
    {
        public Task<IEnumerable<Season>> GetAllAsync()
        {
            return Task.FromResult(new[]
            {
                new Season { Name = "2019/20", From = new DateTime(2019, 7, 1), To = new DateTime(2020, 6, 30) },
                new Season { Name = "2018/19", From = new DateTime(2018, 7, 1), To = new DateTime(2019, 6, 30) }
            }.AsEnumerable());
        }
    }
}
