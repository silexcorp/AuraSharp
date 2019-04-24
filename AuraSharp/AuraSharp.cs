using System;
using System.Runtime.InteropServices;

namespace AuraSharp
{
    public class Aura
    {
        public int[] controllers;

        /*
         * Constructor functions
         */
        public Aura()
        {
            controllers = AuraSDK.GetMotherboardControllers();
        }

        /*
         * Public functions
         */
        public void setAll(byte red, byte green, byte blue)
        {
            byte[] colors = { red, green, blue };

            foreach (int handle in controllers)
            {
                AuraSDK.SetMotherboardApplicationMode(handle);
                AuraSDK.SetMotherboardColors(handle, colors);
            }
        }

        public void setAll(byte[] color)
        {
            byte[] colors = { color[0], color[1], color[2] };

            foreach (int handle in controllers)
            {
                AuraSDK.SetMotherboardApplicationMode(handle);
                AuraSDK.SetMotherboardColors(handle, colors);
            }
        }

        public void setController(int handle, byte[] colors)
        {
            AuraSDK.SetMotherboardApplicationMode(handle);
            AuraSDK.SetMotherboardColors(handle, colors);
        }

        public byte[] getController(int handle)
        {
            byte[] all_colors = AuraSDK.GetMotherboardColors(handle);
            if (all_colors.Length >= 3)
            {
                return new byte[] { all_colors[0], all_colors[1], all_colors[2] };
            }
            else
            {
                return new byte[] { 0, 0, 0 };
            }

        }

        public void Reset()
        {
            foreach (int handle in controllers)
                AuraSDK.SetMotherboardDefaultMode(handle);
        }

        /*
         * 
         * Future functions
         * 
         */
        public void Hex2RGB(string hex)
        {
            int color = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);

            Console.WriteLine(color);
        }

    }

    public class AuraColor
    {
        public static byte[] black = { 0, 0, 0 };
        public static byte[] white = { 255, 255, 255 };

        public static byte[] red = { 255, 0, 0 };
        public static byte[] green = { 0, 255, 0 };
        public static byte[] blue = { 0, 0, 255 };

        public static byte[] yellow = { 255, 255, 0 };
        public static byte[] cyan = { 0, 255, 255 };
        public static byte[] magenta = { 255, 0, 255 };

        public static byte[] silver = { 192, 192, 192 };
        public static byte[] gray = { 128, 128, 128 };
    }

    class AuraSDK
    {
        /*
         * Import DLL
         */
        [DllImport("AURA_SDK.dll")]
        internal static extern int EnumerateMbController(IntPtr handles, int size);

        [DllImport("AURA_SDK.dll")]
        internal static extern int SetMbMode(IntPtr handle, int mode);

        [DllImport("AURA_SDK.dll")]
        internal static extern int GetMbLedCount(IntPtr handle);

        [DllImport("AURA_SDK.dll")]
        internal static extern int GetMbColor(IntPtr handle, IntPtr color, int size);

        [DllImport("AURA_SDK.dll")]
        internal static extern int SetMbColor(IntPtr handle, IntPtr color, int size);

        public static int[] GetMotherboardControllers()
        {
            int count = EnumerateMbController(IntPtr.Zero, 0);
            // TODO error handling
            int[] ret = new int[count];

            IntPtr handles = Marshal.AllocHGlobal(count * IntPtr.Size);
            int err = EnumerateMbController(handles, count);
            // TODO error handling
            Marshal.Copy(handles, ret, 0, count);
            Marshal.FreeHGlobal(handles);

            //EventLog.WriteEntry(LOG_SOURCE, "GetMotherboardControllers found " + count, EventLogEntryType.Information);
            return ret;
        }

        public static void SetMotherboardApplicationMode(int handle)
        {
            // TODO error handling
            SetMbMode(new IntPtr(handle), 1);
        }

        public static void SetMotherboardDefaultMode(int handle)
        {
            // TODO error handling
            SetMbMode(new IntPtr(handle), 0);
        }

        public static int GetMotherboardLedCount(int handle)
        {
            // TODO error handling? says it returns 0 on error, but that's effectively a no-op success
            return GetMbLedCount(new IntPtr(handle));
        }

        public static byte[] GetMotherboardColors(int handle)
        {
            int ledCount = GetMotherboardLedCount(handle);

            // 3 bytes per LED for color
            IntPtr p = Marshal.AllocHGlobal(ledCount * 3);
            // TODO error handling
            GetMbColor(new IntPtr(handle), p, ledCount);
            byte[] get_colors = new byte[ledCount * 3];
            Marshal.Copy(p, get_colors, 0, ledCount * 3);
            Marshal.FreeHGlobal(p);
            byte[] bytes = get_colors;
            return get_colors;
        }

        public static void SetMotherboardColors(int handle, byte[] colors)
        {

            int ledCount = GetMotherboardLedCount(handle);
            byte[] bytes = new byte[ledCount * 3];
            for (int i = 0; i < bytes.Length; i += 3)
            {
                bytes[i] = colors[0];
                bytes[i + 1] = colors[1];
                bytes[i + 2] = colors[2];

            }

            IntPtr p = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, p, bytes.Length);
            SetMbColor(new IntPtr(handle), p, bytes.Length);
            Marshal.FreeHGlobal(p);

        }
    }
}
