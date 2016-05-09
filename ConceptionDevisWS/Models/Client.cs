using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

//! Models are data containers without much behaviour (save initialization related).
namespace ConceptionDevisWS.Models
{
    public class Client : IIdentifiable
    {

        private List<Project> _projects = new List<Project>();
        public int Id { get; set; }
        [StringLength(50), Index(IsUnique=true)]
        public string Reference
        {
            get { return "CLI-" + Id; }
        }
        [StringLength(30)]
        [Column("Prenom")]
        public string FirstName { get; set; }
        [StringLength(30)]
        [Column("Nom")]
        public string LastName { get; set; }
        [StringLength(100)]
        [Column("Adresse")]
        public string Address { get; set; }
        [StringLength(30)]
        [Column("Ville")]
        public string City { get; set; }
        [Column("CodePostal")]
        public int ZipCode { get; set; }
        [StringLength(10)]
        [Column("Telephone")]
        public string Phone { get; set; }
        [EmailAddress, StringLength(50)]
        public string Email { get; set; }
        [Column("DateDeNaissance")]
        public DateTime Birdthdate { get; set; }
        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        public List<Project> Projects
        {
            get { return _projects; }
            set { _projects = value; }
        }

        [IgnoreDataMember, XmlIgnore, JsonIgnore]
        public User User { get; set; }

        //! used to avoid recursive type serialization, also it'll be clear only MiniProject.Id will matter 
        //! for update operations (that's an association display entity)
        [NotMapped]
        [JsonProperty("Projects")]
        public List<MiniProject> MiniProjects
        {
            get
            {
                return _projects.ConvertAll<MiniProject>(p => new MiniProject(p));
            } 
            set
            {
                if (value != null)
                {
                    _projects = value.ConvertAll<Project>(mp => new Project(mp, this));
                }
            }
        }

        public Client() { }

        public Client(MiniClient miniClient)
        {
            if (miniClient != null)
            {

                Id = miniClient.Id;
                FirstName = miniClient.FirstName;
                LastName = miniClient.LastName;
                Address = miniClient.Address;
                City = miniClient.City;
                ZipCode = miniClient.ZipCode;
                Phone = miniClient.Phone;
                Email = miniClient.Email;
                Birdthdate = miniClient.Birdthdate;
            }
        }

        public void UpdateNonComposedPropertiesFrom(Client newClient)
        {
            Address = newClient.Address;
            City = newClient.City;
            ZipCode = newClient.ZipCode;
            Birdthdate = newClient.Birdthdate;
            FirstName = newClient.FirstName;
            LastName = newClient.LastName;
            Phone = newClient.Phone;
            Email = newClient.Email;
        }

    }
}