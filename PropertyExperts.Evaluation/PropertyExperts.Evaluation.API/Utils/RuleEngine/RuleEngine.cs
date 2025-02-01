using PropertyExperts.Evaluation.API.Models.DTOs;

namespace PropertyExperts.Evaluation.API.Utils.RuleEngine
{
    public class Rule
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Action { get; set; }
    }

    public class RuleEngine: IRuleEngine
    {
        public string[] GetActions(ClassificationResponseDto classificationResponse, decimal amount)
        {
            // TODO: Implement rule processing logic
            return ["Review","Accept"];
        }

    }
}
