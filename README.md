# px-evaluation-api
Property Expert's Evaluation API  endpoints 

## Overview
This project is a .NET 8 Web API designed to evaluate invoices. It includes functionalities for receiving invoice data and documents, sending the documents and recieving classification from external service (mock), generating evaluation summaries, and includes unit and integration tests.

## Features
- **Project Setup:** Basic dependencies and initial structure.
- **API Endpoint:** POST `{baseUrl}/api/evaluation/evaluate` to receive and validate invoice details and documents.
- **Mock 3rd-Party API Integration:** Service to interact with a mock API for invoice classification.
- **Evaluation Summary:** Generates a JSON response and an evaluation summary file.
- **Testing:** Includes unit tests and an integration test.

## Technologies Used
- .NET 8
- Swashbuckle for Swagger
- Newtonsoft.Json
- Polly With Microsoft Resiliency
- RestSharp
- NUnit + Moq

### API Endpoints

#### POST /api/evaluation/evaluate
- **Description:** Accepts a PDF document and JSON payload with invoice details.
- **Payload:**
    ```json
    {
      "invoiceId": "12345",
      "invoiceNumber": "S12345",
      "invoiceDate": "2023-04-01",
      "comment": "Invoice comment",
      "amount": 1000
    }
    ```
- **Response:**
    ```json
    {
      "evaluationId": "EVAL001",
      "invoiceId": "12345",
      "rulesApplied": ["Approve"],
      "classification": "WaterLeakDetection",
      "evaluationFile": "Base64String"
    }
    ```

### Accessing classification API
Dummy classifiction API has been deployed [here][def]

[def]: https://px-classigication-api-d3b6h6gtavbrgmgz.eastasia-01.azurewebsites.net/upload
