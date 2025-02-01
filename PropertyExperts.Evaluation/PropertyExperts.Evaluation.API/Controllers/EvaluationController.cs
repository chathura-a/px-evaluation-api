using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PropertyExperts.Evaluation.API.Models.DTOs;
using PropertyExperts.Evaluation.API.Services;

namespace PropertyExperts.Evaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly ILogger<EvaluationController> logger;
        private readonly IEvaluationService evaluationService;

        public EvaluationController(IEvaluationService evaluationService, ILogger<EvaluationController> logger)
        {
            this.evaluationService = evaluationService;
            this.logger = logger;
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluateInvoice([FromForm] InvoiceEvaluateRequestDto invoiceEvaluateRequestDto)
        {
            this.logger.LogInformation("Start processing request : {details}", JsonConvert.SerializeObject(invoiceEvaluateRequestDto));

            var document = invoiceEvaluateRequestDto.File;
            // Validate document
            if (document == null || document.Length == 0)
                throw new BadHttpRequestException("Invoice document is required.");

            if (document.ContentType != "application/pdf")
                throw new BadHttpRequestException("Only PDF files are allowed.");
           
            var result = await this.evaluationService.EvaluateInvoice(invoiceEvaluateRequestDto);
            return Ok(result);
        }
    }
}
