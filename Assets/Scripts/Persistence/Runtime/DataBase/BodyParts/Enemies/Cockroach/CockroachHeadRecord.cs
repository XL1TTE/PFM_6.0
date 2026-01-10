using Persistence.Components;

namespace Persistence.DB
{
    public class CockroachHeadRecord : BodyPartRecord
    {
        public CockroachHeadRecord()
        {
            ID("bp_cockroach-head");

            With<Name>(new Name { m_Value = "Cockroach's Head" });

            With<TagBodyPart>();

            With<TagHead>();

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cockroach-head"
            });
        }
    }
}

