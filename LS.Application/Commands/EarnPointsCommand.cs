using MediatR;

namespace LS.Application.Commands
{
    public record EarnPointsCommand(int UserId, int Points) : IRequest<bool>;
}
