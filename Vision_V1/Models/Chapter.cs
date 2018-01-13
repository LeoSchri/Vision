using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Chapter:BaseElementWD
    {
        public virtual ICollection<Scene> Scenes { get; set; }

        public virtual Arc Arc { get; set; }
        public int? ArcId { get; set; }

        public Chapter():base()
        {
            Scenes = new List<Scene>();
        }
    }
}