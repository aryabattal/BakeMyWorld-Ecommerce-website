using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeMyWorld.ConsoleManager
{
    class Credentials
    {
        public Credentials()
        {

        }
        
        public Credentials(string nickname, string password)
        {
            Nickname = nickname;
            Password = password;
        }

        public string Nickname { get; set; }
        public string Password { get; set; }
    }
}
