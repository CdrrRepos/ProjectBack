using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBack.Entities
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string expiration { get; set; }
    }
}
