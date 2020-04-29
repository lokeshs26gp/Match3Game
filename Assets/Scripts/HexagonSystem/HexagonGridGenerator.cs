
using UnityEngine;
namespace HexagonSystem
{
    internal enum HexCalculation { Edge = 0, Cornor = 30 };//we can calculate hexagon neighbours by 2 ways
    public enum GenerationDirection { Top2Botton = -1, Bottom2Top = 1 }

    public class HexagonGridGenerator 
    {
        private const float offsetY = 0.866025404f;//sq(3)*0.5f offset between outercircle and innercircle radius
        private const float outerRadius = 1.0f;
        private const float innerRadius = outerRadius * offsetY;
        private GameObject gridsContainer;

        public HexagonGridGenerator(DataSO.LevelDataSO levelData,ref HexagonBlock[] hexagonBlocks,ref Bounds gridBounds)
        {
            gridBounds = new Bounds(Vector3.zero, Vector3.zero);
            hexagonBlocks = new HexagonBlock[levelData.gridDimentions.x * levelData.gridDimentions.y];
            GameObject hexblockPrefab = levelData.hexagonPrefab;
            GameObject gridsContainer = new GameObject("GridContainer");
            Vector3 newRow = Vector3.zero;// gridStartposition;
            Vector3 position = newRow;
            int generationDirection = (int)GenerationDirection.Top2Botton;
            for (int row = 0; row < levelData.gridDimentions.x; row++)
            {
                position = newRow;
                for (int column = 0; column < levelData.gridDimentions.y; column++)
                {
                    position = GetEdge(position, 0, levelData.uniformBlockscale, HexCalculation.Edge);
                    GameObject block = Object.Instantiate(hexblockPrefab, position, Quaternion.identity, gridsContainer.transform);
                    block.transform.localScale = Vector3.one * levelData.uniformBlockscale;
                    string debugString = "(" + row + "," + column + ")";
                    block.name = debugString;
                    int index = GetIndex(row, column, levelData.gridDimentions.x);

                    HexagonViewTempInfo viewinfo =  block.GetComponent<HexagonViewTempInfo>().GetInfo(ref gridBounds, debugString);
                    HexagonBlock hexagonBlock = new HexagonBlock(new Vector2Int(row, column), block.transform.position, viewinfo);

                    hexagonBlocks[index] = hexagonBlock;
                    
                }

                float angleDeg = row % 2 == 0 ? 120.0f : 60.0f;// 60 + 60 * row % 2;//ToDo:This can be randomized
                newRow = GetEdge(newRow, angleDeg * generationDirection, levelData.uniformBlockscale, HexCalculation.Edge);
            }

            SetUpNeighbours(ref hexagonBlocks, levelData.gridDimentions);
            gridsContainer.AddComponent<OptimizationSystem.CombineMesh>();
        }

        /// <summary>
        /// Hexagon grid positioning calculation
        /// </summary>
        private Vector3 GetEdge(Vector3 center, float angleDeg, float scale, HexCalculation hexCalculation)
        {
            float angleRed = angleDeg * Mathf.Deg2Rad;

            return new Vector3(center.x + scale * Mathf.Cos(angleRed), center.y + scale * offsetY * Mathf.Sin(angleRed), center.z);

        }
        /// <summary>
        ///Get single dimention array index
        /// </summary>
        public static int GetIndex(int x, int y, int rows)
        {
            return x + y * rows;
        }
        public static int GetIndex(Vector2Int position, int rows)
        {
            return position.x + position.y * rows;
        }
        private void SetUpNeighbours(ref HexagonBlock[] blocks, Vector2Int dimentions)
        {
            //up1 = (x-1,y) ;up2 = (x-1,y-1)
            for(int i =0;i<blocks.Length;i++)
            {
                Vector2Int up1 = new Vector2Int(blocks[i].gridPosition.x - 1, blocks[i].gridPosition.y);
                int n1 = -1;
                int n2 = -1;
                if (IsValide(up1, blocks.Length))
                {
                    int up1Index = GetIndex(up1, dimentions.x);
                    n1 = GetValideNeighbour(ref blocks, up1Index);
                }

                Vector2Int up2 = new Vector2Int(blocks[i].gridPosition.x - 1, blocks[i].gridPosition.y - 1);
                if (IsValide(up2, blocks.Length))
                {
                    int up2Index = GetIndex(up2, dimentions.x);
                    n2 = GetValideNeighbour(ref blocks, up2Index);
                }

                blocks[i].SetUpNeighbours(n1, n2);
            }

        }
        private int GetValideNeighbour(ref HexagonBlock[] blocks, int neighbour)
        {
            if (neighbour < 0 || neighbour >= blocks.Length) return -1;

            return neighbour;
        }
        private bool IsValide(Vector2Int position, int length)
        {
            if (position.x < 0 || position.y < 0) return false;
            if (position.x > length || position.y > length) return false;
            if ((position.x * position.y) >= length) return false;
            return true;
        }

    }
}
