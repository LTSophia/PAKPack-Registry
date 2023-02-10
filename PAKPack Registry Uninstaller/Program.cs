using Microsoft.Win32;

namespace PAKPack_Registry_Uninstaller
{
    internal class Program
    {
        const string subkeyAsterix = "*";
        const string subkeyFolder = "Folder";
        const string subkeyShell = subkeyAsterix + "\\shell\\PAKPack";
        const string subkeyFolShell = subkeyFolder + "\\shell\\PAKPack";
        const string subkeyCM = subkeyAsterix + "\\CommandMenus";
        const string subkeyFolCM = subkeyFolder + "\\CommandMenus";

        public static void Main()
        {
            Console.WriteLine("This program deletes the registry values set in the installer.\n"
                            + "Continue? (y/N)");
            var key = Console.ReadKey(true).KeyChar;
            if (key.ToString().ToLower() != "y")
                return;

            Registry.ClassesRoot.DeleteSubKeyTree(subkeyShell);
            Registry.ClassesRoot.DeleteSubKeyTree(subkeyFolShell);
            Registry.ClassesRoot.DeleteSubKeyTree(subkeyCM);
            Registry.ClassesRoot.DeleteSubKeyTree(subkeyFolCM);
            Console.WriteLine("\n\nUninstallation complete!");
            Console.ReadKey();
        }
    }
}

