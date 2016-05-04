using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;

namespace ConceptionDevisWS.Models
{
    /// <summary>
    /// Represents the application the user is requesting access to
    /// </summary>
    [JsonConverter(typeof(CulturalEnumStringConverter))]
    public enum ERights
    {
        None,
        ConceptionDevis
    }
}