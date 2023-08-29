namespace backend.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Account? Account { get; set; }
        public long AccountId { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public long PaymentMethodId { get; set; }
    }
}
