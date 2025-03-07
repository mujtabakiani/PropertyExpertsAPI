#PropertyExperts Invoice Evaluation API

#Overview
The PropertyExperts Invoice Evaluation API is a RESTful service that evaluates invoices by classifying them and applying business rules based on their content. The API processes invoices (uploaded as PDF files), classifies them, and determines actions (e.g., "Approve", "Flag for Review") based on predefined decision rules.

#Key Features
#Invoice Evaluation: The API accepts invoice requests containing metadata and a PDF file. It evaluates the invoice based on its classification and amount.
#Dynamic Rule Application: Decision rules are applied to the invoice based on its classification (e.g., "FireDamagedWallRepair") and the amount. Actions like "Approve" or "Flag for Review" are determined.
#File Handling: The system supports PDF files, which are processed for classification.
#Validation: Incoming requests are validated to ensure all required fields are present and correct.
#Logging: Extensive logging using Serilog ensures that all actions and errors are captured for troubleshooting.

#How It Works
Invoice Request: Clients send a POST request to /api/invoice/evaluate with invoice details and a PDF file.
Validation: The invoice details are validated (e.g., invoice number, date, amount, file type). If validation fails, an error is returned.
Invoice Classification: The invoice file is sent to an external service for classification (e.g., determining if it's a fire-damaged wall repair).
Rule Evaluation: Based on the classification and the invoice amount, decision rules are applied to determine actions.
Evaluation Summary: A summary of the evaluation, including the classification, risk level, and applied rules, is returned to the client.

#Technologies Used
ASP.NET Core: Framework for building the API.
FluentValidation: For request validation.
Serilog: For structured logging.
RestSharp: For making HTTP requests to external services.
Polly: For retry logic in handling transient errors.
Configuration
Decision Rules: Rules are loaded from a JSON file (decision-rules.json), and they govern the classification actions and thresholds.
External Service: The classification service URL is configurable via appsettings.json, allowing you to switch services easily.

#Setup & Running
Clone this repository.
Configure the MockApi:Url in the appsettings.json to point to the external classification service.
Run the project using the following command:
bash
Copy
Edit
dotnet run
The API will be available at http://localhost:7022. You can send a POST request to /api/invoice/evaluate with the required invoice data.
