using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Topshelf;

namespace callWebServiesWithSoapRequest
{
    class Program { 
        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://localhost:3022/MathService.asmx");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/Add");
            //Content_type    
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //HTTP Body
            
            //return HttpWebRequest    
            return Req;
        }  

        
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x=>
            {
                x.Service<MainWorker>(s =>
                {
                    s.ConstructUsing(mainWorker => new MainWorker());
                    s.WhenStarted(mainWorker => mainWorker.Start());
                    s.WhenStopped(mainWorker => mainWorker.Stop());

                    x.RunAsLocalSystem();
                    x.SetServiceName("CallWSDLService");
                    x.SetDisplayName("Call WSDL Service");
                    x.SetDescription("Call WSDL Server By a Windows service");
                });
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
            Console.ReadLine();
        }
    }
}
