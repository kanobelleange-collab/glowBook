using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Interfaces;


namespace Application.Features.Employees.Queries.GetBySpecialite
{
     public class GetEmployeeBySpecialiteQueryHandler :
       
        IRequestHandler<GetEmployeeBySpecialiteQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;

        public GetEmployeeBySpecialiteQueryHandler(IEmployeeRepository repository)
        {
             _repository = repository; 
        } 

     public async Task<List<EmployeeDto>> Handle(GetEmployeeBySpecialiteQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetBySpecialiteAsync(request.Specialite);
            return list.ToList();
        }
    }
}