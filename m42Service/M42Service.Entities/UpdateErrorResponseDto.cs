using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace m42Service.M42Service.Entities
{
    public class UpdateErrorResponseDto
    {
        [JsonPropertyName("errorStatusCode")]
        public int ErrorStatusCode {  get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage {  get; set; }
    }
}
