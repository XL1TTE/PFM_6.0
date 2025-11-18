using Persistence.Components;

namespace Persistence.DB
{
    public class CowHeadRecord : BodyPartRecord // <-- Обязательное наследование.
    {
        public CowHeadRecord() // <-- Все происходит в конструкторе.
        {
            ID("bp_cow-head"); // <-- Обязательный идентификатор.

            With<TagBodyPart>(); // <-- Идентификатор, при помощи которого, можно будет найти
                                 //     эту запись через фильтр по частям тел.

            With<TagHead>(); // <-- Идентификатор, при помощи которого, можно будет найти
                             //     эту запись через фильтр по частям тел типа голова.

            With<AbilityProvider>(new AbilityProvider // <-- Связывает часть тела и спсособность.
            {
                m_AbilityTemplateID = "abt_cow-head"
            });
        }
    }
}

