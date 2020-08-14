using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace CRM
{
    public class RepositoryFactory<T> : IRepositoryFactory<T> 
        where T : class
    {
        //private string _connectionString;
        private readonly string _tableName;
        private readonly IConfiguration _config;

        public RepositoryFactory(IConfiguration config = null, string tableName = null)
        {
            //_connectionString = connectionString;
            _config = config;
            _tableName = tableName;
        }

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_config.GetConnectionString("Default"));
        }
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        public async Task<IEnumerable<T>> Get()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {_tableName}");
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
            }
        }

        public async Task<T> GetByID(int id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id });
                if (result == null)
                    throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

                return result;
            }
        }

        //public async Task<int> Save(IEnumerable<T> list)
        //{
        //    var inserted = 0;
        //    var query = GenerateInsertQuery();
        //    using (var connection = CreateConnection())
        //    {
        //        inserted += await connection.ExecuteAsync(query, list);
        //    }

        //    return inserted;
        //}


        public async Task Add(T t)
        {
            var insertQuery = GenerateInsertQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, t);
            }
        }

        public async Task Update(T t)
        {
            var updateQuery = GenerateUpdateQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }


        /// <summary>
        /// ////////////////////////
        /// </summary>
        /// <returns></returns>
        //public IDbConnection GetConnection()
        //{
        //    return DbConnectionFactory.GetDbConnection(_connectionString);
        //}

        //public async void Delete(int id)
        //{
        //    using (var conn = GetConnection())
        //    {
        //        var sql = "DELETE FROM Users WHERE Id = @Id";
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@Id", id, System.Data.DbType.Int32);
        //        await conn.QueryFirstOrDefaultAsync<User>(sql, parameters);
        //    }
        //}
        //public async Task<IEnumerable<T>> Get()
        //{
        //    using (var conn = GetConnection())
        //    {
        //        var sql = "SELECT * FROM Users";
        //        return await conn.QueryAsync<T>(sql);
        //    }
        //}
        //public async Task<T> GetByID(int id)
        //{
        //    using (var conn = GetConnection())
        //    {
        //        var sql = "SELECT * FROM User WHERE Id = @Id";
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@Id", id, System.Data.DbType.Int32);
        //        return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
        //    }
        //}
        //public async void Add(T entity)
        //{
        //    using (var conn = GetConnection())
        //    {
        //        var sql = "INSERT INTO Users(Username) "
        //            + "VALUES(@Username)";

        //        var parameters = new DynamicParameters();
        //        parameters.Add("@entity", entity);

        //        await conn.QueryAsync(sql, parameters);
        //    }
        //}
        //public async void Update(T entityToUpdate)
        //{

        //    using (var conn = GetConnection())
        //    {
        //        var existingEntity = await GetByID(1);

        //        var sql = "UPDATE Users "
        //            + "SET ";

        //        var parameters = new DynamicParameters();
        //        if (existingEntity != entityToUpdate)
        //        {
        //            sql += "Username=@Username,";
        //            parameters.Add("@Username", entityToUpdate, DbType.String);
        //        }

        //        sql = sql.TrimEnd(',');

        //        sql += " WHERE Id=@Id";
        //        parameters.Add("@Id", 1, DbType.Int32);

        //        await conn.QueryAsync(sql, parameters);
        //    }
        //}
    }
}

