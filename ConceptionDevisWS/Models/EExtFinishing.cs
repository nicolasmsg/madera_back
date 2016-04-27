using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ConceptionDevisWS.Models
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EExtFinishing
    {
        Wood=1,
        Roughcast=2,
        Paint=4
    }
}