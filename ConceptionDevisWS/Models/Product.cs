using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConceptionDevisWS.Models
{
    public class Product : IIdentifiable
    {
        public int Id { get; set; }

        [StringLength(50), Index(IsUnique = true)]
        public string Reference
        {
            get { return "PROD-" + Id; }
        }


        [Column("Nom")]
        [StringLength(50)]
        public string Name { get; set; }

        public Model Model { get; set; }
        [Column("CheminPlan")]
        public string SchemaPath { get; set; }

        [Column("DonneesGraphiques")]
        [StringLength(4000)]
        public string Graphic { get; set; }

        [Column("Prix")]
        public double Price { get; set; }

        public void UpdateNonComposableProperties(Product newProduct)
        {
            Name = newProduct.Name;
            SchemaPath = newProduct.SchemaPath;
        }
    }
}