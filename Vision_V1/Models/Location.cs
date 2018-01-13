using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Vision_V1.Models.Types;

namespace Vision_V1.Models
{
    public class Location : BaseElementWN
    {
        public virtual Location Parent { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<Location> SubLocations { get; set; }
        public virtual ICollection<Scene> Scenes { get; set; }
        public virtual ICollection<Information> RelevantInformation { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string DescriptionShort()
        {
            return string.IsNullOrEmpty(Description) ? "" : (Description.Length > 20 ? $"{Description.Substring(0, 20)}..." : Description);
        }

        public Orientation Orientation { get; set; } = Orientation.NS;

        public Climate Climate { get; set; } = Climate.NS;

        public LocationType Type { get; set; } = LocationType.NS;

        public Location() : base()
        {

            SubLocations = new List<Location>();
            Scenes = new List<Scene>();
            RelevantInformation = new HashSet<Information>();
        }
    }
}