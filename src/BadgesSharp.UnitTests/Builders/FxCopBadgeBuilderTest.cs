using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("FxCop")]
    public class FxCopBadgeBuilderTest
    {
        [Test]
        public void Build_NoViolations_Svg()
        {
            var target = new FxCopBadgeBuilder(ResourceHelper.GetResource("FxCop-report-no-violations.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("FxCop-no-violations.svg"), actual);
        }

        [Test]
        public void Build_WithViolations_Svg()
        {
            var target = new FxCopBadgeBuilder(ResourceHelper.GetResource("FxCop-report-with-violations.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("FxCop-with-violations.svg"), actual);
        }
    }
}
