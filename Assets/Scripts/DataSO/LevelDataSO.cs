using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataSO
{
    [System.Serializable]
    public class BlockType
    {
        public string name;
        public Sprite sprite;
        public Color selectionColor;// = Color.white;

        public static BlockType[] GetRandom(int count, List<BlockType> blockTypes)
        {
            BlockType[] indexs = new BlockType[count];
            HashSet<BlockType> randomBlocks = new HashSet<BlockType>();
            while (randomBlocks.Count < count)
            {
                BlockType rand = blockTypes[UnityEngine.Random.Range(0, blockTypes.Count-1)];
                if (!randomBlocks.Contains(rand))
                    randomBlocks.Add(rand);
            }
            randomBlocks.CopyTo(indexs);
            return indexs;
        }
    }
    /// <summary>
    /// Level generation data to generate grid and gameplay design elements
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData")]
    public class LevelDataSO : ScriptableObject
    {
        public GameObject hexagonPrefab;
        public float uniformBlockscale = 1.0f;
        public Vector2Int gridDimentions;
        [Header("Select between blocktypes count")]
        public int levelMaxblocktypes;
        [Range(0.1f,10.0f)]
        public float movementSpeed = 1.0f;

        [SerializeField]
        private List<BlockType> blockTypelist = new List<BlockType>();

        private BlockType[] levelBlocktypes;

        public BlockType[] LevelBlocktypes
        {
            get
            {
                if (levelBlocktypes == null)
                    levelBlocktypes = BlockType.GetRandom(levelMaxblocktypes, blockTypelist);

                return levelBlocktypes;
            }
        }
       
        private void OnEnable()
        {
            if (levelBlocktypes != null) return;

            levelBlocktypes = BlockType.GetRandom(levelMaxblocktypes, blockTypelist);
        }

       /* [ContextMenu("CopyTo")]
        public void CopyTo()
        {
            for(int i=0; i<blockTypes.Count;i++)
            {
                blockTypelist.Add(new BlockType 
                { name = blockTypes[i].name,sprite = blockTypes[i].sprite,selectionColor = blockTypes[i].selectionColor });
            }
        }
        */

    }
}
