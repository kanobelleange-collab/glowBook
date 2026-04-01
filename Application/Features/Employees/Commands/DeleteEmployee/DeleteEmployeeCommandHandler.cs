using MediatR;
using Application.Features.Employees.Commands;
using Application.Features.Employees.Interfaces;

namespace Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
            => _repository = repository;

        public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
              await _repository.DeleteAsync(request.Id);
            
        }
    }
}