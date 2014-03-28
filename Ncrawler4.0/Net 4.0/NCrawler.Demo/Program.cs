using System;
using System.Net;
using System.Text.RegularExpressions;

using NCrawler.Interfaces;
using NCrawler.Services;

namespace NCrawler.Demo {
    internal class Program {
        #region Class Methods

        static string strIncReg = @"\/Post\/\d{4}\/\d{1,2}\/\d{1,2}|\/Post/?$|Page=\d*";
        public static IFilter[] IncludeFilter = new[]
			{
                //\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico|\/Category|\/tags|\/about|\/comment|\/rss|\/?page
                
				(RegexFilter)new Regex(strIncReg,
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};

        public static IFilter[] ExtensionsToSkip = new[]
			{
                
				(RegexFilter)new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico|\/Category|\/tags|\/about|\/comment|\/rss|\/bundles|\.axd)",
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};

        private static void Main() {
            // Remove limits from Service Point Manager
            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            // Run demo 1
            SimpleCrawlDemo.Run("http://diaosbook.com");

            // Run demo 2
            //CrawlUsingIsolatedStorage.Run();

            // Run demo 3
            //CrawlUsingDb4oStorage.Run();

            // Run demo 4
            //CrawlUsingEsentStorage.Run();

            // Run demo 5
            //CrawlUsingDbStorage.Run();

#if DOTNET4
            // Run demo 4
            //CrawlUsingSQLiteDbStorage.Run();
#endif

            // Run demo 6
            //IndexerDemo.Run();

            // Run demo 7
            //FindBrokenLinksDemo.Run();

            // Run demo 8
            //AdvancedCrawlDemo.Run();

            Console.Out.WriteLine("\nDone!");

            Console.ReadKey();
        }

        #endregion
    }
}