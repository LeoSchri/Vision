using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class RelationshipPhase : BaseElementWN
    {
        public virtual Scene Scene { get; set; }
        public int? SceneId { get; set; }

        public Relation Relation { get; set; }

        public virtual Relationship Relationship { get; set; }
        public int? RelationshipId { get; set; }

        public Closeness Closeness { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Thoughts on CP character")]
        public string ThoughtsOnCounterpart { get; set; }

        public string ThoughtsOnCounterpartShort()
        {
            return string.IsNullOrEmpty(ThoughtsOnCounterpart) ? "" : (ThoughtsOnCounterpart.Length > 30 ? $"{ThoughtsOnCounterpart.Substring(0, 30)}..." : ThoughtsOnCounterpart);
        }

        public RelationshipPhase()
        {

        }
    }
}