using System.Net.Http.Headers;
using System.Text;

namespace PropertyExperts.Evaluation.Tests.Integration
{
    [TestFixture]
    public class EvaluationControllerTests
    {
        [Test]
        public async Task EvaluateRequst_ReturnEvaluation()
        {
            // Arrange
            var application = new EvaluationAPIWebApplicatinFactoy();
            var client = application.CreateClient();

            var fileContent = this.CreateTestFileContent();
            var request = new MultipartFormDataContent();
            request.Add(fileContent, "file", "test.pdf");
            //request.Add(new StreamContent(formFile.OpenReadStream()), "file", formFile.FileName);
            request.Add(new StringContent("20"), "amount");
            request.Add(new StringContent("Test Comment"), "comment");
            request.Add(new StringContent(DateTime.Now.ToString("o")), "invoiceDate");
            request.Add(new StringContent("INV09123"), "invoiceId");
            request.Add(new StringContent("S12345"), "invoiceNumber");

            // Act
            var response = await client.PostAsync("https://localhost:7190/evaluate", request);
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(result, Is.Not.Empty);
        }

        private StreamContent CreateTestFileContent()
        {
            var content = "This is a test invoice file.";
            var byteArray = Encoding.UTF8.GetBytes(content);
            var ms = new MemoryStream(byteArray);
            var fileContent = new StreamContent(ms);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return fileContent;

        }
    }
}
