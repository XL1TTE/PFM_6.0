using System.Collections.Generic;

namespace Domain.Monster.Mono
{

    public sealed class MonsterData
    {

        public MonsterData(string monsterName, string head_id, string nearArm_id, string farArm_id, string body_id, string nearLeg_id, string farLeg_id)
        {
            m_MonsterName = monsterName;
            Head_id = head_id;
            NearArm_id = nearArm_id;
            FarArm_id = farArm_id;
            Body_id = body_id;
            NearLeg_id = nearLeg_id;
            FarLeg_id = farLeg_id;
        }

        public MonsterData(string data_head, string data_arml, string data_armr, string data_torso, string data_legl, string data_legr)
        {
            Head_id = data_head;
            NearArm_id = data_arml;
            FarArm_id = data_armr;
            Body_id = data_torso;
            NearLeg_id = data_legl;
            FarLeg_id = data_legr;
        }

        public string m_MonsterName;

        public string Head_id;
        public string NearArm_id;
        public string FarArm_id;
        public string Body_id;
        public string NearLeg_id;
        public string FarLeg_id;
        private string data_head;
        private string data_arml;
        private string data_armr;
        private string data_torso;
        private string data_legl;
        private string data_legr;

        public void SetName(string name)
        {
            m_MonsterName = name;
        }
    }
}
