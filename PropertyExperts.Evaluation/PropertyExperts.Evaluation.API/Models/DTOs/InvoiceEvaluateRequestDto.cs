using System.ComponentModel.DataAnnotations;

namespace PropertyExperts.Evaluation.API.Models.DTOs
{
    public class InvoiceEvaluateRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string InvoiceId { get; set; }

        /// <summary>
        /// Added by the craftman
        /// </summary>
        [Required]
        [RegularExpression(@"^S\d{5}$", ErrorMessage = "Invoice number must start with 'S' followed by 5 digits.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Added by the insurance company
        /// </summary>
        [MaxLength(500, ErrorMessage = "Comment must not exceed 500 characters.")]
        public string? Comment { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

    }
}
