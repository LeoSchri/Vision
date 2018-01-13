using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Information :BaseElementWN
    {
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public virtual Information Parent { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<Information> SubContents { get; set; }
        public virtual ICollection<Scene> Scenes { get; set; }
        public virtual ICollection<Plotline> Plotlines { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

        public Information() :base()
        {
            SubContents = new List<Information>();
            Scenes = new HashSet<Scene>();
            Plotlines = new HashSet<Plotline>();
            Characters = new HashSet<Character>();
            Locations = new HashSet<Location>();
        }
    }
}