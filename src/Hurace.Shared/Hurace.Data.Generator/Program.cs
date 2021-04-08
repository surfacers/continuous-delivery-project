using System;

namespace Hurace.Data.Generator
{
    public class Program
    {
        // DB Information:
        // Skiers: female Skiers => Id-Rang [203; 302]      |       male Skiers => Id-Range [303; 402]
        // Races: valid IDs [2,3,8,9,10,12,13,15,16,17,18,19,20,21,22,23,24,25,26,27,29,30]

        public static void PrintSqlStatement(string statement)
        {
            Console.WriteLine(statement);
        }

        public static void GenStartList(int skierStartIndex, int skierAmount, int run, int raceId)
        {
            int startNumber = 1;
            int lastSkierId = skierAmount + skierStartIndex;
            for (int i = skierStartIndex; i < lastSkierId; i++)
            {
                string statement = $"INSERT INTO StartList(SkierId, RaceId, StartNumber, RunNumber, IsDisqualified) VALUES({i}, {raceId}, {startNumber}, {run}, {0})";
                PrintSqlStatement(statement);
                startNumber++;
            }
        }

        public static void GenSensorData(int startListStartIndex, int skierAmount, int sensorAmount, DateTime raceDate)
        {
            int lastStartListId = startListStartIndex + skierAmount;
            Random rand = new Random();
            
            // add start delay of 1 - 5 min
            raceDate = raceDate.AddMinutes(rand.Next(1, 6));
            DateTime sensorTime = raceDate;

            for (int i = startListStartIndex; i < lastStartListId; i++)
            {
                sensorTime = sensorTime.AddSeconds(rand.Next(1, 60));
                for (int j = 1; j <= sensorAmount; j++)
                {
                    string statement = $"INSERT INTO RaceData(StartListId, SensorId, TimeStamp) VALUES({i}, {j}, '{sensorTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}')";
                    PrintSqlStatement(statement);
                    sensorTime = sensorTime.AddSeconds(rand.Next(10, 31));
                    sensorTime = sensorTime.AddMilliseconds(rand.Next(50, 1000));
                }

                sensorTime = sensorTime.AddMinutes(1);
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("============ Welcome to Hurace.Data.Generator ============");

#if false
            // Upcoming Race StartList for a race with only one run (male)
            GenStartList(310, 60, 1, 17);
            // Upcoming Race StartList for a race with two runs (female)
            GenStartList(240, 60, 1, 8);
            // Finished Race StartList for a race with only one run (male)
            GenStartList(330, 55, 1, 29);
            // Finished Race StartList for a race with two runs (female)
            GenStartList(210, 55, 1, 30);
#endif
#if false
            // Finished RaceData (SensorData) for a race with only one run
            GenSensorData(836, 55, 6, new DateTime(2019, 10, 15, 13, 00, 0));
            // Finished RaceData (SensorData) for a race with only one run
            GenSensorData(891, 55, 4, new DateTime(2019, 10, 19, 12, 30, 0)); // first run 
            GenSensorData(946, 40, 4, new DateTime(2019, 10, 19, 16, 00, 0)); // second run
#endif
            
            Console.WriteLine("========================== END ===========================");
        }
    }
}
