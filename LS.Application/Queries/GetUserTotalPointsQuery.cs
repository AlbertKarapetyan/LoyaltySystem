using MediatR;

namespace LS.Application.Queries
{
    public record GetUserTotalPointsQuery(int UserId) : IRequest<int?>;
}
