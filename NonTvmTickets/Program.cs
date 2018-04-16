using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NonTvmTickets
{
    internal class Program
    {
        /// <summary>
        /// Print all ticket types in the RCS_R_T file that do not have a channel element of for for any licensee. This
        /// means that this ticket type can never be sold on a TVM.
        /// </summary>
        /// <param name="args">The rcs_r_t filename</param>
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
                var ns = xdoc.Root.GetDefaultNamespace();
                var nonTvmTickets = xdoc.Descendants(ns + "FTOT")
                    .Select(x => new
                    {
                        Ftot = x.Attribute("t").Value,
                        Channels = x.Descendants(ns + "Channel").Select(y => y.Attribute("ch").Value).Distinct().OrderBy(z=>z).ToList()
                    });
                foreach (var result in nonTvmTickets.Where(x=>!x.Channels.Contains("00004")))
                {
                    var name = TicketRefData.GetTicketTypeName(result.Ftot) ?? "ERROR: CANNOT FIND TICKET TYPE NAME";
                    Console.WriteLine($"ftot: {result.Ftot} {name}");
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
