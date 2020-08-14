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
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _config;
        //public string _tableName = "Cities";
        public CityRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<City>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Cities";
                var a =  await conn.QueryAsync<City>(sql);
                return a;
            }
        }


        public async Task<City> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Cities WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<City>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        
    } 
}

