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
        public string Nom { get; set; }
        public List<Component> Components { get; set; }
    }
}