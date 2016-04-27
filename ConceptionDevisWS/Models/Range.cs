using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public List<Model> Models { get; set; }

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