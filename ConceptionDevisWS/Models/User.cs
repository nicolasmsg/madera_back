using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    [Table("Utilisateurs")]
    public class User : IIdentifiable
    {
        private string _rightsStr;
        private ERights _rights;

        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Login { get; set; }
        [StringLength(20), DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Column("Droits")]
        [IgnoreDataMember, XmlIgnore, JsonIgnore]
        public string RightsStr
        {
            get { return _rightsStr; }
            set
            {
                _rightsStr = value;
                _rights = (ERights)Enum.Parse(typeof(ERights), _rightsStr);
            }
        }
        [NotMapped]
        public ERights Rights
        {
            get { return _rights; }
            set
            {
                _rights = value;
                _rightsStr = _rights.ToString();
            }
        } 
    }
}