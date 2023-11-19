using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Authentication.Login
{
    public class LoginResponse
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public string Role { get;  set; }
        public string Jobs { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
