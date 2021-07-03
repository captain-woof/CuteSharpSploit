using System;
using System.IO;

namespace SharpSploit.Cute
{
    class CuteHelper
    {       
        // Class to help load and execute modules and perform operations with modules

        // Gets current user token
        public static SharpSploit.Credentials.Tokens GetCurrentToken()
        {
            return new SharpSploit.Credentials.Tokens();
        }

        // Prints About
        public static void PrintAbout()
        {
            Console.WriteLine("CuteSharpSploit is an executable wrapper around SharpSploit, a .NET post-exploitation library written in C# by Ryan Cobb. The application provides a CLI, and performs actions thorugh loadable modules. Type help to get a list of available commands.");
        }

        // Prints help menu
        public static void PrintHelp()
        {
            Console.WriteLine("Commands available\n------------------");
            Console.WriteLine("1. load | use <module name>");
            Console.WriteLine("2. set <option name> <option value>");
            Console.WriteLine("3. info | options");
            Console.WriteLine("4. run");
            Console.WriteLine("5. search <module name (substring)>");
            Console.WriteLine("6. show all");
            Console.WriteLine("7. powershell");
            Console.WriteLine("8. help");
            Console.WriteLine("9. about");
            Console.WriteLine("10. exit\n");
        }

        // Drops into a (pseudo) powershell prompt
        public static void DropIntoPowershell()
        {
            Console.WriteLine("Dropping into powershell");
            String Command = "", CurrentDirectory = Directory.GetCurrentDirectory();

            while (!Command.ToLower().Equals("exit"))
            {
                Console.Write(String.Format("PS ({0}) {1}# ",new SharpSploit.Credentials.Tokens().WhoAmI(),CurrentDirectory));
                Command = Console.ReadLine();
                if (!Command.ToLower().Equals("exit"))
                {
                    if((Command.ToLower().StartsWith("cd ")) || (Command.ToLower().StartsWith("chdir ")) || (Command.ToLower().StartsWith("Set-Location ")))
                    {
                        CurrentDirectory = Command.Split(' ')[1];
                        Directory.SetCurrentDirectory(CurrentDirectory);
                    }
                    else
                    {
                        Console.Write(SharpSploit.Execution.Shell.PowerShellExecute(Command));                        
                    }
                }
            }
        }
    }
}
