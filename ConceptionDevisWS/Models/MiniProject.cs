namespace ConceptionDevisWS.Models
{
    public class MiniProject
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public EProjectState State { get; set; }

        public MiniProject() { }

        public MiniProject(Project project)
        {
            Id = project.Id;
            Reference = project.Reference;
            Name = project.Name;
            State = project.State;
        }
    }
}