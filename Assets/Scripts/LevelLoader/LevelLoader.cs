using UnityEngine;

namespace LevelLoaderSystem
{
    using DataSO;
    using HexagonSystem;
    using InputSystem;
    using RandomGenerationSystem;

    public class LevelLoader : MonoBehaviour
    {
        #region public Inspector Reference
        [Header("Level Scriptable Object")]
        public LevelDataSO LevelData;
        [Space(2)]
        [Header("----------------------------------------------------------------")]
        public Camera mainCamera;
        [Header("Grid Camera position offset")]
        public Vector3 gridCameraOffset;
        #endregion
        #region singleton
        private static LevelLoader _instance = null;

        public static LevelLoader Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);

        }
        #endregion

        #region private objects
        private HexagonGridGenerator HexagonGridGenerator;
        private HexagonBlock[] blocks;
        private RandomGenerator randomGenerator;
        private ColliderGenerator colliderGenerator;
        private UnityContext context;
        private InputReader inputReader;
        private Bounds gridBounds = default;
        private InputDataProcessor inputDataProcessor;
        private EmptyRefillGenerator emptyRefillgenerator;
        #endregion
        private void Start()
        {
           //Level Generator
            HexagonGridGenerator = new HexagonGridGenerator(LevelData, ref blocks,ref gridBounds);
            randomGenerator = new RandomGenerator(RandomAlgorithmType.JustRandom, LevelData.LevelBlocktypes);
            randomGenerator.GenerateRandom(blocks);

            //Making camera center based on grid position
            mainCamera.transform.position = gridBounds.center + gridCameraOffset;

            ///Input Systems
            colliderGenerator    = new ColliderGenerator(gridBounds);
            inputReader          = new InputReader(mainCamera);
            inputDataProcessor   = new InputDataProcessor(ref blocks);
            emptyRefillgenerator = new EmptyRefillGenerator(ref blocks, randomGenerator,LevelData.movementSpeed);

            //Update Loop UnityContext
            GameObject @object = new GameObject("UnityContext");
            context = @object.AddComponent<UnityContext>();
            context.AddListener(inputReader.UpdateLoop);
            context.AddListener(emptyRefillgenerator.UpdateLoop);
        }

        private void OnDestroy()
        {
            inputDataProcessor.Dispose();
            emptyRefillgenerator.Dispose();
        }


    }
}
