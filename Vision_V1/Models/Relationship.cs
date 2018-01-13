using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Relationship : BaseElementWN
    {
        public virtual Character Character { get; set; }
        public int? CharacterId { get; set; }

        public virtual Character CounterpartCharacter { get; set; }
        public int? CounterpartCharacterId { get; set; }

        public virtual ICollection<RelationshipPhase> Phases { get; set; }

        public Relationship()
        {
            Phases = new List<RelationshipPhase>();
        }
    }
}