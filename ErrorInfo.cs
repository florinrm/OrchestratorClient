using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrchestratorClient
{
    public class ErrorInfo
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "validationErrors")]
        public ValidationErrorInfo[] validationErrors { get; set; }
    }
}
