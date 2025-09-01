using System.Net.NetworkInformation;
using Scellecs.Morpeh;

namespace Persistence.DB{
    public abstract class IDbRecord{
        protected Entity _record;
        
        protected IDbRecord(){
            _record = DataBase.CreateRecord();
        }
        
        protected void With<T>() where T : struct, IComponent
        {
            DataBase.SetRecord(_record, new T());
        }
        
        protected void With<T>(T value) where T: struct, IComponent{ 
            DataBase.SetRecord(_record, value);
        }
    }
}

