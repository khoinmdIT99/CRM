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
    public class EventRepository : IEventRepository
    {
        private readonly IConfiguration _config;
        public string _tableName = "Events";
        public EventRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<Event>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Events";
                return await conn.QueryAsync<Event>(sql);
            }
        }

        public async Task<Event> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Events WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Event>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<Event> Add(Event entity)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Events(UserID, CustID, ContactID, StartDate, EndDate, TypeID, ForUserID, ForCareDate, FileName, StatusID, Remarks) "
                    + "VALUES(@UserID, @CustID, @ContactID, @StartDate, @EndDate, @TypeID, @ForUserID, @ForCareDate, @FileName, @StatusID, @Remarks);" 
                    + " SELECT CAST(SCOPE_IDENTITY() as int) ";

                conn.Open();
                var inseredID = await conn.ExecuteScalarAsync<int>(sql, entity);
                var result = GetByID(inseredID);
                return result.Result;
            }
        }

        public async Task<Event> Update(Event entityToUpdate)
        {
            using (var conn = Connection)
            {
                var sql = "UPDATE Events "
                        + "SET UserID = @UserID," +
                        " CustID = @CustID," +
                        " ContactID = @ContactID," +
                        "StartDate = @StartDate," +
                        " EndDate = @EndDate," +
                        "TypeID = @TypeID," +
                        " ForUserID = @ForUserID, " +
                        "ForCareDate = @ForCareDate," +
                        "FileName = @FileName," +
                        "StatusID = @StatusID," +
                        $" Remarks = @Remarks Where id = {entityToUpdate.ID}";

                conn.Open();
                var updeted = await conn.QueryAsync<Event>(sql, entityToUpdate);
                var result = GetByID(entityToUpdate.ID);
                return result.Result;

            }

        }
        public async Task<Event> Save(Event entity)
        {
            Event ret;
            if (entity.ID > 0)
            {
                ret = Update(entity).Result;
            }
            else
            {
                ret = Add(entity).Result;

            }
            return ret;
        }

        public async void Delete(int id)
        {
            using (var conn = Connection)
            {
                var sql = "DELETE FROM Events WHERE Id = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
    } 
}

