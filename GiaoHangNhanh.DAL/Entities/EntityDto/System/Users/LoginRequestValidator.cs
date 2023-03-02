using FluentValidation;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(2).WithMessage("Password is at least 6 characters");
        }
    }
}