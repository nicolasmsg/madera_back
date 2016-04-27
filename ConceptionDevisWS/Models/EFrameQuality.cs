using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum EFrameQuality
    {
        Wood=1,
        PVC=2
    }
}