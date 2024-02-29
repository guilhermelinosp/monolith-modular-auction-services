using System.Text.RegularExpressions;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.Messages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Validators;

public partial class ResetPassValidator : AbstractValidator<ResetPassRequest>
{
	public ResetPassValidator()
	{
		RuleFor(e => e.Password)
			.NotEmpty()
			.WithMessage(ValidatorMessage.PASSWORD_NOT_INFORMED)
			.MinimumLength(8)
			.WithMessage(ValidatorMessage.PASSWORD_MINIMUM_LENGTH)
			.Custom((password, validator) =>
			{
				if (!RegexPassword().IsMatch(password))
					validator.AddFailure(new ValidationFailure(nameof(ResetPasswordRequest.NewPassword),
						ValidatorMessage.PASSWORD_NOT_VALID));
			});
	}

	[GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$")]
	private static partial Regex RegexPassword();
}