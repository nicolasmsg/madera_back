using System.Collections.Generic;

namespace ConceptionDevisWS.Models
{
    public class Module : IIdentifiable
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Nom { get; set; }
        public List<Composant> Composants { get; set; }
    }
}