using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PropertyExperts.Evaluation.API.Controllers;
using PropertyExperts.Evaluation.API.Models.DTOs;
using PropertyExperts.Evaluation.API.Services;

namespace PropertyExperts.Evaluation.UnitTests.Controller
{
    [TestFixture]
    public class EvaluactionControllerTests
    {
        private Mock<IEvaluationService> mockEvaluationService;
        private Mock<ILogger<EvaluationController>> mockLogger;
        private EvaluationController controller;
        private IFormFile validFile;

        [SetUp]
        public void SetUp()
        {
            mockEvaluationService = new Mock<IEvaluationService>();
            mockLogger = new Mock<ILogger<EvaluationController>>();
            controller = new EvaluationController(mockEvaluationService.Object, mockLogger.Object);
            validFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("invoice")), 0, 10, "invoice", "invoice.pdf");
        }

        [Test]
        public async Task EvaluateInvoice_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            var requestDto = new InvoiceEvaluateRequestDto
            {
                File = null,
                Amount = 20,
                InvoiceId = "12345",
                InvoiceNumber = "S12345",
                InvoiceDate = DateTime.Now,
            };

            // Act 
            var result = controller.EvaluateInvoice(requestDto);

            // Assert
            var ex = Assert.ThrowsAsync<BadHttpRequestException>(async () => await result);
            Assert.AreEqual("Invoice document is required.", ex.Message);
        }
    }
}
