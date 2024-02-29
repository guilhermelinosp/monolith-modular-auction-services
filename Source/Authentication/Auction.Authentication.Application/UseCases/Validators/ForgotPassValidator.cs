using System.Text.RegularExpressions;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.Messages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Validators;

public partial class ForgotPassValidator : AbstractValidator<ForgotPassRequest>
{
	public ForgotPassValidator()
	{
		RuleFor(e => e.Email)
			.NotEmpty()
			.WithMessage(ValidatorMessage.EMAIL_NOT_INFORMED)
			.Custom((email, validator) =>
			{
				if (!RegexEmail().IsMatch(email))
					validator.AddFailure(new ValidationFailure(nameof(ForgotPasswordRequest.Email),
						ValidatorMessage.EMAIL_NOT_VALID));
			});
	}


	[GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
	private static partial Regex RegexEmail();
}