using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConceptionDevisWS.Models
{
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
}