using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace callWebServiesWithSoapRequest.Models
{
    public class WSDLRequestDTO
    {
        public string Method { get; set; }
        public string URLString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SOAPAction { get; set; }
        public byte[] PostData { get; set; }
        public string ProxyAddress { get; set; }
        public string ContentType { get; set; }
        public string Host { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool PreAuthenticate { get; set; }
        public int Timeout { get; set; }
        public string Accept { get; set; }
    }
}
