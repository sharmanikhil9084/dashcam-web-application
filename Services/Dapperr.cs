using Dapper;
using System.Data.Common;
using System.Data;
using TrackanDrive.Web.Interfaces;
using Npgsql;

namespace TrackanDrive.Web.Services
{
    public class Dapperr:IDapper
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";

        public Dapperr(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {

        }

      
        public async Task<T> Get<T>(string sp, DynamicParameters parms)
        {
            using IDbConnection db = new NpgsqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: CommandType.Text, commandTimeout: 180).FirstOrDefault();
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms)
        {
            using IDbConnection db = new NpgsqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: CommandType.Text, commandTimeout:180).ToList();
        }
    }
}
