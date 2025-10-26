using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Gameplay.TargetSelection
{
    public sealed class TargetSelectInteraction : BaseInteraction, IOnTargetSelection
    {
        private CancellationTokenSource m_CurrentSelectionCancellation;
        private CancellationTokenSource m_LifetimeCancellation;

        public TargetSelectInteraction()
        {
            m_LifetimeCancellation = new CancellationTokenSource();
        }

        public async UniTask Execute(
            IEnumerable<Entity> a_cellOptions,
            TargetSelectionTypes a_type,
            uint a_maxTargets,
            World a_world,
            Result a_result)
        {

            // Cancel previous session.
            CancelSession();

            await UniTask.Yield();

            m_CurrentSelectionCancellation?.Dispose();

            // Create new token for this session.
            m_CurrentSelectionCancellation =
                CancellationTokenSource.CreateLinkedTokenSource(m_LifetimeCancellation.Token);


            var f_cells = a_world.Filter
                    .With<CellTag>()
                    .With<UnderCursorComponent>()
                    .Build();

            List<Entity> t_result = new();

            try
            {
                HighlighSelectOptions(a_cellOptions, a_world, true);

                while (t_result.Count != Math.Min(a_cellOptions.Count(), a_maxTargets))
                {
                    await UniTask.Yield();
                    // Exit if cancelation requested.
                    m_CurrentSelectionCancellation.Token.ThrowIfCancellationRequested();

                    Entity t_clickedCell = default;
                    if (IsPlayerClickedOnCell(f_cells, out t_clickedCell))
                    {
                        if (t_clickedCell.isNullOrDisposed(a_world)) { continue; }

                        if (IsCellMeetType(t_clickedCell, a_type, a_world))
                        {
                            t_result.Add(t_clickedCell);
                        }
                        else
                        {
                            // Play sound and stuff when invalid
                            Debug.Log("Not valid choose.");
                        }
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        t_result.Clear();
                        CancelSession();
                    }
                }

            }
            catch (OperationCanceledException)
            {
                t_result.Clear();
            }
            finally
            {
                HighlighSelectOptions(a_cellOptions, a_world, false);
                a_result.m_Value = t_result;
            }
        }

        private void CancelSession()
        {
            m_CurrentSelectionCancellation?.Cancel();
        }

        private bool IsCellMeetType(Entity a_cell, TargetSelectionTypes a_type, World a_world)
        {
            switch (a_type)
            {
                case TargetSelectionTypes.CELL_WITH_ENEMY:
                    if (F.IsOccupiedCell(a_cell, a_world))
                    {
                        return F.IsEnemy(GU.GetCellOccupier(a_cell, a_world), a_world);
                    }
                    break;
                case TargetSelectionTypes.CELL_WITH_MONSTER:
                    if (F.IsOccupiedCell(a_cell, a_world))
                    {
                        return F.IsMonster(GU.GetCellOccupier(a_cell, a_world), a_world);
                    }
                    break;
                case TargetSelectionTypes.CELL_EMPTY:
                    return !F.IsOccupiedCell(a_cell, a_world);
            }
            return false;
        }

        private void HighlighSelectOptions(IEnumerable<Entity> a_options, World a_world, bool isActive)
        {
            var stash_view = a_world.GetStash<CellViewComponent>();
            foreach (var cell in a_options)
            {
                if (stash_view.Has(cell) == false) { continue; }
                if (isActive)
                {
                    stash_view.Get(cell).Value.EnableSelectedLayer();

                }
                else
                {
                    stash_view.Get(cell).Value.DisableSelectedLayer();

                }
            }
        }

        private bool IsPlayerClickedOnCell(Filter a_CellsWithUnderCursor, out Entity clickedCell)
        {
            if (!a_CellsWithUnderCursor.IsEmpty()
                && Input.GetMouseButtonDown(0))
            {
                clickedCell = a_CellsWithUnderCursor.FirstOrDefault();
                return true;
            }
            clickedCell = default;
            return false;
        }
    }

}


