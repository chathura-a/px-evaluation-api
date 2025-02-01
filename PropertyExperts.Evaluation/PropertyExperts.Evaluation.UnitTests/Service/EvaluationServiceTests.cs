using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using PropertyExperts.Evaluation.API.ExternalServices.Classification;
using PropertyExperts.Evaluation.API.Models.DTOs;
using PropertyExperts.Evaluation.API.Services;
using PropertyExperts.Evaluation.API.Utils.RuleEngine;

namespace PropertyExperts.Evaluation.UnitTests.Service
{
    [TestFixture]
    public class EvaluationServiceTests
    {
        private Mock<ILogger<EvaluationService>> logger;
        private Mock<IClassificationService> mockClassificationService;
        private Mock<IRuleEngine> mockRuleEngine;
        private EvaluationService evaluationService;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger<EvaluationService>>();
            mockClassificationService = new Mock<IClassificationService>();
            mockRuleEngine = new Mock<IRuleEngine> { CallBase = true };
            evaluationService = new EvaluationService(
                logger.Object,
                mockClassificationService.Object,
                mockRuleEngine.Object);
        }

        [Test]
        public async Task EvaluateInvoice_ReturnsCorrectResponse()
        {
            //Arrange
            var mockFile = new Mock<IFormFile>();

            var request = new InvoiceEvaluateRequestDto
            {
                File = mockFile.Object,
                Amount = 20,
                Comment = "Test Comment",
                InvoiceDate = DateTime.Now,
                InvoiceId = "INV09123",
                InvoiceNumber = "S12345"
            };

            var classification = new ClassificationResponseDto
            {
                Classification = "WaterLeakDetected",
                RiskLevel = "Low"
            };

            var rules = new string[1] { "Accept" };

            mockClassificationService.Setup(s => s.GetClassification(request.File)).ReturnsAsync(classification);
            mockRuleEngine.Setup(s => s.GetActions(classification, request.Amount)).Returns(rules);

            // Act
            var result = await evaluationService.EvaluateInvoice(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.InvoiceId, Is.EqualTo(request.InvoiceId));
                Assert.That(result.Classification, Is.EqualTo(classification.Classification));
                Assert.That(result.RulesApplied, Is.EqualTo(rules));
            });
        }

        [Test]
        public async Task EvaluateInvoice_CallsDependanciesCorrectly()
        {
            //Arrange
            var mockFile = new Mock<IFormFile>();

            var request = new InvoiceEvaluateRequestDto
            {
                File = mockFile.Object,
                Amount = 20,
                Comment = "Test Comment",
                InvoiceDate = DateTime.Now,
                InvoiceId = "INV09123",
                InvoiceNumber = "S12345"
            };

            var classification = new ClassificationResponseDto
            {
                Classification = "WaterLeakDetected",
                RiskLevel = "Low"
            };

            var rules = new string[1] { "Accept" };

            mockClassificationService.Setup(s => s.GetClassification(request.File)).ReturnsAsync(classification);
            mockRuleEngine.Setup(s => s.GetActions(classification, request.Amount)).Returns(rules);

            // Act
            await evaluationService.EvaluateInvoice(request);

            // Assert
            mockClassificationService.Verify(s => s.GetClassification(request.File), Times.Once);
            mockRuleEngine.Verify(s => s.GetActions(classification, request.Amount), Times.Once);
        }
    }
}
