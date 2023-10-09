using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.DTOs.Response
{
    public class ODataResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string Metadata { get; set; }
        public List<T> Value { get; set; }
    }
}
