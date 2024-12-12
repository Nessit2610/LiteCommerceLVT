using _21T1080045.DomainModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? userAccount = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT EmployeeID as UserID,
                                    FullName as DisplayName,
                                    Email as UserName,
                                    Photo,
                                    RoleNames
                           FROM Employees where Email = @Email AND Password = @Password";
                var parameters = new
                {
                    Email = username,
                    Password = password
                };
                userAccount = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return userAccount;   
        }

        public bool ChangePassword(string oldPassword, int id, string newPassword)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists (select * from Employees where EmployeeID = @EmployeeID and Password = @OldPassword)
                            begin
                                Update Employees
                                set Password = @NewPassword
                                where EmployeeID = @EmployeeID
                            end";
                var parameters = new
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    EmployeeID = id,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
       
    }
}
