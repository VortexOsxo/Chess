using ChessCore.AI.Evaluation;

namespace ChessTests
{
    [TestClass]
    public class ZobristHasherTest
    {
        private static ZobristHasher? hasher;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            hasher = new ZobristHasher();
            hasher.Initialize();
        }

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}