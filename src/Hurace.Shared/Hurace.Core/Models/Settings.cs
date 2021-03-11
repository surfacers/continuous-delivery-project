namespace Hurace.Core.Models
{
    public class Settings
    {
        public static Settings DefaultSettings = new Settings();

        private Settings()
        {
        }

        public int MinSkierAmount { get; private set; } = 30;
        public int MaxSkierAmount { get; private set; } = 120;
    }
}
