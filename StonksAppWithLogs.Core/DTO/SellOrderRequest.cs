using System.ComponentModel.DataAnnotations;

namespace StonksAppWithLogs.Core.DTO
{
    public class SellOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000")]

        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Value should be between 1 and 10000")]
        public double Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new();

            if (DateAndTimeOfOrder < Convert.ToDateTime("2020-01-01"))
            {
                results.Add(new ValidationResult("Should not be older than Jan 01, 2000"));
            }

            return results;
        }
    }

}
