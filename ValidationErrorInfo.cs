using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrchestratorClient
{
    public class ValidationErrorInfo
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "members")]
        public string[] Members { get; set; }
    }
}
