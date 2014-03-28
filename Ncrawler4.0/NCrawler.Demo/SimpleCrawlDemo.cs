// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleCrawlDemo.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SimpleCrawlDemo type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.LanguageDetection.Google;
using NCrawler.Interfaces;
using NCrawler.Demo.Extensions;
using NCrawler.MP3Processor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;

namespace NCrawler.Demo {
    public class SimpleCrawlDemo {
        #region Class Methods

        public static void Run(string url = "http://ncrawler.codeplex.com") {
            NCrawlerModule.Setup();
            Console.Out.WriteLine("Simple crawl demo");

            // Setup crawler to crawl http://ncrawler.codeplex.com
            // with 1 thread adhering to robot rules, and maximum depth
            // of 2 with 4 pipeline steps:
            //	* Step 1 - The Html Processor, parses and extracts links, text and more from html
            //  * Step 2 - Processes PDF files, extracting text
            //  * Step 3 - Try to determine language based on page, based on text extraction, using google language detection
            //  * Step 4 - Dump the information to the console, this is a custom step, see the DumperStep class
            using (Crawler c = new Crawler(new Uri(url),
                new HtmlDocumentProcessor(), // Process html
                //new iTextSharpPdfProcessor.iTextSharpPdfProcessor(), // Add PDF text extraction
                //new GoogleLanguageDetection(), // Add language detection
                //new Mp3FileProcessor(), // Add language detection
                new UrlReceiverStep()) {
                    // Custom step to visualize crawl
                    MaximumThreadCount = 8,
                    MaximumCrawlDepth = 2,
                    //MaximumCrawlTime = TimeSpan.FromSeconds(1)
                    ExcludeFilter = Program.ExtensionsToSkip,
                    IncludeFilter = Program.IncludeFilter
                }) {
                // Begin crawl
                WebProxy webProxy = new WebProxy("61.143.124.155", 80);
                c.WebProxy = webProxy;
                c.Crawl();
            }
        }

        #endregion
    }

    #region Nested type: UrlReceiverStep

    /// <summary>
    /// Custom pipeline step, to dump url to console
    /// </summary>
    internal class UrlReceiverStep : IPipelineStep {
        #region IPipelineStep Members

        //Regex regex = new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
        //            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        string[] filterExt = { ".jpg", ".css", ".js", ".gif", ".jpeg", ".png", ".ico" };


        /// <summary>
        /// </summary>
        /// <param name="crawler">
        /// The crawler.
        /// </param>
        /// <param name="propertyBag">
        /// The property bag.
        /// </param>
        public void Process(Crawler crawler, PropertyBag propertyBag) {
            lock (this) {

                //var uri = propertyBag.Step.Uri.AbsoluteUri;
                //foreach (var ext in filterExt) {
                //    if (uri.EndsWith(ext)) {
                //        return;
                //    }
                //}
                //if (Global.OldLink.Count(l => l.Url == propertyBag.Step.Uri.AbsoluteUri) > 0) {
                //    return;
                //}


                Global.LinkList.Add(new Link() {
                    Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Title = propertyBag.Title,
                    Url = propertyBag.Step.Uri.AbsoluteUri
                });
                Console.Out.WriteLine(ConsoleColor.Gray, "No.{0},Url: {1}", Global.LinkList.Count, propertyBag.Step.Uri);
                Console.WriteLine();

            }
        }

        #endregion
    }

    #endregion

    #region 全局变量

    public static class Global {
        public static List<Link> OldLink = new List<Link>();
        public static List<Link> LinkList = new List<Link>();
    }
    #endregion

    public class Link {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}