using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class Scene: BaseElementWD
    {
        public int? OrderID { get; set; }

        [Display(Name="Point-in-time")]
        public string PointInTime { get; set; }

        public Mood Mood { get; set; } = Mood.NS;

        public Weather Weather { get; set; } = Weather.NS;

        [DataType(DataType.MultilineText)]
        public string Story { get; set; }

        public virtual Chapter Chapter { get; set; }
        public int? ChapterId { get; set; }

        public virtual Location Location { get; set; }
        public int? LocationId { get; set; }

        public virtual MainCharacter POVCharacter { get; set; }
        public int? POVCharacterId { get; set; }

        public virtual ICollection<Scene> DependentScenesF { get; set; }
        public virtual ICollection<Scene> DependentScenesA { get; set; }
        public virtual ICollection<Plotline> Plotlines { get; set; }
        [Display(Name="Attendant characters")]
        public virtual ICollection<Character> AttendantCharacters { get; set; }
        public virtual ICollection<RelationshipPhase> RelationshipChanges { get; set; }
        public virtual ICollection<CharacterEvolvement> CharacterEvolvements { get; set; }
        public virtual ICollection<Information> RelevantInformation { get; set; }

        public Scene():base()
        {
            Plotlines = new HashSet<Plotline>();
            AttendantCharacters = new HashSet<Character>();
            RelationshipChanges = new List<RelationshipPhase>();
            CharacterEvolvements = new List<CharacterEvolvement>();
            RelevantInformation = new HashSet<Information>();
            DependentScenesF = new HashSet<Scene>();
            DependentScenesA = new HashSet<Scene>();
        }
    }
}