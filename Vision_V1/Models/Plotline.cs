using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Plotline: BaseElementWD
    {
        public virtual ICollection<Scene> Scenes { get; set; }
        [Display(Name="Refers to")]
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Plotline> DependentPlotlinesF { get; set; }
        public virtual ICollection<Plotline> DependentPlotlinesA { get; set; }
        public virtual ICollection<Information> RelevantInformation { get; set; }

        public Plotline():base()
        {
            Scenes = new HashSet<Scene>();
            Characters = new HashSet<Character>();
            DependentPlotlinesF = new HashSet<Plotline>();
            DependentPlotlinesA = new HashSet<Plotline>();
            RelevantInformation = new HashSet<Information>();
        }
    }
}