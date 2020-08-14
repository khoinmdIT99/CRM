using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CRM
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly IConfiguration _config;
        public IndustryRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<Industry>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Industries";
                var a =  await conn.QueryAsync<Industry>(sql);
                return a;
            }
        }


        public async Task<Industry> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Industries WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Industry>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        
    } 
}

