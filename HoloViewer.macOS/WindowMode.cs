using AppKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoloViewer.macOS
{
    public class WindowMode : IWindowMode
    {
        public void FullScreen ()
        {
            NSApplication.SharedApplication.MainWindow.CollectionBehavior |= NSWindowCollectionBehavior.FullScreenPrimary;
            NSApplication.SharedApplication.MainWindow.ToggleFullScreen(NSApplication.SharedApplication.MainWindow);
        }

        void IWindowMode.WindowMode ()
        {
            NSApplication.SharedApplication.MainWindow.CollectionBehavior ^= NSWindowCollectionBehavior.FullScreenPrimary;
            NSApplication.SharedApplication.MainWindow.ToggleFullScreen(NSApplication.SharedApplication.MainWindow);
        }
    }
}
