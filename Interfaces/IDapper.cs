using Dapper;
using System.Data.Common;
using System.Data;

namespace TrackanDrive.Web.Interfaces
{
    public interface IDapper : IDisposable
    {
        Task<T> Get<T>(string sp, DynamicParameters parms);
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms);
        
    }
}
