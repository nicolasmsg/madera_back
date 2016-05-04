using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(CulturalEnumStringConverter))]
    public enum EInsulatorKind
    {
        GlassWool,
        Styrofoam,
        RockWool
    }
}