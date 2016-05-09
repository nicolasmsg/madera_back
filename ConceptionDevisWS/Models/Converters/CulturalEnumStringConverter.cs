using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace ConceptionDevisWS.Models.Converters
{
    public class CulturalEnumStringConverter : JsonConverter
    {
        public static CultureInfo Culture { get; set; }

        public override bool CanConvert(Type objectType)
        {
             return objectType.IsAssignableFrom(typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object readObj = null;
            string[] names = Enum.GetNames(objectType);
            for (int i = 0; i < names.Length; i++)
            {
                string[] parts = reader.Value.ToString().Split(',');
                string enumValues = null;
                for(int j=0; j< parts.Length; j++)
                {
                    string val = parts[j].Trim();
                    string enumValidValue = Array.Find<string>(names, 
                        s => Translations.ResourceManager.GetString
                        ("Enums_" + objectType.Name + "_"+ s).Equals(val, StringComparison.InvariantCultureIgnoreCase)
                    );
                    if(enumValidValue != null)
                    {
                        if(j==0)
                        {
                            enumValues = enumValidValue;
                        } else
                        {
                            enumValues += " , " + enumValidValue;
                        }
                    }
                }
                if(enumValues != null)
                {
                    readObj = Enum.Parse(objectType, enumValues);
                }
            }

            return readObj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string[] parts = value.ToString().Split(',');
            string translatedValue = null;
            for(int i=0; i<parts.Length;i++)
            {
                string val = parts[i].Trim(' ');
                if (i == 0)
                {
                    translatedValue = Translations.ResourceManager.GetString("Enums_" + value.GetType().Name + "_" + val, Culture);
                } else
                {
                    translatedValue += " , " + Translations.ResourceManager.GetString("Enums_" + value.GetType().Name + "_" + val, Culture);
                } 
            }
            writer.WriteValue(translatedValue);
        }
    }
}