using GeometryJoiner;
using Teigha.DatabaseServices;
using HostMgd;
using Teigha.Runtime;

namespace Test_nano2D
{
    public class TestBaseUtilitie
    {
        [Fact, CommandMethod("TestInputEntity")]

        public void CheckInputEntity()
        {
            TrimJoiner trimJoiner = new TrimJoiner();

            Line line = new Line();

            bool result = trimJoiner.CheckInputEntity(line, typeof(Curve));

            Assert.True(result);
        }
    }
}