using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    [Table("Gamme")]
    public class Range : IIdentifiable
    {
        private List<Model> _models = new List<Model>();
        public int Id { get; set; }

        [Column("Nom")]
        [StringLength(50)]
        public string Name { get; set; }

        public string Reference
        {
            get { return "RAN-" + Id; }
        }

        [Column("FinitionsExterieures")]
        public EExtFinishing ExtFinishings { get; set; }
        [Column("TypeIsolant")]
        public EInsulatorKind Insulators { get; set; }
        [Column("QualiteHuisserie")]
        public EFrameQuality FrameQualities { get; set; }
        [Column("ConceptionOssature")]
        public EFrameStructure FrameStructure { get; set; }

        [IgnoreDataMember, XmlIgnore, JsonIgnore]
        public List<Model> Models
        {
            get { return _models; }
            set { _models = value; }
        }

        [NotMapped]
        [JsonProperty("Models")]
        public List<MiniModel> MiniModels
        {
            get
            {
                return _models.ConvertAll<MiniModel>(m => new MiniModel(m));
            }
            set
            {
                if (value != null)
                {
                    _models = value.ConvertAll<Model>(m => new Model(m, this));
                }
            }
        }

        [NotMapped]
        public IEnumerable<EFillingKind> AvailableFillings
        {
            get { return (EFillingKind[])Enum.GetValues(typeof(EFillingKind)); }
        }

        [NotMapped]
        public IEnumerable<EExtFinishing> AvailableExtFinishings
        {
            get
            {
                EExtFinishing[] finishings = (EExtFinishing[])Enum.GetValues(typeof(EExtFinishing));
                return Array.FindAll<EExtFinishing>(finishings, ef => ExtFinishings.HasFlag(ef));
            }
        }

        [NotMapped]
        public IEnumerable<EIntFinishing> AvailableIntFinishings
        {
            get { return (EIntFinishing[])Enum.GetValues(typeof(EIntFinishing)); }
        }

        [NotMapped]
        public IEnumerable<EFrameQuality> AvailableFrameQualities
        {
            get
            {
                EFrameQuality[] frameQualities = (EFrameQuality[])Enum.GetValues(typeof(EFrameQuality));
                return Array.FindAll<EFrameQuality>(frameQualities, fq => FrameQualities.HasFlag(fq));
            }
        }

        public void UpdateNonComposedPropertiesFrom(Range newRange)
        {
            Name = newRange.Name;
            ExtFinishings = newRange.ExtFinishings;
            Insulators = newRange.Insulators;
            FrameQualities = newRange.FrameQualities;
            FrameStructure = newRange.FrameStructure;
        }
    }
}