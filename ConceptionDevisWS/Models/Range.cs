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
        public List<Model> Models { get; set; }

        [NotMapped]
        [JsonProperty("Models")]
        public List<MiniModel> MiniModels
        {
            get
            {
                if (Models == null)
                {
                    return null;
                }
                else
                {
                    return Models.ConvertAll<MiniModel>(m => new MiniModel(m));
                }
            }
            set
            {
                if (value != null)
                {
                    if (Models == null)
                    {
                        Models = new List<Model>();
                    }
                    Models.Clear();
                    foreach (MiniModel miniModel in value)
                    {
                        Models.Add(new Model(miniModel, this));
                    }
                }
                else
                {
                    Models = null;
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