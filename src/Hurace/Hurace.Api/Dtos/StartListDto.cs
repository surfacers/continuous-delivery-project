namespace Hurace.Api.Dtos
{
    public class StartListDto
    {
        public int Id { get; set; }
        public int SkierId { get; set; }
        public int RaceId { get; set; }
        public int StartNumber { get; set; }
        public byte RunNumber { get; set; }
        public bool IsDisqualified { get; set; } = false;
    }
}
