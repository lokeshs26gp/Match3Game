using UnityEngine;
using DataSO;
using HexagonSystem;
namespace RandomGenerationSystem
{
    /// <summary>
    /// Responsible for generating random colors and set to blocks
    /// </summary>
    public class SimpleRandomAlgorithm : IRandomGenerator
    {
        private BlockType[] blocktypes;
        public SimpleRandomAlgorithm(BlockType[] blockTypes)
        {
            blocktypes = blockTypes;
        }

        public void SetRandom( HexagonBlock[] blocks)
        {
            for(int i =0;i< blocks.Length;i++)
            {
                int index = GetRandom();
                blocks[i].SetBlock(blocktypes[index], index);
            }
           
        }

        public int GetRandom()
        {
            return Random.Range(0, blocktypes.Length - 1);
        }

        public BlockType GetBlock(int index)
        {
            return blocktypes[index];
        }
    }

}