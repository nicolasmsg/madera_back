using System.Collections.Generic;

namespace ConceptionDevisWS.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Nom { get; set; }
        public List<Composant> Composants { get; set; }
    }
}