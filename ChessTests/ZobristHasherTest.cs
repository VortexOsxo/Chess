using ChessCore.AI.Evaluation;
using ChessCore.GameContext;
using ChessCore.Moves;

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
        public void HashingShouldBeDifferentAfterMoves()
        {
            var state = new State();
            
            var hashing1 = hasher.Hash(state.board);

            var move = MoveHelper.CreateMove(52, 36);
            MoveHelper.ExecuteMove(state, move);
            
            var hashing2 = hasher.Hash(state.board);
            
            Assert.AreNotEqual(hashing1, hashing2);
        }

        [TestMethod]
        public void UpdateHashShouldBeTheSameAsRehashing()
        {
            var state = new State();
            
            var hashing1 = hasher.Hash(state.board);
            
            var move = MoveHelper.CreateMove(52, 36);
            MoveHelper.ExecuteMove(state, move);
            
            var hashing2 = hasher.Hash(state.board);
            var hashing3 = hasher.UpdateHash(state.board, state.whiteToPlay, hashing1, move);
            
            Assert.AreEqual(hashing2, hashing3);
        }
    }
}