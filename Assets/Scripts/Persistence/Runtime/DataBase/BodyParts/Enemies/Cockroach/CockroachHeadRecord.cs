using Persistence.Components;

namespace Persistence.DB
{
    public class CockroachHeadRecord : BodyPartRecord
    {
        public CockroachHeadRecord()
        {
            ID("bp_cockroach-head");

            With<Name>(new Name("CockroachHeadRecord_name"));

            With<TagBodyPart>();

            With<TagHead>();

            With<AbilityProvider>(new AbilityProvider
            {
                m_AbilityTemplateID = "abt_cockroach-head"
            });
        }
    }
}

