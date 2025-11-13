using System.Collections.Generic;

namespace Domain.Monster.Mono
{

    public sealed class MonsterData
    {

        public MonsterData(string head_id, string nearArm_id, string farArm_id, string body_id, string nearLeg_id, string farLeg_id)
        {
            Head_id = head_id;
            NearArm_id = nearArm_id;
            FarArm_id = farArm_id;
            Body_id = body_id;
            NearLeg_id = nearLeg_id;
            FarLeg_id = farLeg_id;
        }

        public string Head_id;
        public string NearArm_id;
        public string FarArm_id;
        public string Body_id;
        public string NearLeg_id;
        public string FarLeg_id;

    }
}
