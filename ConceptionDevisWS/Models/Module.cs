using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConceptionDevisWS.Models
{
    [Table("Modules")]
    public class Module : IIdentifiable
    {
        public int Id { get; set; }
        [StringLength(50), Index(IsUnique = true)]
        public string Reference
        {
            get { return "MOD-" + Id; }
        }
        [Column("Nom")]
        public string Name { get; set; }
        public List<Component> Components { get; set; }

        [Column("Prix")]
        public double Price { get; set; }

        public void UpdateNonComposedPropertiesFrom(Module newModule)
        {
            Name = newModule.Name;
        }
    }
}