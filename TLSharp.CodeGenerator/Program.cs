using System;
using System.IO;
using System.Reflection;

namespace TLSharp.CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 && !File.Exists("scheme.tl"))
            {
                Console.WriteLine(@"Usage: TLSharp.CodeGenerator <C:\path\to\scheme.tl> (C:\path\to\compiled.src)");
                return;
            }
            if (args.Length == 0)
                args = new string[1] { "scheme.tl " };

            if (!File.Exists(args[0])) // if the file doesn't exist,
                args[0] = Path.Combine( // convert it from relative -> full path
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), args[0]);

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"File {args[0]} doesn't exist");
                return;
            }

            Console.WriteLine("Tokenizing...");
            var all = new Tokenizer().Tokenize(File.ReadAllText(args[0]));
            Console.WriteLine("Tokenized");

            if (args.Length < 2)
                args = new string[2]
                {
                    args[0],
                    Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(args[0]) + ".cs")
                };

            var compiler = new CSharpCodeGenerator { Namespace = "TLSharp.Core.MTProto" };
            if (args.Length > 1)
            {
                Console.WriteLine("Generating...");
                File.WriteAllText(args[1], compiler.GetTLObjectsCode(all, 0));
                Console.WriteLine("Generated");
                return;
            }
        }
    }
}
