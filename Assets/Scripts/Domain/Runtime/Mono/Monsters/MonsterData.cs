using System.Collections.Generic;

namespace Domain.Monster.Mono
{

    public sealed class MonsterData
    {

        public MonsterData(
            string data_head, 
            string data_arml, string data_armr, 
            string data_torso, 
            string data_legl, string data_legr,
            int data_hp,
            string monsterName = "")
        {
            m_MonsterName = monsterName;
            Head_id = data_head;
            NearArm_id = data_arml;
            FarArm_id = data_armr;
            Body_id = data_torso;
            NearLeg_id = data_legl;
            FarLeg_id = data_legr;
            current_hp = data_hp;
        }

        public string m_MonsterName;

        public string Head_id;
        public string NearArm_id;
        public string FarArm_id;
        public string Body_id;
        public string NearLeg_id;
        public string FarLeg_id;

        public int current_hp;

        public void SetHP(int hp)
        {
            current_hp = hp;
        }
        public void SetName(string name)
        {
            m_MonsterName = name;
        }
    }
}
