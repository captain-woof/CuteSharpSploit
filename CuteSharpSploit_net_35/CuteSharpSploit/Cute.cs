using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpSploit.Cute
{
    // Main class for CuteSharpSploit
    class Cute
    {
        // List of all modules
        static String[] AllModules = {
                "credentials/mimikatz/all",
                "credentials/mimikatz/command",
                "credentials/mimikatz/mimikatz_shell",
                "credentials/mimikatz/dcsync",
                "credentials/mimikatz/logonpasswords",
                "credentials/mimikatz/lsacache",
                "credentials/mimikatz/lsasecrets",
                "credentials/mimikatz/pth",
                "credentials/mimikatz/samdump",
                "credentials/mimikatz/wdigest",
                "credentials/tokens/bypassuac",
                "credentials/tokens/getsystem",
                "credentials/tokens/impersonateprocess",
                "credentials/tokens/impersonateuser",
                "credentials/tokens/maketoken",
                "credentials/tokens/rev2self",
                "execution/execute_dotnet_exe_method",
                "execution/execute_dotnet_exe",
                "execution/execute_powershell_cmd",
                "execution/execute_shell_cmd",
                "execution/execute_shellcode",
                "lateral_movement/dcom/remote_exec",
                "lateral_movement/wmi/remote_exec",
                "lateral_movement/service/create_new_remote_service",
                "lateral_movement/service/delete_remote_service",
                "lateral_movement/service/start_remote_service",
                "lateral_movement/service/stop_remote_service",
                "lateral_movement/psexec/remote_exec",
                "lateral_movement/psremote/remote_exec",
                "persistence/com/hijack_clsid",
                "persistence/autorun/install_autorun",
                "persistence/startup/install_startup_payload",
                "evasion/amsi/patch_amsi_dll"
            };

        // Searches in all module list, prints results
        public static void SearchModules(String Substring)
        {
            foreach (String Module in AllModules)
            {
                if (Module.Contains(Substring.ToLower()))
                {
                    Console.WriteLine(Module);
                }
            }
            Console.WriteLine("");
        }

        // Main
        public static void Main(String[] args)
        {
            // Initialization
            Object Output = null;
            String Input = "", ModuleOptionName = "", ModuleOptionValue = "";
            CuteModule CurrentModule = new CuteModule("BLANK");

            // Print number of modules
            Console.WriteLine(String.Format("CuteSharpSploit loaded {0} modules\n",AllModules.Length));

            // Menu Loop
            while (!Input.ToLower().Equals("exit"))
            {
                Console.Write(String.Format("CuteSharpSploit ({0})# ",(CurrentModule.IsInvalid()) ? "nil" : CurrentModule.GetModuleName()));
                Input = Console.ReadLine().ToLower();

                // Perform action based on input
                if (Input.Equals("help"))
                {
                    CuteHelper.PrintHelp();
                }
                else if (Input.Equals("about"))
                {
                    CuteHelper.PrintAbout();
                }
                else if(Input.Equals("show all"))
                {
                    SearchModules("");
                }
                else if (Input.Equals("powershell"))
                {
                    CuteHelper.DropIntoPowershell();
                }
                else if(Input.StartsWith("search "))
                {
                    SearchModules(Input.Split(' ')[1]);
                }
                else if ((Input.StartsWith("load ")) || (Input.StartsWith("use ")))
                {
                    CurrentModule = new CuteModule(Input.Split(' ')[1]);
                    if (CurrentModule.IsInvalid())
                    {
                        Console.WriteLine("Invalid module selected");
                    }
                }
                else if (Input.StartsWith("set "))
                {
                    ModuleOptionName = Input.Split(' ')[1];
                    ModuleOptionValue = Input.Split(' ')[2];
                    if(CurrentModule.SetModuleOptionValue(ModuleOptionName,ModuleOptionValue))
                    {
                        Console.WriteLine(String.Format("Set {0} => {1}",ModuleOptionName,ModuleOptionValue));
                    }
                    else
                    {
                        Console.WriteLine("No such module option, enter 'info' for a list of options.");
                    }
                }
                else if ((Input.Equals("info")) || (Input.Equals("options")))
                {
                    Console.WriteLine(CurrentModule.GetModuleInfo());
                }
                else if (Input.Equals("run"))
                {
                    try
                    {
                        Output = CurrentModule.Run();
                        if (typeof(bool) == Output.GetType())
                        {
                            if ((bool)Output)
                            {
                                Console.WriteLine("Command executed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to execute command!");
                            }
                        }
                        else
                        {
                            Console.WriteLine(Output);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(String.Format("Exception encountered:\n{0}",e.Message));
                    }
                }
            }
            Console.WriteLine("\nBye!\n{Written by Captain Woof}");
        }
    }
}
