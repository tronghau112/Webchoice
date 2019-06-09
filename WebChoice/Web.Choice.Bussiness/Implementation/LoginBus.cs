using System;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.Service.Implementation;
using Web.Choice.Service.Interfaces;

namespace Web.Choice.Bussiness.Implementation
{
    public class LoginBus : ILoginBus
    {
        private ILoginSer _loginSer = null;
        private ILoginSer Login => _loginSer ?? (_loginSer = new LoginSer());
        public bool IsValid(string username, string password)
        {
            try
            {
                if (username.IsEmpty() || password.IsEmpty())
                {
                    return false;
                }
                return Login.IsValid(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
