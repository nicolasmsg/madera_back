using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EProjectState
    {
        Signed,
        BuildingLicense,
        WorkStarted,
        BaseDone,
        WallsDone,
        OutOfWater,
        EquipmentSetup,
        KeysGiven
    }
}