using _21T1080045.DomainModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers.SQLServer
{
    public class SimpleSupplierDAL : BaseDAL, ISimpleQueryDAL<Supplier>
    {
        public SimpleSupplierDAL(string connectionString) : base(connectionString)
        {
        }

        public List<Supplier> List()
        {
            List<Supplier> data = new List<Supplier>();
            using (var connection = OpenConnection())
            {
                var sql = @"Select * from Suppliers";
                data = connection.Query<Supplier>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }
    }
}
