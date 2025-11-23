
using Scellecs.Morpeh;

namespace Domain.Map.Mono{
    
    public sealed class MapChoiceWrapper
    {

        public CHOICE_SCRIPT_TYPE type;
        public IRequestData request;
        public string res_text;

    }

    public enum CHOICE_SCRIPT_TYPE
    {
        GIVE_GOLD,
        ATTACK_ASS,
        TAKE_GOLD
    }
}
