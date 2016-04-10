﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConceptionDevisWS.Models.Auth
{
    public class RevokedToken
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50), Index(IsUnique=true)]
        public string Name { get; set; }
    }
}