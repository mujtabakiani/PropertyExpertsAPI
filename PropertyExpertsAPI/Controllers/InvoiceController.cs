using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using PropertyExperts.API.Models.Requests;
using System.Net;
using PropertyExperts.API.Services.BusinessLogic;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IValidator<InvoiceRequest> _validator;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger,IValidator<InvoiceRequest> validator,IInvoiceService invoiceService)
    {
        _logger = logger;
        _validator = validator;
        _invoiceService = invoiceService;
    }

    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(PropertyExperts.API.Models.Responses.EvaluationSummary), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> EvaluateInvoice([FromForm] InvoiceRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new
            {
                field = e.PropertyName,
                message = e.ErrorMessage,
                attemptedValue = e.AttemptedValue
            }).ToList();

            _logger.LogWarning($"Validation failed for InvoiceRequest: {errors}");

            return BadRequest(new { success = false, errors });
        }

        try
        {
            var result = await _invoiceService.EvaluateInvoiceAsync(request);
            _logger.LogInformation($"Invoice evaluated successfully: {result}");
            return Ok(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while evaluating invoice");
            return StatusCode(500, new { success = false, message = "Internal Server Error" });
        }
    }
}
