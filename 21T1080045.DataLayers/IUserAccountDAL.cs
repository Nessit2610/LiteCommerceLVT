using _21T1080045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(string username, string password);
        bool ChangePassword(string oldPassworrd, int id, string newPassword);
    }
}
