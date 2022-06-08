/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/


using System;
using System.IO;
using System.Text;

namespace QuetzalDragon
{
    public class Driver
    {
        const string VERSION = "1.0";

        //---------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis",
            "Syntactic analysis",
            "AST construction",
            "Wat code generation"
        };

        //----------------------------------------------------------
        void PrintAppHeader()
        {
            Console.WriteLine("Quetzal Dragon Compiler, version: " + VERSION);
            Console.WriteLine("Copyright \u00A9 2022 by. ZV, EJGJ, LJRR. ITESM CEM.");
            Console.WriteLine("This program is free software. you may " +
            "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public Lincese version 3 or" +
            "later.");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        void PrintReleaseIncludes()
        {
            Console.WriteLine("Included in this release:");
            foreach (var phase in ReleaseIncludes)
            {
                Console.WriteLine(" * " + phase);
            }
        }

        //-----------------------------------------------------------
        void Run(string[] args)
        {
            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if (args.Length != 1)
            {
                Console.Error.WriteLine(
                  "Please specify the name of the input file."
                );
                Environment.Exit(1);
            }

            try
            {
                var inputPath = args[0];
                var outputPath = Path.ChangeExtension(inputPath, ".wat");
                var input = File.ReadAllText(inputPath);
                var parser = new Parser(
                    new Scanner(input).Scan().GetEnumerator());
                var program = parser.Program();
                Console.Write(program.ToStringTree());
                Console.WriteLine("Syntax OK.");

                var semantic = new SemanticVisitor1();
                semantic.Visit((dynamic)program);

                Console.WriteLine("Semantics OK.");
                Console.WriteLine();
                Console.WriteLine("Primer visitor");
                Console.WriteLine("Vgst Table");
                Console.WriteLine("============");
                foreach (var entry in semantic.Vgst)
                {
                    Console.WriteLine(entry);
                }
                Console.WriteLine();
                Console.WriteLine("Fgst Table");
                Console.WriteLine("============");
                foreach (var entry in semantic.Fgst)
                {
                    Console.WriteLine(entry);
                }
                Console.WriteLine();
                Console.WriteLine("Segundo visitor");
                Console.WriteLine();
                var semantic2 = new SemanticVisitor2(semantic.Fgst, semantic.Vgst);
                semantic2.Visit((dynamic)program);


                Console.WriteLine("Vgst Table");
                Console.WriteLine("============");
                foreach (var entry in semantic2.Vgst)
                {
                    Console.WriteLine(entry);
                }
                Console.WriteLine();
                Console.WriteLine("lst Table");
                Console.WriteLine("============");
                foreach (var entry in semantic2.Fgst)
                {
                    Console.WriteLine(entry);


                }

                var codeGenerator = new WatVisitor(semantic2.Vgst);
                File.WriteAllText(
                    outputPath,
                    codeGenerator.Visit((dynamic) program));
                Console.WriteLine(
                    "Created Wat (WebAssembly text format) file "
                    + $"'{outputPath}'.");
            }
            catch (Exception e)
            {

                if (e is FileNotFoundException || e is SyntaxError || e is SemanticError)
                {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                throw;
            }
        }

        //------------------------------------------------------------
        public static void Main(string[] args)
        {
            new Driver().Run(args);
        }
    }

}