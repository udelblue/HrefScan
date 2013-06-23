using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace HrefScan
{
    [Serializable]
    public class Link
    {

        private int _code;
        [XmlAttribute("Code")] 
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }


        
        private String _StatusCode;
        [XmlAttribute("Status_Code")]  
        public String StatusCode
        {
            get {
                if (string.IsNullOrEmpty(_StatusCode))
                {
                    _StatusCode = "Not Available";
                }

              _StatusCode =  _StatusCode.Replace('_', ' ');
                
                return _StatusCode; }
            set { _StatusCode = value; }
        }


        private String _href;
        [XmlAttribute("HRef")]  
        public String Href
        {
            get { return _href; }
            set { _href = value; }
        }


        private String _host;
        [XmlIgnore]  
        public String Host
        {
            get { return _host; }
            set { _host = value; }
        }


        public Link(string href, string host)
        {
            this.Href = href;
            this.Host = host;
            Process();
        }

        public Link()
        {

        }

        public Link(string href)
        {
            if (href.StartsWith("/"))
            {
                throw new System.ArgumentException("Parameter cannot begin with /");
            }
            this.Href = href;
            Process();
        }

        private void Process()
        {

            Uri site = new Uri(this.Href);
            string siteScheme = site.Scheme;
            string sitePath = site.AbsolutePath;
            string siteHost = site.Host;
            int sitePort = site.Port;

            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest
            //                               .Create(site);

            //webRequest.Timeout = 10000;
            //webRequest.AllowAutoRedirect = false;
            //HttpWebResponse response;

            //try
            //{


            //    response = (HttpWebResponse)webRequest.GetResponse();
            //    this.code = (int)response.StatusCode;

            //}

            try
            {
                var request = WebRequest.Create(site);
                using (WebResponse response = request.GetResponse())
                {

                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    this.code = (int)httpResponse.StatusCode;

                    //using (var responseStream = response.GetResponseStream())
                    //{
                    //    using (var reader = new StreamReader(responseStream))
                    //    {

                    //    }
                    //}
                }
            }


            catch (Exception ex)
            {


                var httpEx = ex as System.Net.WebException;
                //if (httpEx.ToString().Contains("timed out"))
                //{
                //    this.code = 999;
                //}

                if (httpEx != null)
                {
                    if (httpEx.Status == WebExceptionStatus.ProtocolError)
                    {

                        var tempcode = ((HttpWebResponse)httpEx.Response).StatusCode;

                        Regex regex = new Regex(@"[0-9][0-9][0-9]");
                        Match match = regex.Match(ex.ToString());
                        if (match.Success)
                        {

                            this.code = Convert.ToInt32(match.Value);
                        }




                    }
                }
            }



            var _sc = Enum.Parse(typeof(HttpStatusCode), this.code.ToString());
            this.StatusCode = _sc.ToString();
           
        }






        public enum HttpStatusCode
        {

            Moved = 301,
            OK = 200,
            Redirect = 302,
            Deadlink = 404,
            Created = 201,
            Accepted =202,
            Non_Authoritative_Information = 203,
            No_Content = 204,
            Reset_Content = 205,
            Partial_Content =  206,
            Multi_Status = 207,
            Already_Reported = 208,
            IM_Used = 226,
            Bad_Request = 400,
            Unauthorized = 401,
            Payment_Required  = 402,
            Forbidden = 403,
            Request_Timeout = 408,

        }

    }

    [XmlRoot("Links"), Serializable]  
    public class LinkList<T> : List<Link>
    {


    }

}
