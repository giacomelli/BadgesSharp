#if DEBUG
using System;
using System.IO;
using System.Net;
using HelperSharp;
using NUnit.Framework;
using TestSharp;

namespace BadgesSharpCmd.FunctionalTests
{
    [TestFixture]
    [Category("BadgesSharpCmd")]
    public class ProgramTest
    {
        #region Fields
        private string m_exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BadgesSharpCmd.exe");
        private string m_accessToken = BadgesSharp.Infrastructure.ConfigHelper.GitHubAccessToken;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            WebHostHelper.StopByPort(8888);
            WebHostHelper.StartAndWaitForResponse("BadgesSharp.WebApi", 8888);
        }

        [TestFixtureTearDown]
        public void DownFixture()
        {
            WebHostHelper.StopByPort(8888);
        }
        #endregion

        #region Tests
        [Test]
        public void BadgesSharpCmd_NoArgs_Help()
        {
            var output = ProcessHelper.Run(m_exePath, "", true);
            StringAssert.Contains("Try `BadgesSharpCmd --help` for more information", output);

            output = ProcessHelper.Run(m_exePath, "-help", true);
            StringAssert.Contains(" -h, --help                 show this message and exit", output);
        }

        [Test]
        public void BadgesSharpCmd_Loc_BadgeGenerated()
        {
            AssertBadgeGenerated("LOC", @"..\..\..\BadgesSharp.UnitTests\Resources\Loc-SourceMonitor-report.xml");
        }

        [Test]
        public void BadgesSharpCmd_StyleCop_BadgeGenerated()
        {
            AssertBadgeGenerated("StyleCop", @"..\..\..\BadgesSharp.UnitTests\Resources\StyleCop-report-with-violations.xml");
        }

        [Test]
        public void BadgesSharpCmd_SpecFlow_BadgeGenerated()
        {
            AssertBadgeGenerated("SpecFlow", "success");
        }

        [Test]
        public void BadgesSharpCmd_FxCop_BadgeGenerated()
        {
            AssertBadgeGenerated("FxCop", @"..\..\..\BadgesSharp.Uni*Tests\Resou*ces\FxCop-report-with-*.xml");
            AssertBadgeGenerated("FxCop", @"..\..\..\BadgesSharp.UnitTests\Resources\FxCop-report-with-violations.xml");
        }

        [Test]
        public void BadgesSharpCmd_DupFinder_BadgeGenerated()
        {
            AssertBadgeGenerated("DupFinder", @"..\..\..\BadgesSharp.UnitTests\Resources\*.xml");
            AssertBadgeGenerated("DupFinder", @"..\..\..\BadgesSharp.UnitTests\Resources\dupFinder-report-with-duplicates.xml");
        }

        [Test]
        public void BadgesSharpCmd_CodeCoverage_BadgeGenerated()
        {
            AssertBadgeGenerated("Code coverage", @"..\..\..\BadgesSharp.UnitTests\Resources\CodeCoverage-DotCover-report-success.xml");
        }

        [Test]
        public void BadgesSharpCmd_ContentFileNotExists_BadgeGenerated()
        {
            AssertBadgeGenerated("FxCop", @"file.not.exists.xml", false);
        }

        [Test]
        public void BadgesSharpCmd_MissingRequiredArgs_MissingRequiredMessage()
        {
            var output = ProcessHelper.Run(
                m_exePath,
                "-r SampleProject -b \"{0}\" -c \"{1}\" -a {2}".With("StyleCop", @"..\..\..\BadgesSharp.UnitTests\Resources\StyleCop-report-with-violations.xml", "invalidtoken"), true);

            StringAssert.Contains("Missing required arguments", output);
        }

        [Test]
        public void BadgesSharpCmd_InvalidGitHubRepository_BadgeNotGenerated()
        {
            var output = ProcessHelper.Run(
                m_exePath,
                "-o giacomelli -r SampleProject -b \"{0}\" -c \"{1}\" -a {2}".With("StyleCop", @"..\..\..\BadgesSharp.UnitTests\Resources\StyleCop-report-with-violations.xml", "invalidtoken"), true);

            StringAssert.Contains("Your personal access token seems be invalid", output);
        }

        [Test]
        public void BadgesSharpCmd_InvalidBadge_BadgeGenerated()
        {
            var output = CallCommandLine("PolicialDeEstilo", @"..\..\..\BadgesSharp.UnitTests\Resources\StyleCop-report-with-violations.xml");

            StringAssert.Contains("here is no endpoint to badge 'PolicialDeEstilo'", output);
        }
        #endregion

        #region Helpers
        private void AssertBadgeGenerated(string badgeName, string content, bool contentFileShouldExists = true)
        {
            var normalizedBadgeName = badgeName.Replace(" ", string.Empty);
            string output = CallCommandLine(normalizedBadgeName, content);

            StringAssert.Contains("Badge generated", output, "Badge '{0}' not generated".With(badgeName));
            var badgeUrl = "http://localhost:8888/badges/giacomelli/SampleProject/" + normalizedBadgeName;

            StringAssert.Contains(badgeUrl, output, "Badge '{0}' not generated".With(badgeName));

            if (content.Contains("*"))
            {
                StringAssert.Contains("Using wildcards to find content file. The first file found will be used.", output);
            }

            if (contentFileShouldExists)
            {
                StringAssert.DoesNotContain("WARNING: Content file ", output, "Content file not found to badge '{0}'".With(badgeName));
            }

            var client = new WebClient();
            var actual = client.DownloadString(badgeUrl);
            StringAssert.StartsWith("<svg xmlns", actual);
            StringAssert.Contains(badgeName, actual);
            StringAssert.EndsWith("</svg>", actual);
        }

        private string CallCommandLine(string badgeName, string content)
        {
            return ProcessHelper.Run(
                m_exePath,
                "-o giacomelli -r SampleProject -b \"{0}\" -c \"{1}\" -a {2}".With(badgeName, content, m_accessToken), true);
        }
        #endregion
    }
}
#endif