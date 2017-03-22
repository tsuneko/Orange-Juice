using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

using static System.Globalization.NumberStyles;

namespace orangejuicemoney
{
    class Program
    {

        // C# Source written by /id/tsuneko

        // Use at own risk. I'm not responsible for anything.

        // This source does not have any error checking, it has just been written for copy and paste material by leechers.

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

        static int ReadInt(IntPtr handle, IntPtr address)
        {
            byte[] buffer = new byte[4];
            int bytesRead = 0;

            ReadProcessMemory(handle.ToInt32(), address.ToInt32(), buffer, 4, out bytesRead);
            
            if (bytesRead == 4)
            {
                return BitConverter.ToInt32(buffer, 0);
            }

            return (int) -2147483648; // failed to read memory
        }

        static bool WriteInt(IntPtr handle, IntPtr address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            int bytesWritten = 0;

            WriteProcessMemory(handle.ToInt32(), address.ToInt32(), buffer, 4, out bytesWritten);

            if (bytesWritten == 4)
            {
                return true;
            }

            return false; // failed to write memory
        }

        static void PrintCurrentValues(string ProcessName, IntPtr ProcessHandle, IntPtr ProcessBase, Dictionary<string, int> offsets)
        {
            // Print Current Values

            Console.WriteLine("Current Values:");

            foreach(KeyValuePair<string, int> entry in offsets)
            {
                Console.WriteLine("[" + ProcessName + ".exe+" + entry.Value.ToString("X") + "] " + entry.Key + ": " + ReadInt(ProcessHandle, (IntPtr)(ProcessBase.ToInt32() + entry.Value)));
            }
        }

        static void SetValuesToMax(IntPtr ProcessHandle, IntPtr ProcessBase, Dictionary<string, int> offsets)
        {
            WriteInt(ProcessHandle, (IntPtr)(ProcessBase.ToInt32() + offsets["Stars"]), 99999);
            WriteInt(ProcessHandle, (IntPtr)(ProcessBase.ToInt32() + offsets["Fruits"]), 999);
            WriteInt(ProcessHandle, (IntPtr)(ProcessBase.ToInt32() + offsets["Halloween Candy"]), 999);
            WriteInt(ProcessHandle, (IntPtr)(ProcessBase.ToInt32() + offsets["Christmas Candy"]), 9999);
        }

        static void Main(string[] args)
        {

            string ProcessName = "100orange";
            string ProcessVersion = "Steam 1.17.3";

            Console.Title = "999% Orange Juice [" + ProcessVersion + "] ~ Tsuneko";

            Console.WriteLine("Waiting for process: " + ProcessName);

            Process[] processes = Process.GetProcessesByName(ProcessName);

            while (processes.Length == 0) // process does not exist
            {
                Thread.Sleep(200);
                processes = Process.GetProcessesByName(ProcessName);
            }

            Console.Clear();

            Console.WriteLine("Open Shop then press any key to read values.");
            Console.ReadKey();

            Console.Clear();

            IntPtr ProcessHandle = (IntPtr)OpenProcess(0x0008 | 0x0010 | 0x0020, false, processes[0].Id);
            IntPtr ProcessBase = (IntPtr)processes[0].MainModule.BaseAddress.ToInt32();
            
            // Static offsets for 100% Orange Juice Steam 1.17.2 (16/2/17)
            Dictionary<string, int> offsets = new Dictionary<string, int>();
            int starOffset = int.Parse("27D4C8", HexNumber);
            offsets["Stars"] = starOffset;
            offsets["Fruits"] = starOffset + 2744;
            offsets["Halloween Candy"] = starOffset + 2280;
            offsets["Christmas Candy"] = starOffset + 2284;

            Console.Clear();

            PrintCurrentValues(ProcessName, ProcessHandle, ProcessBase, offsets);

            Console.WriteLine("\nIf these values are correct, press [Enter] to get rich");

            ConsoleKey key = Console.ReadKey().Key;
            while (key != ConsoleKey.Enter)
            {
                // Wait for Enter to be pressed, also quit if Escape is pressed

                if (key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                key = Console.ReadKey().Key;
            }

            Console.Clear();

            SetValuesToMax(ProcessHandle, ProcessBase, offsets);
            PrintCurrentValues(ProcessName, ProcessHandle, ProcessBase, offsets);

            Console.WriteLine("\nFreezing Values. Press Ctrl+C to quit.");

            while (Process.GetProcessesByName(ProcessName).Length > 0)
            {
                SetValuesToMax(ProcessHandle, ProcessBase, offsets);
                Thread.Sleep(100);
            }
        }
    }
}
