using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("StyleCop")]
    public class StyleCopBadgeBuilderTest
    {
        [Test]
        public void Build_NoViolations_Svg()
        {
            var target = new StyleCopBadgeBuilder(ResourceHelper.GetResource("StyleCop-report-no-violations.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("StyleCop-no-violations.svg"), actual);
        }

        [Test]
        public void Build_WithViolations_Svg()
        {
            var target = new StyleCopBadgeBuilder(ResourceHelper.GetResource("StyleCop-report-with-violations.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("StyleCop-with-violations.svg"), actual);
        }
    }
}
