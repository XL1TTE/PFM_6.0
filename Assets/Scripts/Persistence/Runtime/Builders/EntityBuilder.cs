using System.Collections.Generic;
using Core.Components;
using Scellecs.Morpeh;

namespace Persistence.Buiders{
    public abstract class EntityBuilder{
         
       // public abstract EntityBuilder With<T>(T t) where T: struct, IComponent;
        
        public abstract Entity Build();
    }
}
