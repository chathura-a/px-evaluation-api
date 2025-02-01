using System.Text;
using Newtonsoft.Json;
using PropertyExperts.Evaluation.API.ExternalServices.Classification;
using PropertyExperts.Evaluation.API.Models.DTOs;
using PropertyExperts.Evaluation.API.Utils.RuleEngine;

namespace PropertyExperts.Evaluation.API.Services
{
    public class EvaluationService: IEvaluationService
    {
        private readonly ILogger<EvaluationService> logger;
        private readonly IClassificationService classificationService;
        private readonly IRuleEngine ruleEngine;

        public EvaluationService(ILogger<EvaluationService> logger, IClassificationService classificationService, IRuleEngine ruleEngine)
        {
            this.logger = logger;
            this.classificationService = classificationService;
            this.ruleEngine = ruleEngine;
        }

        public async Task<InvoiceEvaluateResponseDto> EvaluateInvoice(InvoiceEvaluateRequestDto request)
        {
            this.logger.LogInformation("EvaluateInvoice : {invoiceId}", JsonConvert.SerializeObject(request.InvoiceId));
            var classification = await this.classificationService.GetClassification(request.File);
            var rules = this.ruleEngine.GetActions(classification, request.Amount);

            return new InvoiceEvaluateResponseDto
            {
                EvaluationId = this.GetEvaluationId(),
                InvoiceId = request.InvoiceId ,
                Classification = classification.Classification,    
                RulesApplied = rules,
                EvaluationFile = this.GenerateEvaluationSummary(classification, rules)
            };
        }

        private string GetEvaluationId()
        {
            Random random = new();
            var fourDigitNumber = random.Next(1000, 10000);
            return $"EVAL{fourDigitNumber}";
        }
        private string GenerateEvaluationSummary(ClassificationResponseDto classificationResult, string[] rules) 
        {
            var actionsString = string.Empty;

            if(rules.Length >= 1) 
            {
                actionsString += $"We recommand {rules.First()}";
            }

            if (rules.Length >= 2) {

                for (int i = 1; i < rules.Length; i++) 
                {
                    if(i == rules.Length- 1)
                    {
                        actionsString += $" and {rules[i]}";
                    }
                    else
                    {
                        actionsString += $", {rules[i]}";
                    }
                }
            }
            var content = $"The claim request has identified as a {classificationResult.Classification} with {classificationResult.RiskLevel} severity. {actionsString}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
    }
}
