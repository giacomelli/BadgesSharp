using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    public class BuilderServiceTest
    {
        [Test]
        public void ExistsBuilder_NoBuilderForBadgeName_False()
        {
            Assert.IsFalse(BuilderService.ExistsBuilder("invalid badge"));
        }

        [Test]
        public void ExistsBuilder_BuilderForBadgeName_True()
        {
            Assert.IsTrue(BuilderService.ExistsBuilder("FxCop"));
            Assert.IsTrue(BuilderService.ExistsBuilder("StyleCop"));
            Assert.IsTrue(BuilderService.ExistsBuilder("DupFinder"));
            Assert.IsTrue(BuilderService.ExistsBuilder("SpecFlow"));
        }
    }
}
