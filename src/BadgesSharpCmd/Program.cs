using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using BadgesSharp.Infrastructure;
using HelperSharp;
using Mono.Options;
using RestSharp;

namespace BadgesSharpCmd
{
    /// <summary>
    /// BadgesSharp command
    /// </summary>
    public static class Program
    {
        #region Fields
        private static string s_webApiUrl;
        private static string s_repositoryOwner = null;
        private static string s_repositoryName = null;
        private static string s_badge = null;
        private static string s_accessToken = null;
        private static string s_status = null;
        private static string s_content = null;
        private static bool s_showHelp = false;
        #endregion

        #region Methods
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Justification = "From configuration.")]
        public static void Main(string[] args)
        {
            s_webApiUrl = ConfigHelper.WebApiUrl;
            var version = typeof(Program).Assembly.GetName().Version.ToString();

            ShowHeader(s_webApiUrl, version);

            if (args.Length == 0)
            {
                Show("Try `BadgesSharpCmd --help` for more information");
                return;
            }

            var options = BuildOptions();

            if (!ParseArguments(options, args))
            {
                return;
            }

            ValidateApiResponse(CallApi());
        }

        private static void ShowHeader(string webApiUrl, string version)
        {
            NewLine();
            Show("BadgesSharpCmd {0} by Diego Giacomelli (@ogiacomelli)", version);
            Show(webApiUrl);
            NewLine();
        }

        private static OptionSet BuildOptions()
        {
            return new OptionSet()
            {
                "Usage: ",
                "   BadgesSharpCmd -o <GitHub repository owner> -r <GitHub repository name> -a <GitHub personal access token> -b <badge name> -c <badge content>",
                string.Empty,
                "Options:",
                {
                    "o|repository-owner=", "the GitHub repository owner", o => s_repositoryOwner = o
                },
                {
                    "r|repository-name=", "the GitHub repository name", r => s_repositoryName = r
                },
                {
                    "a|access-token=", "the GitHub personal access token", a => s_accessToken = a
                },
                {
                    "b|badge=", "the badge", b => s_badge = b
                },
                {
                    "c|content=", "the content to generate the badge. DupFinder, FxCop and StyleCop use the output result xml report as content.", c => s_content = c
                },
                {
                    "h|help", "show this message and exit", v => s_showHelp = v != null
                },
                string.Empty,
                string.Empty,
                "Samples:",
                "BadgesSharpCmd -o giacomelli -r BadgesSharp -a <GitHub personal access token> -b FxCop -c \"tools\\fxcop-report.xml\"",
                string.Empty,
                "BadgesSharpCmd -o giacomelli -r BadgesSharp -a <GitHub personal access token> -b StyleCop -c \"stylecop-report.violations.xml\""
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All errors should be write to console.")]
        private static bool ParseArguments(OptionSet optionsSet, string[] args)
        {
            try
            {
                try
                {
                    optionsSet.Parse(args);
                }
                catch (OptionException e)
                {
                    Console.Write("Argument parsing error: ");
                    Show(e.Message);
                    Show("Try `BadgesSharpCmd --help` for more information");
                    return false;
                }

                if (s_showHelp)
                {
                    optionsSet.WriteOptionDescriptions(Console.Out);
                    return false;
                }

                if (string.IsNullOrEmpty(s_repositoryOwner)
                    || string.IsNullOrEmpty(s_repositoryName)
                    || string.IsNullOrEmpty(s_accessToken)
                    || string.IsNullOrEmpty(s_badge)
                    || string.IsNullOrEmpty(s_content))
                {
                    Show("Missing required arguments.");
                    Show("Try `BadgesSharpCmd --help` for more information");
                    return false;
                }

                if (s_badge.Equals("SpecFlow", StringComparison.OrdinalIgnoreCase))
                {
                    s_status = s_content.Equals("0") ? "Success" : "Failed";
                }
                else
                {
                    var contentFilename = s_content;

                    if (contentFilename.Contains("*"))
                    {
                        Show("Using wildcards to find content file. The first file found will be used.");
                        var file = IOHelper.GetFirstFile(contentFilename);

                        if (file != null)
                        {
                            Show("File found: {0}", file);
                            contentFilename = file;
                        }
                    }

                    // Some tools like FxCop does not generate a report file when everything is ok.
                    if (File.Exists(contentFilename))
                    {
                        s_content = File.ReadAllText(contentFilename);
                    }
                    else
                    {
                        Show("WARNING: Content file {0} not found, considering a not issue/violations report, using empty report file as badge content.", contentFilename);
                        s_content = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Show("ERROR: {0}", ex.Message);
                Environment.Exit(3);
            }

            return true;
        }

        private static IRestResponse CallApi()
        {
            Show("Generating badge {0} to {1}/{2}...", s_badge, s_repositoryOwner, s_repositoryName);

            var client = new RestClient("{0}/badges".With(s_webApiUrl));
            var request = new RestRequest(s_badge, Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(new
            {
                Owner = s_repositoryOwner,
                Repository = s_repositoryName,
                Name = s_badge,
                Status = s_status,
                Content = s_content,
                AccessToken = s_accessToken
            });

            return client.Execute(request);
        }

        private static void ValidateApiResponse(IRestResponse response)
        {
            NewLine();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Show("Badge generated:");
                var badgeUrl = "{0}/badges/{1}/{2}/{3}".With(s_webApiUrl, s_repositoryOwner, s_repositoryName, s_badge);

                Show(badgeUrl);
            }
            else
            {
                string msg = response.ErrorMessage;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        var error = SimpleJson.DeserializeObject(response.Content) as JsonObject;
                        msg = error.First(p => p.Key.Equals("message", StringComparison.OrdinalIgnoreCase)).Value.ToString();
                        break;

                    case HttpStatusCode.NotFound:
                        msg = "There is no endpoint to badge '{0}'".With(s_badge);
                        break;
                }

                Show("ERROR: badge NOT generated: {0}", msg);

                Environment.Exit(1);
            }
        }

        private static void Show(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        private static void NewLine(int lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine();
            }
        }
        #endregion
    }
}
