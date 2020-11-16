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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField RepositoryPageUrlLabel { get; set; }

		[Outlet]
		AppKit.NSTextField TwitterProfilePageUrlLabel { get; set; }

		[Action ("ClickedCloseButton:")]
		partial void ClickedCloseButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TwitterProfilePageUrlLabel != null) {
				TwitterProfilePageUrlLabel.Dispose ();
				TwitterProfilePageUrlLabel = null;
			}

			if (RepositoryPageUrlLabel != null) {
				RepositoryPageUrlLabel.Dispose ();
				RepositoryPageUrlLabel = null;
			}
		}
	}
}
