using System.Collections.Generic;

namespace ConceptionDevisWS.Models
{
    public class MiniModel
    {
        public int Id { get; set; }

        public string Reference
        {
            get { return "MODE-" + Id; }
        }

        public string Name { get; set; }

        public EFillingKind Filling { get; set; }

        public EExtFinishing ExtFinishing { get; set; }

        public EIntFinishing IntFinishing { get; set; }

        public EFrameQuality FrameQuality { get; set; }

        public string ImagePath { get; set; }

        public double BasePricePercentage { get; set; }

        public List<Module> Modules { get; set; }

        public MiniModel() { }

        public MiniModel(Model model)
        {
            Id = model.Id;
            Name = model.Name;
            Filling = model.Filling;
            ExtFinishing = model.ExtFinishing;
            IntFinishing = model.IntFinishing;
            FrameQuality = model.FrameQuality;
            ImagePath = model.ImagePath;
            BasePricePercentage = model.BasePricePercentage;

        }
    }
}