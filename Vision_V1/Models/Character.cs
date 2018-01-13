using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class Character:BaseElementWN
    {
        [DataType(DataType.MultilineText)]
        public string History { get; set; }

        public string HistoryShort()
        {
            return string.IsNullOrEmpty(History) ? "" : (History.Length > 10 ? $"{History.Substring(0, 10)}..." : History);
        }

        [Display(Name="MC")]
        public bool IsMainCharacter { get; set; }

        public int? MainCharacterId { get; set; }
        public virtual MainCharacter MainCharacter { get; set; }

        [NotMapped]
        [Display(Name="Personality")]
        [DataType(DataType.MultilineText)]
        public string FirstCharacterNature { get; set; }

        public virtual ICollection<Scene> Scenes { get; set; }
        public virtual ICollection<CharacterEvolvement> EvolvementSteps { get; set; }
        public virtual ICollection<Relationship> Relationships { get; set; }
        public virtual ICollection<Relationship> RelationshipCounterparts { get; set; }
        public virtual ICollection<Plotline> Plotlines { get; set; }
        public virtual ICollection<Information> RelevantInformation { get; set; }

        public virtual List<CharacterAttribute> CharacterAttributes { get; set; }

        public Character():base()
        {
            Scenes = new HashSet<Scene>();
            EvolvementSteps = new List<CharacterEvolvement>();
            Relationships = new List<Relationship>();
            RelationshipCounterparts = new List<Relationship>();
            Plotlines = new HashSet<Plotline>();
            RelevantInformation = new HashSet<Information>();
            CharacterAttributes = new List<CharacterAttribute>();
        }
    }
}