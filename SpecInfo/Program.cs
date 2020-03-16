using System.IO;

namespace SpecInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Spec spec = new Spec())
            {
                string info = spec.GetInfo();
                string path = @".\spec.txt";
                File.WriteAllText(path, info);
            }
        }
    }
}
