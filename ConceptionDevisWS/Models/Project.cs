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
        private Client _client;
        private MiniClient _miniClient;

        public int Id { get; set; }
        [StringLength(50),Index(IsUnique = true)]
        public string Reference
        {
            get { return "PROJ-" + Id; }
        }
        [StringLength(30), Column("Nom")]
        public string Name { get; set; }
        [IgnoreDataMember,XmlIgnore,JsonIgnore]
        [Required]
        public Client Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                _miniClient = new MiniClient(_client);
            }
        }
        [Column("DateCreation")]
        public DateTime CreationDate { get; set; }

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

        [NotMapped]
        [JsonProperty("Client")]
        public MiniClient MiniClient
        {
            get { return _miniClient; }
            set
            {
                _miniClient = value;
                _client = new Client(_miniClient);
            }
        }

        public Project() { }
        public Project(MiniProject miniProj, Client client)
        {
            Id = miniProj.Id;
            Name = miniProj.Name;
            State = miniProj.State;
            Client = client;
            CreationDate = miniProj.CreationDate;
        }

        public void UpdateNonComposedPropertiesFrom(Project newProject)
        {
            Id = newProject.Id;
            Name = newProject.Name;
            State = newProject.State;
            CreationDate = newProject.CreationDate;
        }
    }
}