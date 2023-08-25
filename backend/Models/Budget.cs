namespace backend.Models
{
    public class Budget
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public Account? Account { get; set; }
        public long AccountId { get; set; }
        public Category? Category { get; set; }
        public long CategoryId { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
     
    }
}
