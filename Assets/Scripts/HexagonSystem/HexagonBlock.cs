
using UnityEngine;
namespace HexagonSystem
{
    using DataSO;
    public struct HexagonBlock 
    {
        public static Vector2 BlockDifference;

        public Vector2Int gridPosition;//Remove this if not required
        public SpriteRenderer blockSpritecomp;
        public SpriteRenderer blockHighlightref;
       
        private Vector3 position;
        private bool isFilled;
        private Color selectionColor;
        private int n1, n2;//upside neighbour1,2
        private int blockTypeIndex;
        public HexagonBlock(Vector2Int gridPos, Vector3 worldPosition,HexagonViewTempInfo viewInfo)
        {
            gridPosition        = gridPos;
            position            = worldPosition;
            blockSpritecomp     = viewInfo.blockSprite;
            blockHighlightref   = viewInfo.highlightSprite;
            isFilled            = false;
            n1                  = -1;
            n2                  = -1;
            blockTypeIndex      = -1;//Not safe
            blockHighlightref.color = selectionColor = Color.clear;
        }
        public void SetBlock(BlockType blockType,int pblocktypeIndex)
        {
            if(blockType == null)
            {
                isFilled = false;
                blockSpritecomp.sprite = null;
                blockSpritecomp.color = blockHighlightref.color = selectionColor = Color.clear;
                blockTypeIndex = -1;
            }
            else
            {
                isFilled = true;
                blockSpritecomp.sprite = blockType.sprite;
                blockSpritecomp.color = Color.white;
                selectionColor = blockType.selectionColor;
                //selectionColor.a = 0.5f;
                this.blockTypeIndex = pblocktypeIndex;
            }
        }
        public void SetBlock(BlockType[] blockTypes,ref HexagonBlock block)
        {
            SetBlock(blockTypes[block.BlockTypeIndex], block.BlockTypeIndex);
            block.SetBlock(null,-1);
            blockSpritecomp.transform.position = block.GetWorldPosition;
        }
        /// <summary>
        /// Set neighbours from HexagonGridGenerator
        /// </summary>
        public void SetUpNeighbours(int up1, int up2)
        {
            n1 = up1;
            n2 = up2;
        }
        /// <summary>
        /// Set highlight from InputDataProcessor through IHexagonBlock interface
        /// </summary>
        public void SetHighLight(Color color)
        {
            if (isFilled)
                blockHighlightref.color = color;
        }
        /// <summary>
        ///  set neighbours moving effect from  RandomEmptyFillGenerator
        /// </summary>
        public void SetBlockMovement(float delta)
        {
            blockSpritecomp.transform.position = Vector3.MoveTowards(blockSpritecomp.transform.position, position, delta);
           // return blockSpritecomp.transform.position.Equals(position);
        }
        public bool IsInPosition { get { return blockSpritecomp.transform.position.Equals(position); } }
        public bool IsBlockSame(ref HexagonBlock block)
        {
            return block.blockSpritecomp.sprite == blockSpritecomp.sprite;
        }
        public Color GetSelectionColor { get { return selectionColor; } }
        public Vector3 GetWorldPosition { get { return position; } }
        public bool IsBlockFilled { get { return isFilled; } }
        public int GetNeighbour1 { get { return n1; } }
        public int GetNeighbour2 { get { return n2; } }
        public int BlockTypeIndex { get { return blockTypeIndex; } }

    }
}
