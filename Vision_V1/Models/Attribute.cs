using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Attribute : BaseElement
    {
        public int? OrderID { get; set; }

        public int Size { get; set; }

        public virtual ICollection<CharacterAttribute> CharacterAttributes { get; set; }

        public Attribute():base()
        {
            CharacterAttributes = new List<CharacterAttribute>();
        }
    }
}