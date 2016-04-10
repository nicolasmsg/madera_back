using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConceptionDevisWS.Models
{
    [Table("Projets")]
    public class Project : IIdentifiable
    {
        private string _stateStr;
        private EProjectState _state;

        public int Id { get; set; }
        [StringLength(50),Index(IsUnique = true)]
        public string Reference
        {
            get { return "PROJ-" + Id; }
        }
        [StringLength(30), Column("Nom")]
        public string Name { get; set; }
        public Client Client { get; set; }

        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        [Column("Etat")]
        public string StateStr
        {
            get { return _stateStr; }
            set
            {
                _stateStr = value;
                _state = (EProjectState)Enum.Parse(typeof(EProjectState), _stateStr);
            }
        }
        [NotMapped]
        public EProjectState State
        {
            get { return _state; }
            set
            {
                _state = value;
                _stateStr = _state.ToString();
            }
        }

        public Project() { }
        public Project(MiniProject miniProj, Client client)
        {
            Id = miniProj.Id;
            Name = miniProj.Name;
            State = miniProj.State;
            Client = client;
        }

        public void UpdateNonComposedPropertiesFrom(Project newProject)
        {
            Id = newProject.Id;
            Name = newProject.Name;
            State = newProject.State;
        }
    }
}