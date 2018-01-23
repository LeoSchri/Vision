using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class Attribute : BaseElement
    {
        public int? OrderID { get; set; }

        public InputFieldSize Size { get; set; }

        [Display(Name = "Shown on table")]
        public bool ShowInTable { get; set; } = true;

        public virtual ICollection<CharacterAttribute> CharacterAttributes { get; set; }

        public Attribute():base()
        {
            CharacterAttributes = new List<CharacterAttribute>();
        }
    }
}