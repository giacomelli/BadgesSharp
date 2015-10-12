using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("SpecFlow")]
    public class SpecFlowBadgeBuilderTest
    {
        [Test]
        public void Build_Success_Svg()
        {
            var target = new SpecFlowBadgeBuilder(BadgeStatus.Success);
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("SpecFlow-success.svg"), actual);
        }

        [Test]
        public void Build_Failed_Svg()
        {
            var target = new SpecFlowBadgeBuilder(BadgeStatus.Failed);
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("SpecFlow-failed.svg"), actual);
        }
    }
}
