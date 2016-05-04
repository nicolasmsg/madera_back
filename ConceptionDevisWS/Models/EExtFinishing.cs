using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;
using System;

namespace ConceptionDevisWS.Models
{
    [Flags]
    [JsonConverter(typeof(CulturalEnumStringConverter))]
    public enum EExtFinishing
    {
        Wood=1,
        Roughcast=2,
        Paint=4
    }
}