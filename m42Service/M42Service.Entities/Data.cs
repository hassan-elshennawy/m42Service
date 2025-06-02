using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace m42Service.Entities
{
    public class Data
    {
        [JsonPropertyName("name")]
        public string Name {  get; set; }
        [JsonPropertyName("dob")]
        public DateOnly Dob { get; set; }
        [JsonPropertyName("mrn")]
        public string Mrn { get; set; }
    }
}
