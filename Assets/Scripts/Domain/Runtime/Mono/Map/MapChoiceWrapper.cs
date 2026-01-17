
using Scellecs.Morpeh;
using System.Collections.Generic;

namespace Domain.Map.Mono{
    
    public sealed class MapChoiceWrapper
    {
        public Dictionary<CHOICE_SCRIPT_TYPE, IRequestData> request_type_data;
        //public CHOICE_SCRIPT_TYPE type;
        //public IRequestData request;
        public string res_text;

    }

    public enum CHOICE_SCRIPT_TYPE
    {
        GIVE_GOLD,
        TAKE_GOLD,
        GIVE_PARTS,
        SWAP_PARTS,
        SWAP_PARTS_BETWEEN,
        GIVE_HP,
        TAKE_HP
    }
}
