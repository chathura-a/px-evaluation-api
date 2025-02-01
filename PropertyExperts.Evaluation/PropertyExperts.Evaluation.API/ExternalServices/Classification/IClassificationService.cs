using System.IO;
using PropertyExperts.Evaluation.API.Models.DTOs;
using RestSharp;

namespace PropertyExperts.Evaluation.API.ExternalServices.Classification
{
    public interface IClassificationService
    {
        Task<ClassificationResponseDto> GetClassification(IFormFile file);
    }
}
