using System;
using System.Collections.Generic;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Monster.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Monster.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonstersSpawnRequestSystem : ISystem
    {
        public World World { get; set; }

        private Filter _freeMonsterSpawnCells;

        private Request<GenerateMonstersRequest> _genMonstersRequest;
        private Request<SpawnNewMonsterRequest> _spawnRequest;

        public void OnAwake()
        {
            _freeMonsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .Without<TagOccupiedCell>()
                .With<CellPositionComponent>()
                .Build();

            _genMonstersRequest = World.GetRequest<GenerateMonstersRequest>();
            _spawnRequest = World.GetRequest<SpawnNewMonsterRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _genMonstersRequest.Consume())
            {

                var cells = PickRandomFreeCells(req.MosntersCount);

                foreach (var cell in cells)
                {
                    _spawnRequest.Publish(new SpawnNewMonsterRequest
                    {
                        CellEntity = cell
                    });
                }
            }
        }

        public void Dispose()
        {
            _freeMonsterSpawnCells.Dispose();
        }

        private List<Entity> PickRandomFreeCells(int MonsterCount)
        {
            int lenght = _freeMonsterSpawnCells.GetLengthSlow();

            List<Entity> cells = new();
            List<Entity> result = new();

            foreach (var cell in _freeMonsterSpawnCells)
            {
                cells.Add(cell);
            }

            var cycles = Math.Min(MonsterCount, lenght);

            for (int i = 0; i < cycles; i++)
            {
                var pick = UnityEngine.Random.Range(0, cells.Count);
                result.Add(cells[pick]);
                cells.RemoveAt(pick);
            }
            return result;
        }
    }
}


