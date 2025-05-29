using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.Entities
{
    internal class AuthDto
    {
        public string grant_type { get; set; }
        public string client_authentication_method { get; set; }
        public string client_secret { get; set; }
        public int client_id {  get; set; }
    }
}
