using UnityEngine;

namespace Domain.BattleField.Mono
{
    public class BattleFieldController : MonoBehaviour{
        public CellGenRule[,] Grid;
        public Vector2Int GridSize;

        public void InitializeGrid(){
            if(Grid == null){return;}

            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    if(Grid[x, y] == null){continue;}
                    
                    
                }
            }
        }
    }
}
