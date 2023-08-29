using backend.Models;

namespace backend.DTOs
{
    public class MonthDTO
    {
        public double Amount { get; set; }
        public MonthsType Month { get; set; }
        public int Year { get; set; }
      
    }
}
