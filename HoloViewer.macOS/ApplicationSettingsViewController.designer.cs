// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace HoloViewer.macOS
{
	[Register ("ApplicationSettingsViewController")]
	partial class ApplicationSettingsViewController
	{
		[Outlet]
		AppKit.NSTextField StartupPageUrlTextFiled { get; set; }

		[Action ("ClickedCloseButton:")]
		partial void ClickedCloseButton (AppKit.NSButton sender);

		[Action ("ClickedOKButton:")]
		partial void ClickedOKButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (StartupPageUrlTextFiled != null) {
				StartupPageUrlTextFiled.Dispose ();
				StartupPageUrlTextFiled = null;
			}
		}
	}
}
