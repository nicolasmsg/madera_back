using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConceptionDevisWS.Models
{
    public class Client : IIdentifiable
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        public int ZipCode { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }
        [EmailAddress, StringLength(50)]
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }

        public List<Projet> Projets { get; set; }

    }
}