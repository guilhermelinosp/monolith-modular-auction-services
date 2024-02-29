using System.Text.RegularExpressions;
using Auction.Authentication.Domain.Messages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Validators;

public partial class SignUpValidator : AbstractValidator<RegisterRequest>
{
	public SignUpValidator()
	{
		RuleFor(e => e.Email)
			.NotEmpty()
			.WithMessage(ValidatorMessage.EMAIL_NOT_INFORMED)
			.Custom((email, validator) =>
			{
				if (!RegexEmail().IsMatch(email))
					validator.AddFailure(new ValidationFailure(nameof(RegisterRequest.Email),
						ValidatorMessage.EMAIL_NOT_VALID));
			});

		RuleFor(e => e.Password)
			.NotEmpty()
			.WithMessage(ValidatorMessage.PASSWORD_NOT_INFORMED)
			.MinimumLength(8)
			.WithMessage(ValidatorMessage.PASSWORD_MINIMUM_LENGTH)
			.Custom((password, validator) =>
			{
				if (!RegexPassword().IsMatch(password))
					validator.AddFailure(new ValidationFailure(nameof(LoginRequest.Password),
						ValidatorMessage.PASSWORD_NOT_VALID));
			});
	}

	[GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$")]
	private static partial Regex RegexPassword();

	[GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
	private static partial Regex RegexEmail();
}