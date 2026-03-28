// Application/Common/Interfaces/IApplicationDbContext.cs
using System.Data;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // ✅ Dapper travaille avec IDbConnection
        IDbConnection CreateConnection();
        Task InitializeAsync(); // ✅ ajouter cette méthode
    
    }
}