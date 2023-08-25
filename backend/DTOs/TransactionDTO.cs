using System.Diagnostics.Eventing.Reader;

namespace backend.DTOs
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public double Amount { get; set; }  
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
