using AutoMapper;
using LS.Application.Commands;
using LS.Application.DTOs;
using LS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LS.Application.Handlers
{
    internal class CreateUserCommandHandler: BaseCommandHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IUserService userService, 
            ILogger<CreateUserCommandHandler> logger, 
            IMapper mapper)
            : base(logger)
        {
            _userService = userService;
            _mapper = mapper;
        }

        protected override async Task<UserDto> ExecuteAsync(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(request.Name, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
