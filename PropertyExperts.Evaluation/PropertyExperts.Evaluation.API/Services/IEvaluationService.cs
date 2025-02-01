using PropertyExperts.Evaluation.API.Models.DTOs;

namespace PropertyExperts.Evaluation.API.Services
{
    public interface IEvaluationService
    {
        Task<InvoiceEvaluateResponseDto> EvaluateInvoice(InvoiceEvaluateRequestDto request);
    }
}
