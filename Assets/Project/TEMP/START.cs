using ECS.Components;
using ECS.Components.Monsters;
using ECS.Requests;
using Scellecs.Morpeh;
using UnityEngine;

namespace Project
{
    public class START : MonoBehaviour
    {
        
        void Start()
        {
            /* ########################################## */
            /*       Request for planing game state.      */
            /* ########################################## */
            var planningStageEnterReq = World.Default.GetRequest<PlanningStageEnterRequest>();
            planningStageEnterReq.Publish(new PlanningStageEnterRequest{});

        }
    }
}
