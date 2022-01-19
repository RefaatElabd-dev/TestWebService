using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using localhost = ConsumeWebServise.ServiceReference1;

namespace ConsumeWebServise
{
    class Program
    {
        static void Main(string[] args)
        {
            localhost.MathServiceSoapClient MyMathService = new localhost.MathServiceSoapClient();
            Console.WriteLine("2 + 4 = {0}", MyMathService.Add(2, 4));
            Console.ReadLine();
        }
    }
}
