/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/


using System;
using System.IO;
using System.Text;

namespace QuetzalDragon{
    public class Driver{
        const string VERSION = "1.0";
        
        //---------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis"
        };

        //----------------------------------------------------------
        void PrintAppHeader(){
            Console.WriteLine("Quetzal Dragon Compiler, version: " + VERSION);
            Console.WriteLine("Copyright \u00A9 2022 by. ZV, EJGJ, LJRR. ITESM CEM.");
            Console.WriteLine("This program is free software. you may " +
            "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public Lincese version 3 or"+
            "later.");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        void PrintReleaseIncludes(){
            Console.WriteLine("Included in this release:");
            foreach (var phase in ReleaseIncludes){
                Console.WriteLine(" * " + phase);
            }
        }

        //-----------------------------------------------------------
        void Run(string[] args){
            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if(args.Length != 1){
                Console.Error.WriteLine(
                  "Please specify the name of the input file."  
                );
                Environment.Exit(1);
            }

            try {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);

                Console.WriteLine(
                    $"===== Tokens from: \"{inputPath}\" ====="
                    );
                var count = 1;
                foreach (var tok in new Scanner(input).Scan()){
                    Console.WriteLine($"[{count++}] {tok}");
                }
            } catch (FileNotFoundException e){
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        //------------------------------------------------------------
        public static void Main(string[] args){
            new Driver().Run(args);
        }
    }
    
}