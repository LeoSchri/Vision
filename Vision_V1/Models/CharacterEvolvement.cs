using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class CharacterEvolvement :BaseElementWN
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name ="Personality")]
        public string CharacterDescription { get; set; }

        public string CharacterDescriptionShort()
        {
            return string.IsNullOrEmpty(CharacterDescription) ? "" : (CharacterDescription.Length > 50 ? $"{CharacterDescription.Substring(0, 50)}..." : CharacterDescription);
        }

        public virtual Character Character { get; set; }
        public int? CharacterId { get; set; }

        public virtual Scene Scene { get; set; }
        public int? SceneId { get; set; }

        public CharacterEvolvement():base()
        {

        }
    }
}