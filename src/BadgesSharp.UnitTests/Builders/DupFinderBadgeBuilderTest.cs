using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("DupFinder")]
    public class DupFinderBadgeBuilderTest
    {
        [Test]
        public void Build_NoDuplicates_Svg()
        {
            var target = new DupFinderBadgeBuilder(ResourceHelper.GetResource("dupFinder-report-no-duplicates.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("dupFinder-no-duplicates.svg"), actual);
        }

        [Test]
        public void Build_WithDuplicates_Svg()
        {
            var target = new DupFinderBadgeBuilder(ResourceHelper.GetResource("dupFinder-report-with-duplicates.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("dupFinder-with-duplicates.svg"), actual);
        }
    }
}
