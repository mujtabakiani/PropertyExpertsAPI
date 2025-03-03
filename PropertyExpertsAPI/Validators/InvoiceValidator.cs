using FluentValidation;
using PropertyExperts.API.Models.Requests;

namespace PropertyExperts.API.Validators
{
    public class InvoiceValidator : AbstractValidator<InvoiceRequest>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.InvoiceId).GreaterThan(0).WithMessage("Invoice ID must be greater than zero.");
            
            RuleFor(x => x.InvoiceNumber).Matches(@"^S\d{5}$").WithMessage("Invoice number must start with 'S' followed by 5 digits.");
            
            RuleFor(x => x.InvoiceDate).NotEmpty().WithMessage("Invoice date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Invoice date cannot be in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-10)).WithMessage("Invoice date is too old. Must be within the last 10 years.");

            RuleFor(x => x.Comment).NotEmpty().WithMessage("Insurance comment is required.");

            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.File).Must(BeValidFile).WithMessage("Only PDF files up to 5MB are allowed.");
        }

        private bool BeValidFile(IFormFile file)
        {
            if (file == null) return false;
            string extension = Path.GetExtension(file.FileName).ToLower();
            return extension == ".pdf" && file.Length <= 5 * 1024 * 1024;
        }
    }
}
