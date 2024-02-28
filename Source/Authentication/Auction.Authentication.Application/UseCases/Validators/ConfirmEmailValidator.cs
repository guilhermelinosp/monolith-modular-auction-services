using System.Text.RegularExpressions;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace Auction.Authentication.Application.UseCases.Validators;

public partial class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
	public ConfirmEmailValidator()
	{
		RuleFor(e => e.Email)
			.NotEmpty()
			.WithMessage(ValidatorMessage.EMAIL_NOT_INFORMED)
			.EmailAddress()
			.WithMessage(ValidatorMessage.EMAIL_NOT_VALID);

		RuleFor(e => e.Otp)
			.NotEmpty()
			.WithMessage(ValidatorMessage.OTP_NOT_INFORMED)
			.Length(6)
			.WithMessage(ValidatorMessage.OTP_LENGTH)
			.Custom((otp, validator) =>
			{
				if (!RegexOtp().IsMatch(otp))
					validator.AddFailure(new ValidationFailure(nameof(ConfirmEmailRequest.Otp),
						ValidatorMessage.OTP_NOT_VALID));
			});
	}
	
	[GeneratedRegex(@"^(?=.*[0-9])")]
	private static partial Regex RegexOtp();
}