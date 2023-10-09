using Microsoft.Win32;

namespace PAKPack_Registry
{
    internal class Program
    {
        static string PAKPackPath    = new FileInfo(@"PAKPack\PAKPack.exe").FullName; 
        const string  classesRoot    = "HKEY_CLASSES_ROOT";
        const string  subkeyAsterix  = "*";
        const string  subkeyFolder   = "Folder";
        const string  subkeyShell    = "shell\\PAKPack";
        const string  subkeyCM       = subkeyAsterix + "\\CommandMenus";
        const string  subkeyFolCM    = subkeyFolder + "\\CommandMenus";
        const string  shell          = classesRoot + "\\" + subkeyAsterix + "\\" + subkeyShell;
        const string  folShell       = classesRoot + "\\" + subkeyFolder + "\\" + subkeyShell;
        const string  cmPak          = classesRoot + "\\" + subkeyCM + "\\PAK";
        const string  folPak         = classesRoot + "\\" + subkeyFolCM + "\\PAK";
        const string  cmpacking      = classesRoot + "\\" + subkeyCM + "\\packing";
        const string  folpacking     = classesRoot + "\\" + subkeyFolCM + "\\packing";
        
        public static void Main()
        {
            
            Console.WriteLine("This program edits Registry values to add Right-Click context menus for PAK Packing & Unpacking\n"
                            + "Continue? (y/N)");
            var key = Console.ReadKey(true).KeyChar;
            if (key.ToString().ToLower() != "y")
                return;
            Registry.SetValue(shell, "MUIVerb", "PAKPack");
            Registry.SetValue(shell, "ExtendedSubCommandsKey", subkeyCM + "\\PAK");
            Console.WriteLine($"\nAdding unpacking function...");
            Registry.SetValue(cmPak + "\\shell\\unpack", "MUIVerb", "Unpack File");
            Registry.SetValue(cmPak + "\\shell\\unpack\\command", "", $"\"{PAKPackPath}\" unpack \"%1\"");
            Registry.SetValue(cmPak + "\\shell\\pack", "MUIVerb", "Pack File");
            Registry.SetValue(cmPak + "\\shell\\pack", "ExtendedSubCommandsKey", subkeyCM + "\\packing");
            Registry.SetValue(cmPak + "\\shell\\list", "MUIVerb", "PAK File Details");
            Registry.SetValue(cmPak + "\\shell\\list\\command", "", $"\"{PAKPackPath}\" list \"%1\"");
            Console.WriteLine($"Adding simple file details viewer...");

            Registry.SetValue(folShell, "MUIVerb", "PAKPack");
            Registry.SetValue(folShell, "ExtendedSubCommandsKey", subkeyFolCM + "\\PAK");
            Registry.SetValue(folPak + "\\shell\\pack", "MUIVerb", "Pack Folder");
            Registry.SetValue(folPak + "\\shell\\pack", "ExtendedSubCommandsKey", subkeyFolCM + "\\packing");


            Console.WriteLine($"Adding packing function for each PAK version...");
            foreach (string version in new string[] {"v1", "v2", "v2be", "v3", "v3be"})
            {
                Registry.SetValue(cmpacking + "\\shell\\" + version, "MultiSelectModel", "Player");
                Registry.SetValue(cmpacking + "\\shell\\" + version + "\\command", "", $"\"{PAKPackPath}\" packoradd {version} \"%1\"");
                Registry.SetValue(cmpacking + "\\shell\\" + version, "MultiSelectModel", "Player");
                Registry.SetValue(cmpacking + "\\shell\\" + version + "\\command", "", $"\"{PAKPackPath}\" packoradd {version} \"%1\"");

                Registry.SetValue(folpacking + "\\shell\\" + version, "MultiSelectModel", "Player");
                Registry.SetValue(folpacking + "\\shell\\" + version + "\\command", "", $"\"{PAKPackPath}\" pack \"%1\" {version}");
                Registry.SetValue(folpacking + "\\shell\\" + version, "MultiSelectModel", "Player");
                Registry.SetValue(folpacking + "\\shell\\" + version + "\\command", "", $"\"{PAKPackPath}\" pack \"%1\" {version}");

            }

            Console.WriteLine($"\nPAKPack Context Menu installation complete!");

            Console.WriteLine("\n\n++ Would you like to add PAKPack to the PATH variable? ++\n(If you dont know what this means, say No)\n"
                            + "(y/N)");
            key = Console.ReadKey(true).KeyChar;
            if (key.ToString().ToLower() == "y")
            {
                var pakFolder = new DirectoryInfo(@"PAKPack\").FullName;
                Console.WriteLine($"\nAppending PAKPack to PATH variable...");
                var pathString = Registry.GetValue("HKEY_CURRENT_USER\\Environment", "Path", null);
                if (pathString != null)
                    Registry.SetValue("HKEY_CURRENT_USER\\Environment", "Path", pathString + pakFolder + ";");
                else
                    throw new Exception("Error getting current PATH value.");
                Console.WriteLine("PAKPack has been added to the PATH variable!");

            }

            Console.WriteLine("\n\nInstallation complete! Right click on any file/folder (or multiple!)\nto do multiple PAK actions!");
            Console.ReadKey();
        }
    }
}

