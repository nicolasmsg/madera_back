using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    public class Projet : IIdentifiable
    {
        private string _etatStr;
        private EEtatProjet _etat;

        public int Id { get; set; }
        [StringLength(50)]
        public string Reference { get; set; }
        [StringLength(30)]
        public string Nom { get; set; }
        public Client Client { get; set; }

        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        [Column("Etat")]
        public string EtatStr
        {
            get { return _etatStr; }
            set
            {
                _etatStr = value;
                _etat = (EEtatProjet)Enum.Parse(typeof(EEtatProjet), _etatStr);
            }
        }
        [NotMapped]
        public EEtatProjet Etat
        {
            get { return _etat; }
            set
            {
                _etat = value;
                _etatStr = _etat.ToString();
            }
        }
    }
}