
using HexagonSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    /// <summary>
    /// Responsible for processing raw input data to match3 game input
    /// </summary>
    public class InputDataProcessor
    {
        public static readonly float offsetLessthan = 0.2f;

        public static Action<int[]> OnSelectionSuccessEvent;

        private HexagonBlock[] hexagonBlocks;
       
        private Stack<int> currentSelectedStack;
        private Color currentSelectedcolor;
        private List<int> SelectionList;
        public InputDataProcessor(ref HexagonBlock[] blocks)
        {
            hexagonBlocks = blocks;
            InputReader.OnClickEvent += ProcessRawInputData;
            currentSelectedStack = new Stack<int>();
            SelectionList = new List<int>(3);
            InputReader.OnClickEvent += ProcessRawInputData;
        }
        public void Dispose()
        {
            InputReader.OnClickEvent -= ProcessRawInputData;
        }
        public void ProcessRawInputData(RaycastHit hitinfo,MouseButtonState buttonState, bool notHit)
        {
            if (notHit)
            {
                CancelAllSelected();
                return;
            }

            for(int i=0;i<hexagonBlocks.Length;i++)
            {
                Vector3 difference = hexagonBlocks[i].GetWorldPosition - hitinfo.point;
                if(difference.sqrMagnitude< offsetLessthan)
                {
                    switch (buttonState)
                    {
                        case MouseButtonState.Down:
                            CancelAllSelected();
                            currentSelectedcolor = hexagonBlocks[i].GetSelectionColor;
                            currentSelectedStack.Push(i);
                            goto default;//Make highlight from default
                        case MouseButtonState.Drag:
                            if (currentSelectedStack.Count <= 0)//if mouse down didnt selected any hexagon then dont process drag
                                break;
                            else if (!currentSelectedStack.Contains(i))//if already selected block dont push again 
                            {

                                if (hexagonBlocks[i].IsBlockSame(ref hexagonBlocks[currentSelectedStack.Peek()]))
                                {
                                    currentSelectedStack.Push(i);
                                    goto default;
                                }
                                else
                                    CancelAllSelected();//if selecting block is not from previous selected then cancel all selection
                                break;
                            }
                            else
                            {
                                if (currentSelectedStack.Peek() != i)//back dehighlighting if selected block selecting by backwords 
                                {
                                    hexagonBlocks[currentSelectedStack.Pop()].SetHighLight(Color.clear);
                                }
                                break;
                            }
                        case MouseButtonState.Up:
                            {
                                SelectionSuccess();
                                
                                break;
                            }
                        default:
                            hexagonBlocks[i].SetHighLight(currentSelectedcolor);
                            break;
                    }
                    break;
                }
            }

        }

        /// <summary>
        /// selection success processing for atleast match 3
        /// </summary>
        private void SelectionSuccess()
        {
            
            if (currentSelectedStack.Count >= 3)
            {
               SelectionList.Clear();
                while (currentSelectedStack.Count > 0)
                {
                    int pop = currentSelectedStack.Pop();
                    hexagonBlocks[pop].SetHighLight(Color.clear);
                    hexagonBlocks[pop].SetBlock(null,-1);
                    SelectionList.Add(pop);
                }
                OnSelectionSuccessEvent(SelectionList.ToArray());
            }
            else
                CancelAllSelected();

        }

        /// <summary>
        /// Clear all selection from stack
        /// </summary>
        private void CancelAllSelected()
        {
            while (currentSelectedStack.Count > 0)
            {
                int pop = currentSelectedStack.Pop();
                hexagonBlocks[pop].SetHighLight(Color.clear);
            }
            currentSelectedStack.Clear();//just to reconform not require here
        }

    }
}

