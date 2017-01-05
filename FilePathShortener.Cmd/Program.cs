using System;
using System.IO;
using System.Linq;

namespace FilePathShortener.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootDirectory = Directory.GetCurrentDirectory();

            if (args.Any())
            {
                rootDirectory = args[0];
            }

            Console.Out.WriteLine("PLEASE BE AWARE THAT THESE CHANGES CANNOT BE UNDONE!");
            Console.Out.WriteLine("Are you sure to rename all directories under the root directory '{0}' (y, n)?", rootDirectory);

            string answer = Console.In.ReadLine();

            if (answer == null || answer.ToLower() != "y")
            {
                return;
            }

            try
            {
                ShortenDirectoryNames(rootDirectory);
                Console.Out.WriteLine("Successfully renamed the directory tree under '{0}' (press any key to exit)", rootDirectory);
                Console.In.ReadLine();
            }
            catch (Exception exception)
            {
                Console.Out.WriteLine("{0} (press any key to exit)", exception.Message);
                Console.In.ReadLine();
            }


        }

        private static void ShortenDirectoryNames(string rootDirectory, string name = null)
        {
            try
            {
                var adaptedRootDirectory = rootDirectory;

                if (name != null)
                {
                    adaptedRootDirectory = GetUpdatedRootDirectory(rootDirectory, name);

                    if (rootDirectory != adaptedRootDirectory && !Directory.Exists(adaptedRootDirectory))
                    {
                        Directory.Move(rootDirectory, adaptedRootDirectory);
                    }
                }

                var directories = Directory.GetDirectories(adaptedRootDirectory);
                var subdirectoryName = 0;

                foreach (var directory in directories)
                {
                    ShortenDirectoryNames(directory, Convert.ToString(subdirectoryName++));
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Failed in {0} with error '{1}'", rootDirectory, exception.Message));
            }
        }

        private static string GetUpdatedRootDirectory(string rootDirectory, string name)
        {
            var pathWithoutRootDirectory = Path.GetDirectoryName(rootDirectory);

            if (pathWithoutRootDirectory == null)
            {
                throw new Exception(string.Format("There is no chance to rename the directory: {0}", rootDirectory));
            }

            return Path.Combine(pathWithoutRootDirectory, name)
            ;
        }
    }
}
