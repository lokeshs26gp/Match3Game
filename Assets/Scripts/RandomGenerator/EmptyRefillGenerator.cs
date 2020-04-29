
using UnityEngine;
using HexagonSystem;
using DataSO;
using System.Collections.Generic;

namespace RandomGenerationSystem
{
    public class EmptyRefillGenerator
    {
        private HexagonBlock[] hexagonBlocks;
        private RandomGenerator randonGeneratorcached;
        private HashSet<int> movingHashset;
        private float movementSpeed;
        public EmptyRefillGenerator(ref HexagonBlock[] blocks, RandomGenerator randomGenerator,float speed)
        {
            hexagonBlocks           =  blocks;
            randonGeneratorcached   = randomGenerator;
            movementSpeed           = speed;

            movingHashset = new HashSet<int>();
            InputSystem.InputDataProcessor.OnSelectionSuccessEvent += OnSelectionSuccess;
        }
        public void Dispose()
        {
            InputSystem.InputDataProcessor.OnSelectionSuccessEvent -= OnSelectionSuccess;
        }
        private void OnSelectionSuccess(int[] SuccessList)
        {
            for (int i = 0; i < SuccessList.Length; i++)
                GetUpSideNeighbour(SuccessList[i], ref hexagonBlocks);

        }

        public void UpdateLoop(float delta)
        {
            if (movingHashset.Count <= 0) return;

            for (int i = 0; i < hexagonBlocks.Length; i++)
            {
                GetUpSideNeighbour(i, ref hexagonBlocks);
                if(movingHashset.Contains(i))
                {
                    if (hexagonBlocks[i].IsInPosition)
                        movingHashset.Remove(i);
                }
            }
            float speed = delta * movementSpeed;
            foreach (int n in movingHashset)
            {
               hexagonBlocks[n].SetBlockMovement(speed);
            }

            //Debug.Log("EmptyfillRunning");
        }
        /// <summary>
        /// By checking neighbours filling all blocks recursivly
        /// </summary>
        /// <param name="block"></param>
        private void GetUpSideNeighbour(int index , ref HexagonBlock[] blocks)
        {
            int getOneNeighbour = GetOneUpNeighbour(index, ref blocks);
            if(getOneNeighbour ==-1)
            {
                if (!blocks[index].IsBlockFilled)
                {
                    int arrayIndex = -1;
                    BlockType block = randonGeneratorcached.GetRandomSingle(out arrayIndex);
                    blocks[index].SetBlock(block, arrayIndex);
                }
            }
            else
            {
                if (blocks[getOneNeighbour].IsBlockFilled && !blocks[index].IsBlockFilled)
                {
                    blocks[index].SetBlock(randonGeneratorcached.blockTypes, ref blocks[getOneNeighbour]);
                    movingHashset.Add(index);
                    GetUpSideNeighbour(getOneNeighbour, ref blocks);
                    
                }
                else
                    GetUpSideNeighbour(getOneNeighbour, ref blocks);

            }

        }
       
        /// <summary>
        /// Getting neighbours from top/up side from  RandomEmptyFillGenerator
        /// </summary>
        public int GetOneUpNeighbour(int index ,ref HexagonBlock[] blocks)
        {
            int n1 = blocks[index].GetNeighbour1;
            int n2 = blocks[index].GetNeighbour2;
            if (n1 != -1 && n2 != -1)
            {
                if (blocks[n1].IsBlockFilled && blocks[n2].IsBlockFilled)
                    return Random.Range(0, 1) < 0.5f ? n1 : n2;
                else if (blocks[n1].IsBlockFilled)
                    return n1;
                else if (blocks[n2].IsBlockFilled)
                    return n2;
                else return Random.Range(0, 1) < 0.5f ? n1 : n2;
            }
            else if (n1 != -1)
                return n1;
            else if (n2 != -1)
                return n2;
            else
                return -1;
        }
    }
}
