using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace map2agbgui.Native
{
    class WindowMods
    {

        #region Imports

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Constants

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;

        #endregion

        #region Methods

        /// <summary>
        /// Removes the maximize box of a window
        /// </summary>
        /// <param name="target">The target window</param>
        public static void RemoveMaximizeBox(Window target)
        {
            var hwnd = new WindowInteropHelper(target).Handle;
            var extendedStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, extendedStyle & ~WS_MAXIMIZEBOX);
        }

        /// <summary>
        /// Removes the minimize box of a window
        /// </summary>
        /// <param name="target">The target window</param>
        public static void RemoveMinimizeBox(Window target)
        {
            var hwnd = new WindowInteropHelper(target).Handle;
            var extendedStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, extendedStyle & ~WS_MINIMIZEBOX);
        }

        /// <summary>
        /// Removes the icon of a window
        /// </summary>
        /// <param name="target">The target window</param>
        public static void RemoveIcon(Window target)
        {
            var hwnd = new WindowInteropHelper(target).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
        }

        #endregion

    }
}
