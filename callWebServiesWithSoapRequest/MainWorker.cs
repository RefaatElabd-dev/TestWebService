using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Net;
using callWebServiesWithSoapRequest.Models;

namespace callWebServiesWithSoapRequest
{
    public static class WSDL
    {
        const string endPoint = "http://Localhost:7047/Royal_Lab/WS/ROYAL/Page/WSAnalyzer";
        private static string RquestBody()
        {
            return @"<?xml version='1.0' encoding='utf-8'?>
                                <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                                    <soap:Body>
                                        <Add xmlns='http://tempuri.org/'>
                                            <a> 5 </a>
                                            <b> 17 </b>
                                        </Add>
                                    </soap:Body>
                                </soap:Envelope>";
        }
        private static HttpWebRequest PrepareRequest(WSDLRequestDTO requestDTO)
        {
            //string xmlString = RquestBody();

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestDTO.URLString);
            //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(@"http://localhost:3022/MathService.asmx");
            myReq.Method = requestDTO.Method;// "POST";
            myReq.ContentType = requestDTO.ContentType;//"text/xml; encoding=utf-8";
            myReq.Timeout = requestDTO.Timeout;// 180000;
            myReq.KeepAlive = true;
            myReq.Headers.Add("SOAPAction", requestDTO.SOAPAction); //"http://tempuri.org/Add"
            myReq.Accept = requestDTO.Accept;//"gzip,deflate";
            //byte[] PostData = Encoding.UTF8.GetBytes(xmlString.Trim());
            myReq.UseDefaultCredentials = requestDTO.UseDefaultCredentials;// false;
            NetworkCredential cred;
            cred = new NetworkCredential(requestDTO.UserName, requestDTO.Password);
            myReq.Credentials = cred;
            //myReq.Host = WebRequestHost.Trim();
            //myReq.Credentials = new System.Net.NetworkCredential("UserName", "P@$$w0rd");
            myReq.PreAuthenticate = requestDTO.PreAuthenticate;//true;
            //string SetProxy;
            //SetProxy = "192.168.91.202:8090"; // something like this... "10.2.0.1:8080";
            var proxyObject = new WebProxy(requestDTO.ProxyAddress);
            myReq.Proxy = proxyObject;
            try
            {
                var writer = myReq.GetRequestStream();
                writer.Write(requestDTO.PostData, 0, requestDTO.PostData.Length);
                //writer.Write(PostData, 0, PostData.Length);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Writer Exception " + ex.Message + ex.InnerException + " host : " + myReq.Host);
            }

            return myReq;
        }
        public static string GetSOAPResponse()
        {
            HttpWebRequest myReq = PrepareRequest(GetRequestDto());

            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            string resp;
            using (var responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    resp = sr.ReadToEnd();
                }
            }

            return resp;
        }

        private static WSDLRequestDTO GetRequestDto()
        {
            //prepare from config
            #region test Request
            //return new WSDLRequestDTO
            //{
            //    URLString = @"http://localhost:3022/MathService.asmx",
            //    Method = "Post",
            //    PostData = Encoding.UTF8.GetBytes(RquestBody().Trim()),
            //    ContentType = "text/xml; encoding=utf-8",
            //    PreAuthenticate = true,
            //    UseDefaultCredentials = false,
            //    Timeout = 18000,
            //    ProxyAddress = "192.168.91.202:8090",
            //    Accept = "gzip,deflate",
            //    SOAPAction = "http://tempuri.org/Add",
            //    UserName = "UserName",
            //    Password = "Password"
            //};
            #endregion

            return new WSDLRequestDTO
            {
                URLString = @"http://localhost:3022/MathService.asmx",
                Method = "Post",
                PostData = Encoding.UTF8.GetBytes(RquestBody().Trim()),
                ContentType = "text/xml; encoding=utf-8",
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                Timeout = 18000,
                ProxyAddress = "192.168.91.202:8090",
                Accept = "gzip,deflate",
                SOAPAction = "http://tempuri.org/Add",
                UserName = "UserName",
                Password = "Password"
            };
        }
    }

    public class MainWorker
    {
        private readonly Timer _timer;
        
        public MainWorker()
        {
            _timer = new Timer(3000) { AutoReset = true };
            _timer.Elapsed += TimerElabsed;
        }

        private void TimerElabsed(object sender, ElapsedEventArgs e)
        {
            //Prepare Request
            //Send Request
            //Write Response
            string[] lines = new string[] { DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss :::: " + WSDL.GetSOAPResponse()) };
            string LogPath = ConfigurationManager.AppSettings["LogPath"];
            File.AppendAllLines(LogPath, lines);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
