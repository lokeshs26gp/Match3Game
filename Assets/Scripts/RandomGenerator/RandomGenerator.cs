
using HexagonSystem;
using DataSO;
namespace RandomGenerationSystem
{
    public enum RandomAlgorithmType
    {
        JustRandom,
        FormulaBased
    }
    /// <summary>
    /// Responsible for generating random blocks by different algorithms
    /// </summary>
    public class RandomGenerator
    {
        private IRandomGenerator randomGenerator;
        public BlockType[] blockTypes;
        public RandomGenerator(RandomAlgorithmType randomAlgorithmType,BlockType[] blockTypes)
        {
            switch (randomAlgorithmType)
            {
                case RandomAlgorithmType.JustRandom:
                    randomGenerator = new SimpleRandomAlgorithm(this.blockTypes = blockTypes);
                    break;
                case RandomAlgorithmType.FormulaBased:
                    ///TODo: need to implement later
                    break;
            }
        }

        public void GenerateRandom(HexagonBlock[] blocks)
        {
            randomGenerator.SetRandom(blocks);
        }
        /// <summary>
        /// filling empty blocks when selection successfull
        /// </summary>
        /// <returns></returns>
        public BlockType GetRandomSingle(out int index)
        {
            index = randomGenerator.GetRandom();
            return blockTypes[index];
        }

        

    }
}