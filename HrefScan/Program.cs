using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace HrefScan
{
    class Program
    {



        static void Main(string[] args)
        {


            string url = args[0];
            string host;

            if (url.Contains("http"))
            {
                Console.WriteLine("Scanning");

                LinkScrapper ls = new LinkScrapper(url);
                host = ls.Host;
            
                LinkList<Link> linkslist = new LinkList<Link>();

                foreach (var link in ls.HRefList)
                {
                    Link lt = new Link(link, ls.Host);

                    linkslist.Add(lt);
                }

                //write xml
                if (args.Contains("/xml"))
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(linkslist.GetType());
                    x.Serialize(XmlWriter.Create("LinkResults.xml"), linkslist);

                }


                //write xml
                if (args.Contains("/csv"))
                {
                      string filePath = @"LinkResults.csv";
                    StringBuilder sb = new StringBuilder();
                     sb.AppendLine("HyperReference" + "," + "Link" + "," + "StatusCode");
                    foreach (Link link in linkslist)
                    {
                        sb.AppendLine(link.Href + "," + link.code + "," + link.StatusCode);
                    }


                     File.WriteAllText(filePath, sb.ToString());


                   

                }


                //write xml
                if (args.Contains("/output"))
                {
                    foreach (Link lt in linkslist)
                    {
                        Console.WriteLine(lt.code + " : " + lt.StatusCode + " : " + lt.Href);
                    }

                    Console.WriteLine("Complete");
                }

                    
             
            }

            else
            {
                Console.WriteLine("Site must be formated with complete url. Must include http, Example http://www.google.com ");
            }


        

            

            Console.ReadLine();

        }





    }
}
