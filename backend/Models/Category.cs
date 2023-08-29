namespace backend.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }

    }
}
