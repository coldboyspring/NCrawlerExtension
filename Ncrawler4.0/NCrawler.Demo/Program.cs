using System;
using System.Net;
using System.Text.RegularExpressions;

using NCrawler.Interfaces;
using NCrawler.Services;
using NCrawler.Demo.Helpers;
using System.Text;
using System.Collections.Generic;

namespace NCrawler.Demo {
    internal class Program {
        #region Class Methods

        static string strIncReg = @"city\?cityCode=|destination";
        //static string strIncReg = @"airport";
        public static IFilter[] IncludeFilter = new[]
			{
				(RegexFilter)new Regex(strIncReg,
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};

        public static IFilter[] ExtensionsToSkip = new[]
			{                
				(RegexFilter)new Regex(@".+",
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
			};

        private static void Main() {
            // Remove limits from Service Point Manager
            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            Global.OldLink = XMLHelper.XmlDeserializeFromFile<List<Link>>("E://dest.xml", Encoding.UTF8);

           
            // Run demo 1
            //SimpleCrawlDemo.Run("http://www.kopu.cn/airport");
            SimpleCrawlDemo.Run("http://www.kopu.cn/destination");
            //SimpleCrawlDemo.Run("http://diaosbook.com");

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
            XMLHelper.XmlSerializeToFile(Global.LinkList, "E:\\" + DateTime.Now.ToString("yyyyMMddmm") + ".xml", Encoding.UTF8);
            Console.Out.WriteLine("\nDone!");

            Console.ReadKey();
        }

        #endregion
    }
}