namespace PropertyExperts.Evaluation.API.Models.DTOs
{
    public class InvoiceEvaluateResponseDto
    {
        public string EvaluationId { get; set; }
        public string InvoiceId { get; set; }
        public string[] RulesApplied { get; set; }
        public string Classification { get; set; }

        public string EvaluationFile { get; set; }

    }
}
