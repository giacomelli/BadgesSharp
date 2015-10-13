using System;
using BadgesSharp.Builders;
using NUnit.Framework;
using TestSharp;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("CodeCoverage")]
    public class CodeCoverageBadgeBuilderTest
    {
        [Test]
        public void Build_NotDotCoverReport_Svg()
        {
            var target = new CodeCoverageBadgeBuilder(ResourceHelper.GetResource("CodeCoverage-not-DotCover-report.xml"));

            ExceptionAssert.IsThrowing(new InvalidOperationException("Code coverage report file format unsupported. The supported formats are: DotCover"), () =>
            {
                target.Build();
            });            
        }

        [Test]
        public void Build_DotCoverFailedStatus_Svg()
        {
            var target = new CodeCoverageBadgeBuilder(ResourceHelper.GetResource("CodeCoverage-DotCover-report-failed.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("CodeCoverage-failed.svg"), actual);
        }

        [Test]
        public void Build_DotCoverWarningStatus_Svg()
        {
            var target = new CodeCoverageBadgeBuilder(ResourceHelper.GetResource("CodeCoverage-DotCover-report-warning.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("CodeCoverage-warning.svg"), actual);
        }

        [Test]
        public void Build_DotCoverSuccessStatus_Svg()
        {
            var target = new CodeCoverageBadgeBuilder(ResourceHelper.GetResource("CodeCoverage-DotCover-report-success.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("CodeCoverage-success.svg"), actual);
        }
    }
}
