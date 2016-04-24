using System;

namespace ConceptionDevisWS.Models
{
    /// <summary>
    /// Model used in web responses to prevent a recursive structure with <see cref="ConceptionDevisWS.Models.Client"/>.<see cref="ConceptionDevisWS.Models.Project"/>s[n].<see cref="ConceptionDevisWS.Models.Client"/>.
    /// </summary>
    public class MiniProject
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public EProjectState State { get; set; }
        public DateTime CreationDate { get; set; }
        public string TechnicalSheetPath { get; set; }

        public MiniProject() { }

        public MiniProject(Project project)
        {
            Id = project.Id;
            Reference = project.Reference;
            Name = project.Name;
            State = project.State;
            CreationDate = project.CreationDate;
            TechnicalSheetPath = project.TechnicalSheetPath;
        }
    }
}