using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Map.Components
{
    [System.Serializable]
    public struct MapNodeNeighboursComponent : IComponent
    {
        // ��� ��� ���� ����� ������ ��� ������ � ����������� � ����������
        [SerializeField]
        private List<int> _neighboursEditor;

        private List<byte> _neighboursRuntime;

        public List<byte> node_neighbours
        {
            get
            {
                if (_neighboursRuntime == null || _neighboursRuntime.Count != _neighboursEditor.Count)
                {
                    _neighboursRuntime = _neighboursEditor.ConvertAll(x => (byte)x);
                }
                return _neighboursRuntime;
            }
            set
            {
                _neighboursRuntime = value;
                _neighboursEditor = value.ConvertAll(x => (int)x);
            }
        }
    }
}