using UnityEngine;

namespace Domain.BattleField.Mono
{
    
    public class BattleGridGenerator : MonoBehaviour
    {
        [Header("Grid Settings")]
        public Vector2Int gridSize = new Vector2Int(10, 10);
        public float CellsGap = 1f;
        
        public CellGenRule CellGenRulePrefab;

        public bool EditMode = false;


        [SerializeField, HideInInspector]
        public InspectorCell[,] _inspectorGrid;
        
        private GameObject[,] _gridCache;
        private Transform _gridParent;

#if UNITY_EDITOR

        public void GenerateGrid()
        {
            ClearGrid();

            _gridParent = CreateNewGridContainer();

            _gridCache = new GameObject[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (_inspectorGrid[x, y].isSelected)
                    {
                        var Position = CalculateCellPosition(x, y, CellsGap);
                        
                        var cell = CreateCellGenRule(Position, $"Cell ({x}, {y})");
                        
                        cell.GridPosition = new Vector2Int(x, y);
                        
                        _gridCache[x, y] = cell.gameObject;
                    }
                }
            }
        }
        
        private Transform CreateNewGridContainer(){
            var container = new GameObject("GRID");
            container.transform.SetParent(transform, false);
            return container.transform;
        }
        
        private Vector3 CalculateCellPosition(int x, int y, float cellSize) {
            float offsetX = (gridSize.x - 1) * cellSize * 0.5f;
            float offsetY = (gridSize.y - 1) * cellSize * 0.5f;

            return new Vector3(
                x * cellSize - offsetX,
                y * cellSize - offsetY,
                0
            );
        }
        private CellGenRule CreateCellGenRule(Vector3 position, string cellName = null){
            var cellGenRule = Instantiate(CellGenRulePrefab, position, Quaternion.identity, _gridParent);
            if(cellName != null){
                cellGenRule.name = cellName;
            } 
            return cellGenRule;
        }
        
        
        public void ClearGrid()
        {
            if (_gridCache == null) return;

            foreach (var cell in _gridCache)
            {
                if (cell != null) DestroyImmediate(cell);
            }

            DestroyImmediate(_gridParent.gameObject);

            _gridCache = null;
        }

        
        void OnValidate()
        {
            if (_inspectorGrid == null || _inspectorGrid.GetLength(0) != gridSize.x || _inspectorGrid.GetLength(1) != gridSize.y)
            {
                _inspectorGrid = new InspectorCell[gridSize.x, gridSize.y];
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        _inspectorGrid[x, y] = new InspectorCell();
                    }
                }
            }
        }

#endif
    }

}
