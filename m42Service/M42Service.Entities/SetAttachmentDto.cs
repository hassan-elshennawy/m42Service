using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    public class SetAttachmentDto
    {
        public int SeqNumber { get; set; }
        public int Status {  get; set; }
        public string ModifiedPdf {  get; set; }
    }
}
