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
    public class PositionRepository : IPositionRepository
    {
        private readonly IConfiguration _config;
    
        public PositionRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<Position>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Positions";
                var a =  await conn.QueryAsync<Position>(sql);
                return a;
            }
        }


        public async Task<Position> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Positions WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Position>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        
    } 
}

