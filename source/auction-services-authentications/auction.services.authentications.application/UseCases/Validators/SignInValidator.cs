using System.Text.RegularExpressions;
using auction.services.authentications.domain.DTOs.Requests;
using auction.services.authentications.domain.Messages;
using FluentValidation;
using FluentValidation.Results;

namespace auction.services.authentications.application.UseCases.Validators;

public partial class SignInValidator : AbstractValidator<LoginRequest>
{
	public SignInValidator()
	{
		RuleFor(e => e.Email)
			.NotEmpty()
			.WithMessage(ValidatorMessage.EMAIL_NOT_INFORMED)
			.Custom((email, validator) =>
			{
				if (!RegexEmail().IsMatch(email))
					validator.AddFailure(new ValidationFailure(nameof(LoginRequest.Email),
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