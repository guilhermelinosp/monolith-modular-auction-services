using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.Messages;
using FluentValidation;

namespace Auction.Authentication.Application.UseCases.Validators;

public class VerifyEmailValidator : AbstractValidator<VerifyEmailRequest>
{
	public VerifyEmailValidator() =>
		RuleFor(e => e.Email)
			.NotEmpty()
			.WithMessage(ValidatorMessage.EMAIL_NOT_INFORMED)
			.EmailAddress()
			.WithMessage(ValidatorMessage.EMAIL_NOT_VALID);
}