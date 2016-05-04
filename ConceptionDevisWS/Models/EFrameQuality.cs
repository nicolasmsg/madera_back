using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;
using System;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(CulturalEnumStringConverter))]
    [Flags]
    public enum EFrameQuality
    {
        Wood=1,
        PVC=2
    }
}