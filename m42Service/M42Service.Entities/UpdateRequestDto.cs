using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    internal class UpdateRequestDto
    {
        public string pdf {  get; set; }
        public Data data;
        public string templsteId { get; set; }
    }
}
