using System;
using Core.Components;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Events;
using Gameplay.Features.Monster.Requests;
using Persistence.Buiders;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterSpawnSystem : ISystem
    {
        public World World { get; set; }

        private Stash<CellPositionComponent> _cellPositionStash;
        private Stash<TransformRefComponent> _monsterTransformStash;

        private Request<SpawnNewMonsterRequest> _spawnRequests;
        private Event<CellOccupiedEvent> _cellOccupiedEvent;
        
        
        private MonsterBuilder _monsterBuilder;

        public void OnAwake()
        {
            _cellPositionStash = World.GetStash<CellPositionComponent>();
            _monsterTransformStash = World.GetStash<TransformRefComponent>();

            _spawnRequests = World.GetRequest<SpawnNewMonsterRequest>();
            _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();

            _monsterBuilder = new MonsterBuilder(World);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _spawnRequests.Consume())
            {
                SpawnNewMonster(req);
            }
        }

        public void Dispose()
        {
        }


        private void SpawnNewMonster(SpawnNewMonsterRequest req)
        {
            var cellPos = _cellPositionStash.Get(req.CellEntity);

            Entity monster = _monsterBuilder
                .AttachHead("mp_DammyHead")
                .AttachBody("mp_DammyTorso")
                .AttachFarArm("mp_DammyArm")
                .AttachNearArm("mp_DammyArm")
                .AttachNearLeg("mp_DammyLeg")
                .AttachFarLeg("mp_DammyLeg")
                .Build();

            if (!_monsterTransformStash.Has(monster))
            {
                throw new Exception("Monster entity does not have a transform component. Can't spawn monster without it!");
            }
            ref var monsterTransform = ref _monsterTransformStash.Get(monster);
            monsterTransform.TransformRef.position =
                new UnityEngine.Vector3(cellPos.global_x, cellPos.global_y, cellPos.global_y * 0.01f);

            _cellOccupiedEvent.NextFrame(new CellOccupiedEvent
            {
                CellEntity = req.CellEntity,
                OccupiedBy = monster
            });

        }
    }
}


