using System;
using System.IO;
using System.Security.Cryptography;

namespace FillFile
{
    public class Variables
    {
        public bool? IsRandom { get; set; }
        public int FileLength { get; set; }
        public string FileName { get; set; }
    }

    class Program
    {
        private static string version = "1.0.0";

        static void Main(string[] args)
        {
            try
            {
                Variables v = ParseArgs(args);

                if (v.IsRandom != null)
                {
                    Console.WriteLine("FillFile " + version);

                    if ((bool)v.IsRandom)
                    {
                        Console.WriteLine("Creating Random Filled File...");
                        File.WriteAllBytes(v.FileName, CreateRandomFilledFile(v.FileLength));
                    }
                    else
                    {
                        Console.WriteLine("Creating Zero Filled File...");
                        File.WriteAllBytes(v.FileName, CreateZeroFilledFile(v.FileLength));
                    }
                }
                else
                    Terminate("Invalid input parameters.");
            }
            catch
            {
                Terminate("Input parameters incorrect");
            }

            Console.WriteLine("File Created.");
        }

        private static Variables ParseArgs(string[] args)
        {
            //FillFile -z 512 -o FileName
            //FillFile -r 512 -o FileName
            //FillFile -h

            Variables v = new Variables();

            if (args.Length == 4 || args.Length == 1)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-z")
                    {
                        v.IsRandom = false;
                        v.FileLength = int.Parse(args[i + 1]);
                        i++;
                    }

                    if (args[i] == "-r")
                    {
                        v.IsRandom = true;
                        v.FileLength = int.Parse(args[i + 1]);
                        i++;
                    }

                    if (args[i] == "-o")
                    {
                        v.FileName = args[i + 1];
                        i++;
                    }

                    if (args[i] == "-h" || args[i] == "-help")
                    {
                        Console.WriteLine("FillFile " + version);
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine("To create a zero filled file:");
                        Console.WriteLine("FillFile -z 512 -o FileName");
                        Console.WriteLine("To create a random filled file:");
                        Console.WriteLine("FillFile -r 512 -o FileName");
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine("Parameters:");
                        Console.WriteLine("-h                   - Display help");
                        Console.WriteLine("-o (Filename)        - Output file name");
                        Console.WriteLine("-r (length)          - Random filled file of a given length in bytes");
                        Console.WriteLine("-z (length)          - Zero filled file of a given length in bytes");
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                Terminate("Invalid number of parameters.");
            }

            return v;
        }

        private static byte[] CreateZeroFilledFile(int length)
        {
            return new byte[length];
        }

        private static byte[] CreateRandomFilledFile(int length)
        {
            byte[] rnd = new byte[length];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rnd);

            return rnd;
        }

        private static void Terminate(string message)
        {
            Console.WriteLine("FillFile " + version);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(message);
            Console.WriteLine("Incorrect usage.  Please type 'FillFile -h' for help.");
            Environment.Exit(0);
        }
    }
}
