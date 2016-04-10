using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    public class Client : IIdentifiable
    {
        public int Id { get; set; }
        [StringLength(50), Index(IsUnique=true)]
        public string Referrence
        {
            get { return "CLI-" + Id; }
        }
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
        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        public List<Project> Projects { get; set; }

        // used to avoid recursive type serialization, also it'll be clear only MiniProject.Id will matter 
        // for update operations (that's an association display entity)
        [NotMapped]
        [JsonProperty("Projects")]
        public List<MiniProject> MiniProjects
        {
            get
            {
                if (Projects == null)
                {
                    return null;
                }
                else
                {
                    return Projects.ConvertAll<MiniProject>(p => new MiniProject(p));
                }
            } 
            set
            {
                if (value != null)
                {
                    if(Projects == null)
                    {
                        Projects = new List<Project>();
                    }
                    Projects.Clear();
                    foreach(MiniProject miniProj in value)
                    {
                        Projects.Add(new Project(miniProj, this));
                    }
                }
            }
        }

        public void UpdateNonComposedPropertiesFrom(Client newClient)
        {
            Address = newClient.Address;
            City = newClient.City;
            ZipCode = newClient.ZipCode;
            Birthdate = newClient.Birthdate;
            FirstName = newClient.FirstName;
            LastName = newClient.LastName;
            Phone = newClient.Phone;
            Email = newClient.Email;
        }

    }
}