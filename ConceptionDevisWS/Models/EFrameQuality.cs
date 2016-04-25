using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EFrameQuality
    {
        Wood,
        PVC
    }
}