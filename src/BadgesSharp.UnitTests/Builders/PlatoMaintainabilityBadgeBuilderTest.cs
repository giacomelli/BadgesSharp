using BadgesSharp.Builders;
using NUnit.Framework;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("Plato")]
    public class PlatoMaintainabilityBadgeBuilderTest
    {
        [Test]
        public void Build_FailedStatus_Svg()
        {
            var target = new PlatoMaintainabilityBadgeBuilder(ResourceHelper.GetResource("PlatoMaintainability-report-failed.json"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("PlatoMaintainability-failed.svg"), actual);
        }

        [Test]
        public void Build_WarningStatus_Svg()
        {
            var target = new PlatoMaintainabilityBadgeBuilder(ResourceHelper.GetResource("PlatoMaintainability-report-warning.json"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("PlatoMaintainability-warning.svg"), actual);
        }

        [Test]
        public void Build_SuccessStatus_Svg()
        {
            var target = new PlatoMaintainabilityBadgeBuilder(ResourceHelper.GetResource("PlatoMaintainability-report-success.json"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("PlatoMaintainability-success.svg"), actual);
        }
    }
}
