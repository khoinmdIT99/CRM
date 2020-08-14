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
    public class MainIndustryRepository : IMainIndustryRepository
    {
        private readonly IConfiguration _config;
        public MainIndustryRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<MainIndustry>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM MainIndustries";
                var a =  await conn.QueryAsync<MainIndustry>(sql);
                return a;
            }
        }


        public async Task<MainIndustry> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM MainIndustries WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<MainIndustry>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        
    } 
}

