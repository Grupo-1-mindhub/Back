namespace backend.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public string? Number { get; set; }
        public DateTime Deadline { get; set; }
        public Client Client { get; set; }
        public long ClientId { get; set; }
    }
}