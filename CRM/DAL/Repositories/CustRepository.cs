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
    public class CustRepository : ICustRepository
    {
        private readonly IConfiguration _config;
        //public string _tableName = "Custs";
        public CustRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection { get { return new SqlConnection(_config.GetConnectionString("Default")); } }

        public async Task<IEnumerable<Cust>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Custs";
                return await conn.QueryAsync<Cust>(sql);
            }
        }


        public async Task<Cust> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Custs WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Cust>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<Cust> GetFull(int id)
        {
            Cust item;
            string sQuery = "SELECT * FROM Custs WHERE ID = @ID;" +
                                "Select * from Contacts Where CustID = @ID;"
                                +"Select * from Events Where CustID = @ID";
            using (var conn = Connection)
            {
                conn.Open();

                using (var multi = conn.QueryMultiple(sQuery, new { ID = id }))
                {
                    item = multi.Read<Cust>().First();
                    item.Contacts = multi.Read<Contact>().ToList();
                    item.Events = multi.Read<Event>().ToList();
                }
            }
            return item;
        }



        public async Task<Cust> Add(Cust entity)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Custs(Name, Address, CityID,Website,Email,Phone,Mobile,Fax,MainIndustryID,Industry,StatusID,Description) "
                    + "VALUES(@Name, @Address, @CityID, @Website, @Email, @Phone, @Mobile, @Fax, @MainIndustryID, @Industry, @StatusID, @Description);"
                    + "SELECT CAST(SCOPE_IDENTITY() as int) ";

                conn.Open();
                var inseredID = await conn.ExecuteScalarAsync<int>(sql, entity);
                var result = GetByID(inseredID);
                return result.Result;
            }
        }

        public async Task<Cust> Update(Cust entityToUpdate)
        {
            using (var conn = Connection)
            {
                var sql = "UPDATE Custs "
                    + "SET Name = @Name," +
                    " Address = @Address," +
                    " CityID = @CityID," +
                    "Website = @Website," +
                    " Email = @Email," +
                    "Phone = @Phone," +
                    " Mobile = @Mobile, " +
                    "Fax = @Fax," +
                    "MainIndustryID = @MainIndustryID," +
                    " Industry = @Industry," +
                    "StatusID = @StatusID," +
                    $"Description = @Description Where id = {entityToUpdate.ID}";

                conn.Open();
                var updeted = await conn.QueryAsync<Cust>(sql, entityToUpdate);
                var result = GetByID(entityToUpdate.ID);
                return result.Result;

            }
        }

        public async Task<Cust> Save(Cust entity)
        {
            Cust ret;
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
                var sql =
                    "Delete From Events Where CustID = @Id;" +
                    "Delete From Contacts Where CustID = @Id;"+
                    "DELETE FROM Custs WHERE Id = @Id;";
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}

