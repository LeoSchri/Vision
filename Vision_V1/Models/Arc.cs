using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Arc : BaseElementWD
    {
        public Book Book { get; set; }
        public int? BookId { get; set; }

        [ForeignKey("MainPlotline")]
        public int? PlotlineId { get; set; }
        public virtual Plotline MainPlotline { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }

        public Arc():base()
        {
            Chapters = new List<Chapter>();
        }
    }
}