using System;
using BadgesSharp.Builders;
using NUnit.Framework;
using TestSharp;

namespace BadgesSharp.UnitTests.Builders
{
    [TestFixture]
    [Category("LOC")]
    public class LocBadgeBuilderTest
    {
        [Test]
        public void Build_NotSourceMonitorReport_Svg()
        {
            var target = new LocBadgeBuilder(ResourceHelper.GetResource("Loc-not-SourceMonitor-report.xml"));

            ExceptionAssert.IsThrowing(new InvalidOperationException("LOC report file format unsupported. The supported formats are: SourceMonitor"), () =>
            {
                target.Build();
            });            
        }
       
        [Test]
        public void Build_SourceMonitor_Svg()
        {
            var target = new LocBadgeBuilder(ResourceHelper.GetResource("Loc-SourceMonitor-report.xml"));
            var actual = target.Build();
            Assert.AreEqual(ResourceHelper.GetResource("Loc.svg"), actual);
        }
    }
}
