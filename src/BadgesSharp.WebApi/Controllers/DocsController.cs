using System.Web.Mvc;
using BadgesSharp.WebApi.Models;

namespace BadgesSharp.WebApi.Controllers
{
    public class DocsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GettingStarted()
        {
            return View();
        }

        public ActionResult DupFinder()
        {
            var model = new BadgePageViewModel()
            {
                Description = "Code duplication finder built by JetBrains",
                Url = "https://confluence.jetbrains.com/display/NETCOM/Introducing+dupFinder",
                Report = "dupFinder-report.xml",
                TeamCityReport = @"%system.teamcity.build.tempDir%\dupfinder-report-*.xml",
                RunCommand = "C:\\ProgramData\\chocolatey\\lib\\resharper-clt.portable\\tools\\dupfinder.exe /output=dupFinder-report.xml /show-text /exclude=**\\*Test.cs; [Your solution file].sln\n"
            };

            return View(model);
        }

        public ActionResult FxCop()
        {
            var model = new BadgePageViewModel()
            {
                Description = "FxCop is a free static code analysis tool from Microsoft that checks .NET managed code assemblies for conformance to Microsoft's .NET Framework Design",
                Url = "https://www.microsoft.com/en-us/download/details.aspx?id=6544",
                Report = "fxcop-report.xml",
                TeamCityReport = @"%system.teamcity.build.tempDir%\fxcop-output-*\fxcop-result.xml",
                RunCommand = "\"C:\\Program Files (x86)\\Microsoft Fxcop 10.0\\FxCopCmd.exe\" /project:[Your FxCop file].FxCop /out:fxcop-report.xml"
            };

            return View(model);
        }

        public ActionResult StyleCop()
        {
            var model = new BadgePageViewModel()
            {
                Description = "StyleCop analyzes C# source code to enforce a set of style and consistency rules",
                Url = "https://stylecop.codeplex.com",
                Report = "stylecop-report.xml",
                TeamCityReport = @"%system.teamcity.build.tempDir%\Stylecop-output-*\stylecop-result.xml",
                RunCommand = "StyleCopCmd\\Net.SF.StyleCopCmd.Console\\StyleCopCmd.exe -sf .[Your Solution file].sln -of stylecop-report.xml"
            };
            
            return View(model);
        }

        public ActionResult PlatoMaintainability()
        {            
            var model = new BadgePageViewModel()
            {
                Description = "JavaScript source code visualization, static analysis, and complexity tool",
                Url = "https://github.com/es-analysis/plato",
                Report = "report.json",
                RunCommand = "See how config Plato on your project: https://github.com/es-analysis/plato"
            };

            return View(model);
        }
    }
}