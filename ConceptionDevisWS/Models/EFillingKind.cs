using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EFillingKind
    {
        WoodenWool,
        NaturalWool,
        Hemp
    }
}