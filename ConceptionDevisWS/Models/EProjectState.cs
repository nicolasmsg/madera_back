using ConceptionDevisWS.Models.Converters;
using Newtonsoft.Json;

namespace ConceptionDevisWS.Models
{
    [JsonConverter(typeof(CulturalEnumStringConverter))]
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