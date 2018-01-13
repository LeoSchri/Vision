using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Book:BaseElementWD
    {
        [ForeignKey("MainPlotline")]
        public int? PlotlineId { get; set; }
        public virtual Plotline MainPlotline { get; set; }

        public virtual ICollection<Arc> Arcs { get; set; }

        public Book():base()
        {
            Arcs = new List<Arc>();
        }
    }
}