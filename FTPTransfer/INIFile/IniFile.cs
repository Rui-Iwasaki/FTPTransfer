using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FTPTransfer
{   
    public class IniFilePara
    {
        public const string INI_FILE_PATH = @"\ParaSetting.ini";
    }

    static class IniFile
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName);


        public static string GetValueString(string section, string key)
        {
            string path = Directory.GetCurrentDirectory();
            var sb = new StringBuilder(1024);

            path += IniFilePara.INI_FILE_PATH;

            GetPrivateProfileString(section, key, "", sb, Convert.ToUInt32(sb.Capacity), path);
            return sb.ToString();
        }

        public static int GetValueInt(string section, string key)
        {
            string path = Directory.GetCurrentDirectory();
            var sb = new StringBuilder(1024);

            path += IniFilePara.INI_FILE_PATH;

            return (int)GetPrivateProfileInt(section, key, 0, path);
        }


    }
}
