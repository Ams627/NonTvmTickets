using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NonTvmTickets
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new Exception($"You must specify a filename for the RCS_R_T file");
                }
                var filename = args[0];
                var xdoc = XDocument.Load(filename);
                var nonTvmTickets = xdoc.Descendants("FTOT")
                    .Select(x => new { Ftot = x.Attribute("t").Value, Channels = x.Descendants("Channel").Select(y => y.Attribute("ch").Value).ToList() });
                foreach (var result in nonTvmTickets)
                {
                    Console.WriteLine($"ftot: {result.Ftot} channels: {string.Join(",", result.Channels)}");
                }
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
