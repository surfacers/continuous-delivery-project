using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hurace.Core.Enums;
using Hurace.Core.Models;

namespace Hurace.Logic.Test
{
    public class Data
    {
        public Data()
        {
            this.Setup();
        }

        public int Id { get; set; }
        public List<Location> Locations { get; private set; }
        public List<Skier> Skiers { get; private set; }
        public List<Race> Races { get; private set; }
        public List<StartList> StartLists { get; private set; }
        public List<RaceData> RaceDatas { get; private set; }

        private void Setup()
        {
            this.Locations = new[]
            {
                new Location { Id = 1, City = "Hagenberg", CountryCode = "AUT" },
                new Location { Id = 2, City = "Linz", CountryCode = "AUT" },
                new Location { Id = 3, City = "Passau", CountryCode = "GER" },
            }.ToList();

            this.Skiers = new[]
            {
                // Male
                new Skier { Id = 9, FirstName = "Davis", LastName = "Taylor", CountryCode = "FRA", BirthDate = new DateTime(1982, 07, 05, 06, 01, 16), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = true },
                new Skier { Id = 10, FirstName = "Quinn", LastName = "Obrien", CountryCode = "FRA", BirthDate = new DateTime(1997, 07, 07, 04, 17, 04), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = true },
                new Skier { Id = 11, FirstName = "Malcolm", LastName = "Cantu", CountryCode = "FRA", BirthDate = new DateTime(1981, 05, 25, 0, 17, 38), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = true },
                new Skier { Id = 12, FirstName = "Marsden", LastName = "Fisher", CountryCode = "FRA", BirthDate = new DateTime(1998, 08, 17, 15, 57, 44), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = true },
                new Skier { Id = 13, FirstName = "Gray", LastName = "Cannon", CountryCode = "FRA", BirthDate = new DateTime(1977, 03, 23, 15, 13, 27), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = true },
                new Skier { Id = 16, FirstName = "Amir", LastName = "Love", CountryCode = "GER", BirthDate = new DateTime(1977, 11, 16, 0, 09, 06), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = false },
                new Skier { Id = 17, FirstName = "Steven", LastName = "Garcia", CountryCode = "GER", BirthDate = new DateTime(1998, 10, 08, 19, 55, 42), Image = null, IsRemoved = false, Gender = Gender.Male, IsActive = false },

                // Female
                new Skier { Id = 4, FirstName = "Linda", LastName = "Sherman", CountryCode = "AUT", BirthDate = new DateTime(1992, 09, 09, 18, 13, 33), Image = null, IsRemoved = false, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 5, FirstName = "Isabelle", LastName = "Velez", CountryCode = "AUT", BirthDate = new DateTime(1991, 03, 01, 03, 51, 09), Image = null, IsRemoved = false, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 6, FirstName = "Cassandra", LastName = "David", CountryCode = "AUT", BirthDate = new DateTime(1990, 09, 20, 04, 01, 15), Image = null, IsRemoved = false, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 7, FirstName = "Miranda", LastName = "Barrett", CountryCode = "AUT", BirthDate = new DateTime(1993, 05, 31, 0, 49, 38), Image = null, IsRemoved = false, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 8, FirstName = "Ebony", LastName = "Hensley", CountryCode = "AUT", BirthDate = new DateTime(1990, 07, 17, 18, 20, 11), Image = null, IsRemoved = false, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 14, FirstName = "Susan", LastName = "Leach", CountryCode = "SUI", BirthDate = new DateTime(1995, 05, 28, 03, 06, 27), Image = null, IsRemoved = true, Gender = Gender.Female, IsActive = true },
                new Skier { Id = 15, FirstName = "Riley", LastName = "Roberts", CountryCode = "FRA", BirthDate = new DateTime(1981, 01, 23, 06, 04, 34), Image = null, IsRemoved = true, Gender = Gender.Female, IsActive = true },
            }.ToList();

            this.Races = new[]
            {
                // Male
                    // NotStarted
                    new Race { Id = 18, Name = "Levi", Description = "Night Slalom in Levi", RaceDate = new DateTime(2019, 11, 24, 14, 0, 0, 0), RaceType = RaceType.Slalom, LocationId = 1, SensorAmount = 5, RaceState = RaceState.NotStarted, Gender = Gender.Male },
                    new Race { Id = 19, Name = "Lake Louise First Downhill this year", RaceDate = new DateTime(2019, 11, 30, 13, 0, 0, 0), RaceType = RaceType.Downhill, LocationId = 2, SensorAmount = 4, RaceState = RaceState.NotStarted, Gender = Gender.Male },
                
                    // Running
                    new Race { Id = 20, Name = "Beaver Creek", Description = "First race in BC", RaceDate = new DateTime(2019, 12, 06, 12, 0, 0, 0), RaceType = RaceType.SuperG, LocationId = 3, SensorAmount = 5, RaceState = RaceState.Running, Gender = Gender.Male },

                    // Done
                    new Race { Id = 21, Name = "Gröden", Description = null, RaceDate = new DateTime(2019, 12, 20, 12, 0, 0, 0), RaceType = RaceType.SuperG, LocationId = 1, SensorAmount = 5, RaceState = RaceState.Done, Gender = Gender.Male },
                    new Race { Id = 22, Name = "Gröden", Description = "Downhill in Gröden", RaceDate = new DateTime(2019, 12, 21, 12, 30, 0, 0), RaceType = RaceType.Slalom, LocationId = 2, SensorAmount = 6, RaceState = RaceState.Done, Gender = Gender.Male },
                    new Race { Id = 23, Name = "Alta Badia", Description = null, RaceDate = new DateTime(2019, 12, 22, 14, 0, 0, 0), RaceType = RaceType.GiantSlalom, LocationId = 3, SensorAmount = 5, RaceState = RaceState.Done, Gender = Gender.Male },

                // Female
                    // NotStarted
                    new Race { Id = 24, Name = "Lake Louise First Super-G this year", RaceDate = new DateTime(2019, 12, 1, 9, 0, 0, 0), RaceType = RaceType.SuperG, LocationId = 2, SensorAmount = 6, RaceState = RaceState.NotStarted, Gender = Gender.Female },
                
                    // Running
                    new Race { Id = 25, Name = "Beaver Creek", Description = "The hardest race in BC", RaceDate = new DateTime(2019, 12, 07, 13, 0, 0, 0), RaceType = RaceType.Downhill, LocationId = 3, SensorAmount = 7, RaceState = RaceState.Running, Gender = Gender.Female },
                    new Race { Id = 26, Name = "Beaver Creek", Description = null, RaceDate = new DateTime(2019, 12, 8, 14, 0, 0, 0), RaceType = RaceType.GiantSlalom, LocationId = 3, SensorAmount = 5, RaceState = RaceState.Running, Gender = Gender.Female },
                    new Race { Id = 27, Name = "Val d'Isere", Description = null, RaceDate = new DateTime(2019, 12, 14, 13, 30, 0, 0), RaceType = RaceType.Downhill, LocationId = 2, SensorAmount = 5, RaceState = RaceState.Running, Gender = Gender.Female },
                
                    // Done
                    new Race { Id = 28, Name = "Val d'Isere", Description = null, RaceDate = new DateTime(2019, 12, 15, 14, 15, 0, 0), RaceType = RaceType.Downhill, LocationId = 1,    SensorAmount = 5, RaceState = RaceState.Done, Gender = Gender.Female },
                    new Race { Id = 29, Name = "Alta Badia", Description = null, RaceDate = new DateTime(2019, 12, 23, 13, 15, 0, 0), RaceType = RaceType.SuperG, LocationId = 3, SensorAmount = 5, RaceState = RaceState.Done, Gender = Gender.Female }
            }.ToList();

            this.Id = 30;

            this.StartLists = new List<StartList>();
            this.RaceDatas = new List<RaceData>();

            this.SetupStartList(raceId: 20, runNumber: 1, skierAmount: 5);
            this.SetupStartList(raceId: 21, runNumber: 1, skierAmount: 4);
            this.SetupStartList(raceId: 22, runNumber: 1, skierAmount: 6);
            this.SetupStartList(raceId: 22, runNumber: 2, skierAmount: 6);
            this.SetupStartList(raceId: 23, runNumber: 1, skierAmount: 6);
            this.SetupStartList(raceId: 23, runNumber: 2, skierAmount: 6);

            this.SetupStartList(raceId: 25, runNumber: 1, skierAmount: 5);
            this.SetupStartList(raceId: 26, runNumber: 1, skierAmount: 4);
            this.SetupStartList(raceId: 26, runNumber: 2, skierAmount: 4);
            this.SetupStartList(raceId: 27, runNumber: 1, skierAmount: 6);
            this.SetupStartList(raceId: 28, runNumber: 1, skierAmount: 6);
            this.SetupStartList(raceId: 29, runNumber: 1, skierAmount: 3);
        }

        private void SetupStartList(int raceId, int runNumber, int skierAmount)
        {
            Race race = this.Races.Where(r => r.Id == raceId).First();
            var startLists = this.Skiers
                .Where(s => s.Gender == race.Gender)
                .Take(skierAmount)
                .Select((skier, i) => new StartList
                {
                    Id = this.Id++,
                    IsDisqualified = false,
                    RaceId = raceId,
                    RunNumber = (byte)runNumber,
                    SkierId = skier.Id,
                    StartNumber = i
                });

            DateTime startTime = race.RaceDate;
            foreach (var startList in startLists)
            {
                this.StartLists.Add(startList);
                this.RaceDatas.AddRange(this.CreateRaceData(startList.Id, race.SensorAmount, startTime));
            }
        }

        private IEnumerable<RaceData> CreateRaceData(int startListId, int sensorAmount, DateTime startTime)
        {
            return Enumerable
                .Range(1, sensorAmount)
                .Select(i => new RaceData
                {
                    Id = this.Id++,
                    SensorId = (byte)i,
                    StartListId = startListId,
                    TimeStamp = startTime = startTime.AddSeconds(i)
                });
        }
    }
}
