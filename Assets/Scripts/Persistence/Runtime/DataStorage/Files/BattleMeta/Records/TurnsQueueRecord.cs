
using System;
using System.Collections.Generic;
using Domain.UI.Widgets;
using Persistence.DS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace DS.Files
{
    [Serializable]
    public struct TurnsQueueRecord : IDataStorageRecord
    {
        public List<Entity> m_Queue;

        /// <summary>
        /// Map, to associate ui controller with  member of turn queue.
        /// </summary>
        public IntHashMap<TurnQueueElementView> m_QueueElementsRenderMap;

        public Entity m_LastTurnTaker;
        public Entity m_CurrentTurnTaker;
    }
}
