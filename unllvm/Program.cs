using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unllvm
{
    class Program
    {
        public static byte[] HexToBytes(string value)
        {
            if (value == null || value.Length == 0)
                return new byte[0];
            if (value.Length % 2 == 1)
                throw new FormatException();
            byte[] result = new byte[value.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = byte.Parse(value.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
            return result;
        }
        static void Main(string[] args)
        {
            if(args.Length==0)

            Console.WriteLine("unllvm hexllvm_filename");
            var filename = args[0];
            var txt = System.IO.File.ReadAllText(filename);
            var srcbytes = HexToBytes(txt);
            using (var ms = new System.IO.MemoryStream(srcbytes))
            {
                var bts = llvm.QuickFile.FromFile(ms);
                System.IO.File.WriteAllBytes(filename + ".json", bts);
            }
        }
    }
}
