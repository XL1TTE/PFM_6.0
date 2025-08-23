using System.Collections;
using System.Collections.Generic;
using Scellecs.Morpeh;

namespace Domain.Extentions
{
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
