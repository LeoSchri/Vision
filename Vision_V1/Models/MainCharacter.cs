using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vision_V1.Models
{
    public class MainCharacter:BaseElementWN
    {
        public virtual ICollection<Scene> POVScenes { get; set; }

        public MainCharacter():base()
        {
            POVScenes = new List<Scene>();

        }
    }
}