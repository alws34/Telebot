using Contracts;
using Models;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace SpecPlugin
{
    [Export(typeof(IPlugin))]
    public class SpecPlugin : IPlugin
    {
        public SpecPlugin()
        {
            Pattern = "/spec";
            Description = "Get full hardware information.";
            MinOSVersion = new Version(5, 1);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            using (Spec spec = new Spec())
            {
                string info = spec.GetInfo();
                string path = @".\spec.txt";
                File.WriteAllText(path, info);
            }

            var fileStrm = new FileStream(@".\spec.txt", FileMode.Open);

            var result = new Response(fileStrm);

            await resp(result);
        }
    }
}
