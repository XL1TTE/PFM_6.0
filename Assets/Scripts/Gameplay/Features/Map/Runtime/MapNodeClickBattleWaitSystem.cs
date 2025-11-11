using Domain.Map.Events;
using DS.Files;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using UnityEngine.SceneManagement;

namespace Gameplay.Map.Systems
{
    public sealed class MapNodeClickBattleWaitSystem : ISystem
    {
        public World World { get; set; }

        private Event<MapNodeClickedEvent> ev_clicked_node;

        private Stash<MapNodeEventId> nodeEvIDsStash;
        private Stash<MapNodeEventType> nodeTypesStash;
        public void OnAwake()
        {
            ev_clicked_node = World.GetEvent<MapNodeClickedEvent>();

            nodeEvIDsStash = World.GetStash<MapNodeEventId>();
            nodeTypesStash = World.GetStash<MapNodeEventType>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in ev_clicked_node.publishedChanges)
            {
                if (evt.node_entity != null)
                {
                    var ent = evt.node_entity;

                    ref var mapNodeEvTypeComponent = ref nodeTypesStash.Get(ent);

                    if (mapNodeEvTypeComponent.event_type == EVENT_TYPE.BATTLE)
                    {
                        ref var mapNodeEvIDComponent = ref nodeEvIDsStash.Get(ent);


                        //ref var crusadeBattleId = ref DataStorage.GetRecordFromFile<Crusade, CurrentBattleId>();
                        //crusadeBattleId.battle_event_id = mapNodeEvIDComponent.event_id;

                        ref var battleConfig = ref DataStorage.GetRecordFromFile<BattleConfig, LoadConfig>();

                        if (DataBase.TryFindRecordByID(mapNodeEvIDComponent.event_id, out var @event)) {
                            var prefab = DataBase.GetRecord<PrefabComponent>(@event);
                            battleConfig.m_prefab_level = prefab.Value;
                        }


                        //SceneManager.LoadScene("BattleField");


                        break;
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}