using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class BaseElement_B
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Last modified")]
        public DateTime LastModified { get; set; }

        public BaseElement_B()
        {

        }
    }

    public class BaseElement: BaseElement_B
    {
        public Project Project { get; set; }
        public int? ProjectId { get; set; }


        public BaseElement():base()
        {

        }
    }

    public class BaseElementWN : BaseElement
    {

        public virtual ICollection<Note> Notes { get; set; }

        public BaseElementWN() : base()
        {
            Notes = new List<Note>();
        }
    }

    public class BaseElementWD: BaseElementWN
    {
        [Display(Name = "Estimated duration")]
        public Duration EstimatedDuration { get; set; } = Duration.NS;

        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        public string SummaryShort()
        {
            return string.IsNullOrEmpty(Summary) ? "" : (Summary.Length > 50 ? $"{Summary.Substring(0, 50)}..." : Summary);
        }

        public BaseElementWD():base()
        {

        }
    }

}