using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    public class AuthResponsDto
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public int Refresh_expires_in { get; set; }
        public string Token_type { get; set; }
        public int Not_before_policy { get; set; }
        public string Scope { get; set; }
    }
}
