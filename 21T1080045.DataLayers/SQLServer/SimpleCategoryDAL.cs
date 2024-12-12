using _21T1080045.DomainModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers.SQLServer
{
    public class SimpleCategoryDAL : BaseDAL, ISimpleQueryDAL<Category>

    {
        public SimpleCategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public List<Category> List()
        {
            List<Category> data = new List<Category>();
            using (var connection = OpenConnection())
            {
                var sql = @"Select * from Categories";
                data = connection.Query<Category>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }
    }
}
