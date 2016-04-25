using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;

namespace ConceptionDevisWS.Models
{
    [Table("Modele")]
    public class Model : IIdentifiable
    {
        public int Id { get; set; }

        public string Reference
        {
            get { return "MODE-" + Id; }
        }

        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        [Required]
        public Range Range { get; set; }

        [NotMapped]
        [JsonProperty("Range")]
        public string RangeStr
        {
            get { return Range.Name; }
        }

        [Column("Nom")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("Remplissage")]
        public EFillingKind Filling { get; set; }

        [Column("FinitionsExterieures")]
        public EExtFinishing ExtFinishing { get; set; }

        [Column("FinitionsInterieures")]
        public EIntFinishing IntFinishing { get; set; }

        [Column("QualiteHuisserie")]
        public EFrameQuality FrameQuality { get; set; }

        [Column("CheminImage")]
        public string ImagePath { get; set; }

        [Column("PourcentagePrixBase")]
        public double BasePricePercentage { get; set; }

        public void UpdateNonComposedPropertiesFrom(Model newModel)
        {
            Name = newModel.Name;
            Filling = newModel.Filling;
            IntFinishing = newModel.IntFinishing;
            ExtFinishing = newModel.ExtFinishing;
            FrameQuality = newModel.FrameQuality;
            ImagePath = newModel.ImagePath;
            BasePricePercentage = newModel.BasePricePercentage;
        }
    }
}