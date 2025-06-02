using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    public class AuthResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public AuthResponsDto Success { get; set; }
        public AuthErrorDto Error { get; set; }
    }
}
