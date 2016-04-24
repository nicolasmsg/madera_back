using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConceptionDevisWS.Models
{
    /// <summary>
    /// Represents the application the user is requesting access to
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ERights
    {
        None,
        ConceptionDevis
    }
}