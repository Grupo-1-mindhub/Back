namespace backend.DTOs
{
    public class PaymentMethodDTO { 
        public long Id { get; set; }
        public string Description { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }
    }
}
