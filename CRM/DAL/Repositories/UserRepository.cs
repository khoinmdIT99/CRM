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
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        public string _tableName = "Users";
        public UserRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }

        
        public async void Delete(int id) {
            using (var conn = Connection)
            {
                var sql = "DELETE FROM Users WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, System.Data.DbType.Int32);
                await conn.QueryFirstOrDefaultAsync<User>(sql, parameters);
            }
        }

        public async Task<IEnumerable<User>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Users";
                return await conn.QueryAsync<User>(sql);
            }
        }

        public async Task<User> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Users WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<User>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        public async Task<User> Login(string username,string password)
        {
            using (var conn = Connection)
            {
                string sQuery = $"SELECT * FROM Users WHERE Username = @Username and Password = @Password";
                conn.Open();
                var result = await conn.QueryAsync<User>(sQuery, new { Username = username, Password = password });
                return result.FirstOrDefault();
            }
        }
        public async Task<User> Add(User entity)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Users(Username) "
                    + "VALUES(@Username)";

                var parameters = new DynamicParameters();
                parameters.Add("@entity", entity);

                 conn.Open();
                var result = await conn.QueryAsync<User>(sql, parameters);
                return result.FirstOrDefault();
            }
        }

        public async Task<User> Update(User entityToUpdate)
        {
            using (var conn = Connection)
            {
                var existingEntity =  GetByID(1);

                var sql = "UPDATE Users "
                    + "SET ";

                var parameters = new DynamicParameters();
                if (existingEntity != null)
                {
                    sql += "Username=@Username,";
                    parameters.Add("@Username", entityToUpdate, DbType.String);
                }

                sql = sql.TrimEnd(',');

                sql += " WHERE Id=@Id";
                parameters.Add("@Id", 1, DbType.Int32);

                conn.Open();
                var result = await conn.QueryAsync<User>(sql, parameters);
                return result.FirstOrDefault();
            }
        }
    }
}

