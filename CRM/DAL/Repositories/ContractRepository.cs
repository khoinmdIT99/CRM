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
    public class ContactRepository : IContactRepository
    {
        private readonly IConfiguration _config;
        public string _tableName = "Contacts";
        public ContactRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection  { get { return new SqlConnection(_config.GetConnectionString("Default"));  } }
        
        public async Task<IEnumerable<Contact>> Get()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Contacts";
                return await conn.QueryAsync<Contact>(sql);
            }
        }

        public async Task<Contact> GetByID(int id)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Contacts WHERE ID = @ID";
                conn.Open();
                var result = await conn.QueryAsync<Contact>(sQuery, new { ID = id });
                return result.FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Contact>> GetByCustID(int custID)
        {
            using (var conn = Connection)
            {
                string sQuery = "SELECT * FROM Contacts WHERE CustID = @custID";
                conn.Open();
                var result = await conn.QueryAsync<Contact>(sQuery, new { custID });
                return result;
            }
        }

        public async Task<Contact> Add(Contact entity)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Contacts(CustID, Name, PositionID, Email, Phone1, Phone2, Mobile, Fax, StatusID, Remarks) "
                    + "VALUES(@CustID, @Name, @PositionID, @Email, @Phone1, @Phone2,  @Mobile, @Fax, @StatusID, @Remarks);"
                    + "SELECT CAST(SCOPE_IDENTITY() as int) ";

                conn.Open();
                var inseredID = await conn.ExecuteScalarAsync<int>(sql, entity);
                var result = GetByID(inseredID);
                return result.Result;
            }
        }

        public async Task<Contact> Update(Contact entityToUpdate)
        {
            using (var conn = Connection)
            {
                var sql = "UPDATE Contacts "
                        + "SET CustID = @CustID," +
                        " Name = @Name," +
                        " PositionID = @PositionID," +
                        " Email = @Email," +
                        " Phone1 = @Phone1," +
                        " Phone2 = @Phone2," +
                        " Mobile = @Mobile, " +
                        " Fax = @Fax," +
                        " StatusID = @StatusID," +
                        $" Remarks = @Remarks  Where id = {entityToUpdate.ID}";

                conn.Open();
                var updeted = await conn.QueryAsync<Contact>(sql, entityToUpdate);
                var result = GetByID(entityToUpdate.ID);
                return result.Result;

            }
        }

        public async Task<Contact> Save(Contact entity)
        {
            Contact ret;
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
                var sql = "DELETE FROM Contacts WHERE Id = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
    } 
}

