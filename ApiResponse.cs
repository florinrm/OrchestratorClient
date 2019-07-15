﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrchestratorClient
{
    public class ApiResponse<T>
    {
        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }

        [JsonProperty(PropertyName = "targetUrl")]
        public string TargetUrl { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorInfo Error { get; set; }

        [JsonProperty(PropertyName = "unAuthorizedRequest")]
        public bool UnAuthorizedRequest { get; set; }

        [JsonProperty(PropertyName = "__abp")]
        public bool Abp { get; }
    }
}
