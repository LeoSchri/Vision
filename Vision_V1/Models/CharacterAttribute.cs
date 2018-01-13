using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class CharacterAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? CharacterId { get; set; }
        public Character Character { get; set; }

        public int? AttributeId { get; set; }
        public Attribute Attribute { get; set; }

        public string Value { get; set; }
    }
}