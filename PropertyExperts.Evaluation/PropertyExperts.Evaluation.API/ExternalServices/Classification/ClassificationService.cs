using Polly.Registry;
using PropertyExperts.Evaluation.API.Models.DTOs;
using PropertyExperts.Evaluation.API.Utils.FileUtils;
using RestSharp;

namespace PropertyExperts.Evaluation.API.ExternalServices.Classification
{
    public class ClassificationService : IClassificationService
    {
        private readonly ILogger<ClassificationService> logger;
        private readonly IConfiguration configuration;
        private readonly IRestClient restClient;
        private readonly ResiliencePipelineProvider<string> resiliencePipeline;

        public ClassificationService(IRestClient restClient, ResiliencePipelineProvider<string> resiliencePipeline, ILogger<ClassificationService> logger, IConfiguration configuration)
        {
            this.restClient = restClient;
            this.resiliencePipeline = resiliencePipeline;
            this.logger = logger;
            this.configuration = configuration;
            
        }
        public async Task<ClassificationResponseDto> GetClassification(IFormFile file)
        {
            var byteArray = await FileUtils.ConvertFormFileToByteArray(file);

            var pipeline = this.resiliencePipeline.GetPipeline("default");

            var classificationApi = this.configuration.GetValue<string>("ExternalAPIs:ClassificationAPI");

            var request = new RestRequest(classificationApi);
            request.AddFile("file", byteArray, file.FileName, "application/pdf");

            var classificationResponse = await pipeline.ExecuteAsync(async canellationToken => await this.restClient.PostAsync<ClassificationResponseDto>(request, canellationToken));

            return classificationResponse;
        }
    }
}
