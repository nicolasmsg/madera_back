using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    public class Utilisateur : IIdentifiable
    {
        private string _droitsStr;
        private EDroits _droits;

        public int Id { get; set; }
        public string Login { get; set; }
        public string MotDePasse { get; set; }

        [Column("Droits")]
        [IgnoreDataMember, XmlIgnore, JsonIgnore]
        public string DroitsStr
        {
            get { return _droitsStr; }
            set
            {
                _droitsStr = value;
                _droits = (EDroits)Enum.Parse(typeof(EDroits), _droitsStr);
            }
        }
        [NotMapped]
        public EDroits Droits
        {
            get { return _droits; }
            set
            {
                _droits = value;
                _droitsStr = _droits.ToString();
            }
        } 
    }
}