using System.IO;

namespace SpecInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Spec spec = new Spec();
            string info = spec.GetInfo();
            string path = @".\info.txt";
            File.WriteAllText(path, info);
        }
    }
}
