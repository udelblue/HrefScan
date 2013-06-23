using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace HrefScan
{
    public class LinkScrapper
    {

        private List<string> _hrefList;
        public List<string> HRefList
        {
            get
            {
                if (_hrefList == null)
                {
                    this._hrefList = new List<string>();
                }

                _hrefList = ProcessList(_hrefList);

                return _hrefList;
            }
            set { _hrefList = value; }
        }

        private String _Host;
        public String Host
        {
            get { return _Host; }
            set { _Host = value; }
        }       

        public LinkScrapper(string href)
        {


            string siteString = href;
            Uri site = new Uri(siteString);
            string siteScheme = site.Scheme;
            string sitePath = site.AbsolutePath;
            this.Host = site.Host;
            int sitePort = site.Port;


            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(site.OriginalString);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {

                // sb.AppendLine(link.InnerText + " , " + " " + " , " + attLink.Value);
                HtmlAttribute attLink = link.Attributes["href"];
                if (attLink != null)
                {
                    string loclink = attLink.Value;

                    if (!string.IsNullOrEmpty(loclink))
                    {


                        if (!loclink.StartsWith("#") || !loclink.StartsWith("java"))
                        {

                            if (loclink.StartsWith("/"))
                            {

                                HRefList.Add("http://" + this.Host + loclink);

                            }
                            else
                            {
                                HRefList.Add(loclink);
                            }




                        }
                    }




                }





            }

        }

        private List<string> ProcessList(List<string> hreflist)
        {

            hreflist.Remove("");

            for (int i = 0; i < hreflist.Count; i++)
            {
                string href = hreflist[i];
                if (string.IsNullOrEmpty(href))
                {
                    hreflist.RemoveAt(i);
                }

                if (href[0] == '#')
                {
                    hreflist.RemoveAt(i);
                }

                if (href.Contains("javascript:"))
                {
                    hreflist.RemoveAt(i);
                }

                if (href[0] == 'w' | href[0] == 'W')
                {
                    hreflist.RemoveAt(i);
                    hreflist.Add(@"http://" + href);
                }


            }



            return hreflist;
        }








    }
}
