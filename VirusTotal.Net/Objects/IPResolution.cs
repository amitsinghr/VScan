﻿using System;
using Newtonsoft.Json;

namespace VirusTotalNET.Objects
{
    public class IPResolution
    {
        [JsonProperty("last_resolved", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastResolved { get; set; }

        public string Hostname { get; set; }
    }
}