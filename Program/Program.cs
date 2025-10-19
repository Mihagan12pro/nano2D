using Teigha.Runtime;

namespace Program
{
    public static class Program
    {
        [CommandMethod("TrimAndJoin")]
        public static void TrimAndJoin()
        {
            using (GeometryJoiner.TrimJoiner joiner = new GeometryJoiner.TrimJoiner())
            {
                joiner.TrimAndJoin();
            }
        }
    }
}
