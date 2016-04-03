using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    public class Composant : IIdentifiable
    {
        private string _angleStr;
        private EAngle _angle;

        public int Id { get; set; }
        [StringLength(50)]
        public string Referrence { get; set; }
        [StringLength(50)]
        public string Nom { get; set; }
        public double Longueur { get; set; }
        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        public List<Module> Modules { get; set; }

        [Column("Angle")]
        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        public string AngleStr
        {
            get { return _angleStr; }
            set
            {
                _angleStr = value;
                _angle = (EAngle)Enum.Parse(typeof(EAngle), _angleStr);
            }
        }
        [NotMapped]
        public EAngle Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                _angleStr = _angle.ToString();
            }
        }
    }
}