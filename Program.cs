using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using static System.Globalization.NumberStyles;

namespace _999percent
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        internal struct MEMORY_BASIC_INFORMATION
        {
            public static UInt32 Size = (UInt32)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION));

            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        class OJProcess
        {
            public string Name;
            public string Version;
            public string Date;
            public IntPtr Handle;
            public IntPtr Base;
        }

        class Signature
        {
            public byte[] Bytes;
            public string Mask;
            public int Length;
            public int ByteOffset;
            public int MaxValue;
            public IntPtr PointerAddress;
            public IntPtr Address;
        }

        class StaticOffset
        {
            public int Offset;
            public int MaxValue;
        }

        static bool TokenizeSignature(string s, out Signature signature)
        {
            signature = new Signature();

            string[] tokens = s.Split(',');

            int NumberChars = tokens[0].Length;
            signature.Bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                signature.Bytes[i / 2] = Convert.ToByte(tokens[0].Substring(i, 2), 16);
            }

            signature.Mask = tokens[1];

            if (signature.Bytes.Length != signature.Mask.Length)
            {
                return false;
            }

            signature.Length = signature.Bytes.Length;

            signature.ByteOffset = 0;
            try
            {
                signature.ByteOffset = Convert.ToInt32(tokens[2]);
            }
            catch
            {
                return false;
            }

            signature.MaxValue = 0;
            try
            {
                signature.MaxValue = Convert.ToInt32(tokens[3]);
            }
            catch
            {
                return false;
            }

            return true;
        }

        static bool TokenizeStaticOffset(string s, out StaticOffset staticoffset)
        {
            staticoffset = new StaticOffset();

            string[] tokens = s.Split(',');

            staticoffset.Offset = 0;
            try
            {
                staticoffset.Offset = int.Parse(tokens[0], HexNumber);
            }
            catch
            {
                return false;
            }

            staticoffset.MaxValue = 0;
            try
            {
                staticoffset.MaxValue = Convert.ToInt32(tokens[1]);
            }
            catch
            {
                return false;
            }

            return true;
        }

        static bool ReadInt(IntPtr handle, IntPtr address, out int n)
        {
            byte[] buffer = new byte[4];
            int bytesRead = 0;

            ReadProcessMemory(handle, address, buffer, 4, out bytesRead);

            if (bytesRead == 4)
            {
                n = BitConverter.ToInt32(buffer, 0);
                return true;
            }

            n = 0;
            return false;
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

            return false;
        }

        static bool SignatureScan(OJProcess process, Signature signature)
        {
            bool canFind = true;
            MEMORY_BASIC_INFORMATION info;

            int MEM_COMMIT = 0x1000;
            int MEM_MAPPED = 0x40000;
            int MEM_PRIVATE = 0x20000;
            int MEM_IMAGE = 0x1000000;

            for (long currentAddress = (long)process.Base; VirtualQueryEx(process.Handle, (IntPtr)currentAddress, out info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) == 28; currentAddress += (long)info.RegionSize)
            {
                if (info.State == MEM_COMMIT && (info.Type == MEM_MAPPED || info.Type == MEM_PRIVATE || info.Type == MEM_IMAGE))
                {
                    int bytesRead = 0;
                    byte[] dump = new byte[(int)info.RegionSize];
                    ReadProcessMemory(process.Handle, (IntPtr)currentAddress, dump, (int)info.RegionSize, out bytesRead);
                    Array.Resize(ref dump, bytesRead);

                    if (bytesRead > 0)
                    {
                        int currentOffset = 0;
                        while (currentOffset < bytesRead - signature.Length)
                        {
                            canFind = true;
                            for (int i = 0; i < signature.Length - 1; i++)
                            {
                                if (signature.Mask[i] == 'x' && dump[currentOffset + i] != signature.Bytes[i])
                                {
                                    canFind = false;
                                    break;
                                }
                            }

                            if (canFind)
                            {
                                signature.PointerAddress = (IntPtr)currentAddress + currentOffset + signature.ByteOffset;
                                int offset;
                                if (ReadInt(process.Handle, signature.PointerAddress, out offset))
                                {
                                    signature.Address = (IntPtr)offset;
                                    return true;
                                }
                                return false;
                            }
                            currentOffset++;
                        }
                    }
                }
            }

            return false;
        }

        static void PrintCurrentValues(OJProcess process, Dictionary<string, Signature> signatures, Dictionary<string, StaticOffset> staticoffsets, int processbase)
        {
            foreach (string key in signatures.Keys)
            {
                int value;
                ReadInt(process.Handle, signatures[key].Address, out value);
                Console.WriteLine(key + ": " + value.ToString());
            }

            foreach (string key in staticoffsets.Keys)
            {
                int value;
                ReadInt(process.Handle, (IntPtr)(staticoffsets[key].Offset + processbase), out value);
                Console.WriteLine(key + ": " + value.ToString());
            }
        }

        static void SetValuesToMax(OJProcess process, Dictionary<string, Signature> signatures, Dictionary<string, StaticOffset> staticoffsets, int processbase)
        {
            foreach (string key in signatures.Keys)
            {
                WriteInt(process.Handle, signatures[key].Address, signatures[key].MaxValue);
            }

            foreach (string key in staticoffsets.Keys)
            {
                WriteInt(process.Handle, (IntPtr)(staticoffsets[key].Offset + processbase), staticoffsets[key].MaxValue);
            }
        }

        static void Error(string log)
        {
            Console.WriteLine("Error: " + log);
            Console.ReadKey();
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            Console.Title = "999% Orange Juice ~ Tsuneko";

            if (!File.Exists("conf.ini"))
            {
                Error("conf.ini file does not exist!");
            }

            OJProcess OrangeJuice = new OJProcess();
            OrangeJuice.Name = "";
            OrangeJuice.Version = "Unknown Version";
            OrangeJuice.Date = "";

            Dictionary<string, Signature> signatures = new Dictionary<string, Signature>();
            Dictionary<string, StaticOffset> staticoffsets = new Dictionary<string, StaticOffset>();

            string[] lines = File.ReadAllLines("conf.ini");
            string currentSection = "process";
            foreach (string l in lines)
            {
                string line = string.Join("", l.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); // remove all whitespace
                if (line.Length > 2)
                {
                    if (line[0] == ';')
                    {
                        continue;
                    }
                    if (line[0] == '[' && line[line.Length - 1] == ']')
                    {
                        currentSection = line.Substring(1, line.Length - 2);
                    }
                    else if (line.Contains('=') && line.IndexOf('=') > 0 && line.IndexOf('=') < line.Length - 1)
                    {
                        string key = line.Substring(0, line.IndexOf('='));
                        string value = line.Substring(line.IndexOf('=') + 1, line.Length - line.IndexOf('=') - 1);
                        if (currentSection == "process")
                        {
                            switch (key)
                            {
                                case "name":
                                    OrangeJuice.Name = value;
                                    break;
                                case "version":
                                    OrangeJuice.Version = value;
                                    break;
                                case "date":
                                    OrangeJuice.Date = value;
                                    break;
                            }
                        }
                        else if (currentSection == "signatures")
                        {
                            Signature signature;
                            if (TokenizeSignature(value, out signature))
                            {
                                signatures[key] = signature;
                            }
                        }

                        else if (currentSection == "staticoffsets")
                        {
                            StaticOffset staticoffset;
                            if (TokenizeStaticOffset(value, out staticoffset))
                            {
                                staticoffsets[key] = staticoffset;
                            }
                        }
                    }
                }
            }

            // Check process configuration values
            if (OrangeJuice.Name == "")
            {
                Error("conf.ini does not contain process name");
            }
            Console.Title = "999% Orange Juice [" + OrangeJuice.Version + "] ~ Tsuneko " + OrangeJuice.Date;

            // Wait for process
            Console.WriteLine("Waiting for process: " + OrangeJuice.Name + ".exe");

            Process[] processes = Process.GetProcessesByName(OrangeJuice.Name);
            while (processes.Length == 0) // process does not exist
            {
                Thread.Sleep(200);
                processes = Process.GetProcessesByName(OrangeJuice.Name);
            }
            Console.Clear();

            // Get handle to process
            int PROCESS_QUERY_INFORMATION = 0x0400;
            int PROCESS_VM_OPERATION = 0x0008;
            int PROCESS_VM_READ = 0x0010;
            int PROCESS_VM_WRITE = 0x0020;
            OrangeJuice.Handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, processes[0].Id);
            OrangeJuice.Base = (IntPtr)processes[0].MainModule.BaseAddress.ToInt32();

            Console.WriteLine("Signatures:");
            foreach (string key in signatures.Keys)
            {
                if (SignatureScan(OrangeJuice, signatures[key]))
                {
                    Console.WriteLine(key + " - [" + OrangeJuice.Name + ".exe]+" + ((long)signatures[key].Address - (long)OrangeJuice.Base).ToString("X"));
                }
                else
                {
                    signatures.Remove(key);
                }
            }

            if (signatures.Keys.Count == 0)
            {
                Error("No valid signatures");
            }
            Console.WriteLine();

            if (staticoffsets.Keys.Count > 0)
            {
                Console.WriteLine("Static Offsets:");
                foreach (string key in staticoffsets.Keys)
                {
                    Console.WriteLine(key + " - [" + OrangeJuice.Name + ".exe]+" + (staticoffsets[key].Offset).ToString("X"));
                }
                Console.WriteLine();
            }

            PrintCurrentValues(OrangeJuice, signatures, staticoffsets, OrangeJuice.Base.ToInt32());
            Console.WriteLine("\nIf these values are correct, press [Enter] to steal many stars");
            ConsoleKey k = Console.ReadKey().Key;
            while (k != ConsoleKey.Enter)
            {
                if (k == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                k = Console.ReadKey().Key;
            }
            Console.Clear();
            Console.WriteLine("Freezing Values. Press Ctrl+C to quit.\n");

            SetValuesToMax(OrangeJuice, signatures, staticoffsets, OrangeJuice.Base.ToInt32());
            PrintCurrentValues(OrangeJuice, signatures, staticoffsets, OrangeJuice.Base.ToInt32());

            while (Process.GetProcessesByName(OrangeJuice.Name).Length > 0)
            {
                SetValuesToMax(OrangeJuice, signatures, staticoffsets, OrangeJuice.Base.ToInt32());
                Thread.Sleep(1000);
            }

            Environment.Exit(0);
        }
    }
}
