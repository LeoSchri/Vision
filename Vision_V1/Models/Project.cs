using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Project:BaseElement_B
    {
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Plotline> Plotlines { get; set; }
        public virtual ICollection<Scene> Scenes { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<MainCharacter> MainCharacters { get; set; }
        public virtual ICollection<Information> Information { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public Project():base()
        {
            Books = new List<Book>();
            Plotlines = new List<Plotline>();
            Scenes = new List<Scene>();
            Characters = new List<Character>();
            Information = new List<Information>();
            Locations = new List<Location>();
            Attributes = new List<Attribute>();
        }
    }
}