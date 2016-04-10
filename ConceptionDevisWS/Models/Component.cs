using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    [Table("Composants")]
    public class Component : IIdentifiable
    {
        private string _angleStr;
        private EAngle _angle;

        public int Id { get; set; }
        [StringLength(50), Index(IsUnique = true)]
        public string Referrence
        {
            get { return "COMP-" + Id; }
        }
        [StringLength(50)]
        [Column("Nom")]
        public string Name { get; set; }
        [Column("Longueur")]
        public double Length { get; set; }
        // prevent self referencing loop within response serialization as (module being a recursive structure with  Module.Components[i].Modules
        //  cause troubles to serialization process)
        [IgnoreDataMember, XmlIgnore,JsonIgnore]
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