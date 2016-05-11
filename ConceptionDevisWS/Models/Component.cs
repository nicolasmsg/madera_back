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
        public string Reference
        {
            get { return "COMP-" + Id; }
        }
        [StringLength(50)]
        [Column("Nom")]
        public string Name { get; set; }
        [Column("Longueur")]
        public double Length { get; set; }
        //! Hidden from web service's response and not updatable either (use compoment to update its modules instead).
        [IgnoreDataMember, XmlIgnore,JsonIgnore]
        public List<Module> Modules { get; set; }

        [Column("Prix")]
        public double Price { get; set; }

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