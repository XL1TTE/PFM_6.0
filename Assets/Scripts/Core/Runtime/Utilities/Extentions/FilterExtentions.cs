using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;

namespace Core.Utilities.Extentions{
    public static class FilterExtentions{
        public static IEnumerable<Entity> AsEnumerable(this Filter filter){
            List<Entity> enumerable = new List<Entity>();
            foreach (var e in filter)
            {
                enumerable.Add(e);
            }
            return enumerable;
        }
    }
    
}
