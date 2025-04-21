using FluentValidation;
using LS.API.Models;

namespace LS.API.FluentValidations
{
    public class EarnPointsRequestValidator : AbstractValidator<EarnPointsRequest>
    {
        public EarnPointsRequestValidator()
        {
            RuleFor(x => x.Points).GreaterThan(0);
        }
    }
}
