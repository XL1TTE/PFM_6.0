using System.Collections.Generic;
using System.Linq;

namespace Core.Utilities.Extentions{
    public static class ListExtentions{
        public static T Pop<T>(this List<T> list){
            var value = list.Last();
            list.RemoveAt(list.Count-1);
            return value;
        }
    }
    
}
