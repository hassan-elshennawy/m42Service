using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace m42Service.Entities
{
    public class UpdateRequestDto
    {
        [JsonPropertyName("pdf")]
        public string Pdf {  get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }
        [JsonPropertyName("templateId")]
        public string TemplsteId { get; set; }
    }
}
