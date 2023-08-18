namespace backend.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Budget? Budget { get; set; }
        public long BudgetId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public long PaymentMethodId { get; set; }
    }
}
