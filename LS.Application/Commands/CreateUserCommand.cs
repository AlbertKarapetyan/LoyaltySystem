using LS.Application.DTOs;
using MediatR;

namespace LS.Application.Commands
{
    public record CreateUserCommand(string Name) : IRequest<UserDto>;

}
