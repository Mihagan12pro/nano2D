using GeometryJoiner;
using Teigha.Runtime;

namespace Program
{
    public static class Program
    {
        [CommandMethod("TrimAndJoin")]
        public static void TrimAndJoin()
        {
            using (TrimJoiner joiner = new TrimJoiner())
            {
                joiner.TrimAndJoin();
            }
        }
    }
}
