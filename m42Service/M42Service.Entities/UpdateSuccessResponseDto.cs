using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    public class UpdateSuccessResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("modifiedPdf")]
        public string ModifiedPdf { get; set; }
    }
}
