using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(CulturalEnumStringConverter))]
    public enum EFillingKind
    {
        WoodenWool,
        NaturalWool,
        Hemp
    }
}