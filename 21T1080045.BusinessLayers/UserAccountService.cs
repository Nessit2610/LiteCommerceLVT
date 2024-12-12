using _21T1080045.DataLayers;
using _21T1080045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;

        static UserAccountService()
        {
            string connectionString = Configuration.ConnectionString;
            employeeAccountDB = new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);  
        }
        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccountDB.Authorize(username, password);
            }
            return employeeAccountDB.Authorize(username, password);
        }
        public static bool ChangePassword(string oldPassword, int id , string newPassword)
        {
            return employeeAccountDB.ChangePassword(oldPassword, id, newPassword);
        }
        public enum UserTypes
        {
            Employee,
            Customer
        }
    }
}
