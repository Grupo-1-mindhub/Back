namespace backend.DTOs
{
    public class BudgetDTO
    {
        public long Id { get; set; }

        public double Balance { get; set; }

        public ICollection<AccountDTO> Accounts { get; set; }
  
    }
}
