
using Gameplay.Common.Requests;
using Scellecs.Morpeh;
using UnityEngine;

namespace Project
{
    public class START : MonoBehaviour
    {
        void Start()
        {
            var req = World.Default.GetRequest<PlanningStageEnterRequest>();
            
            req.Publish(new PlanningStageEnterRequest{});
        }
    }
}
