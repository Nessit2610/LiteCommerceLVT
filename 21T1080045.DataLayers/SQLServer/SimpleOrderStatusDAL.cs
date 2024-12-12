using _21T1080045.DomainModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers.SQLServer
{
    public class SimpleOrderStatusDAL : BaseDAL, ISimpleQueryDAL<OrderStatus>
    {
        public SimpleOrderStatusDAL(string connectionString) : base(connectionString)
        {
        }

        public List<OrderStatus> List()
        {
            List<OrderStatus> data = new List<OrderStatus>();
            using (var connection = OpenConnection())
            {
                var sql = @"Select * from OrderStatus";
                data = connection.Query<OrderStatus>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }
    }
}
