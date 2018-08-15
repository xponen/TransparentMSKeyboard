using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MSTouchKeyboardTransparent
{
    static class Program
    {
        //reference: 
        //https://www.dotnetperls.com/main
        //https://stackoverflow.com/questions/28719056/operator-cannot-be-applied-to-operands-of-type-intptr-and-int-net-3-5
        //https://www.codeproject.com/Questions/260769/Convert-System-IntPtr-to-System-Runtime-InteropSer
        //https://www.pinvoke.net/default.aspx/user32.setwindowlong
        //https://stackoverflow.com/questions/1360758/modifying-opacity-of-any-window-from-c-sharp
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            uint alphaValue = 128;
            if (args == null || args.Length==0)
            {
                
            }
            else{
                String argument = args[0];
                if (!UInt32.TryParse(argument, out alphaValue))
                    alphaValue = 128;
                alphaValue = Math.Min(Math.Max(1, alphaValue), 255);
            }

            IntPtr KeyboardWnd = GetHWND();

            // Remove WS_EX_LAYERED from this window styles
            NativeMethods.SetWindowLongPtr(new HandleRef(null, KeyboardWnd), NativeMethods.GWL_EXSTYLE, (IntPtr)((int)(NativeMethods.GetWindowLongPtr(KeyboardWnd, NativeMethods.GWL_EXSTYLE)) & ~NativeMethods.WS_EX_LAYERED));
            // Ask the window and its children to repaint
            NativeMethods.RedrawWindow(KeyboardWnd, IntPtr.Zero, IntPtr.Zero, NativeMethods.RedrawWindowFlags.Erase | NativeMethods.RedrawWindowFlags.Invalidate | NativeMethods.RedrawWindowFlags.Frame | NativeMethods.RedrawWindowFlags.AllChildren);

            NativeMethods.SetWindowLongPtr(new HandleRef(null, KeyboardWnd), NativeMethods.GWL_EXSTYLE, (IntPtr)((int)(NativeMethods.GetWindowLongPtr(KeyboardWnd, NativeMethods.GWL_EXSTYLE)) | NativeMethods.WS_EX_LAYERED));
            NativeMethods.SetLayeredWindowAttributes(KeyboardWnd, 0, (byte)alphaValue, NativeMethods.LWA_ALPHA);
        }

        private static IntPtr GetHWND()
        {
            IntPtr hParent = IntPtr.Zero;
            int count1 = 0;
            while (true & count1<10)
            {
                hParent = NativeMethods.FindWindowEx(IntPtr.Zero, hParent, "ApplicationFrameWindow", "");
                if (hParent == IntPtr.Zero) break;
                IntPtr currChild = NativeMethods.FindWindowEx(hParent, IntPtr.Zero, "Windows.UI.Core.CoreWindow", "Microsoft Text Input Application");
                if (currChild != IntPtr.Zero) return currChild;
                count1++;
            }

            return IntPtr.Zero;
        }
    }
}
