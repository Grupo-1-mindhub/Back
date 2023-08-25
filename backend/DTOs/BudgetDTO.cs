namespace backend.DTOs
{
    public class BudgetDTO
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }

    }
}
