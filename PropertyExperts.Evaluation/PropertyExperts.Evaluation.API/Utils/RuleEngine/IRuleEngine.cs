using PropertyExperts.Evaluation.API.Models.DTOs;

namespace PropertyExperts.Evaluation.API.Utils.RuleEngine
{
    public interface IRuleEngine
    {
        public string[] GetActions(ClassificationResponseDto classificationResponse, decimal amount);
    }
}
