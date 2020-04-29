
using DataSO;
using HexagonSystem;

namespace RandomGenerationSystem
{
    /// <summary>
    ///we can develop different types of random generation system by injecting through interface
    /// </summary>
    public interface IRandomGenerator
    {
        void SetRandom(HexagonBlock[] blocks);

        int GetRandom();

        BlockType GetBlock(int index);
    }

}
