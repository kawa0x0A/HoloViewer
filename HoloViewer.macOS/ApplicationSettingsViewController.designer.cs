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
		AppKit.NSTextField CaptureSavePathTextField { get; set; }

		[Outlet]
		AppKit.NSButton InsertTweetHoloViewerHashTag { get; set; }

		[Outlet]
		AppKit.NSButton InsertTweetYoutubeTag { get; set; }

		[Outlet]
		AppKit.NSTextField StartupPageUrlTextFiled { get; set; }

		[Outlet]
		AppKit.NSButton UpdateCheckCheckBox { get; set; }

		[Action ("ClickedCaptureSavePathButton:")]
		partial void ClickedCaptureSavePathButton (AppKit.NSButton sender);

		[Action ("ClickedCloseButton:")]
		partial void ClickedCloseButton (AppKit.NSButton sender);

		[Action ("ClickedOKButton:")]
		partial void ClickedOKButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CaptureSavePathTextField != null) {
				CaptureSavePathTextField.Dispose ();
				CaptureSavePathTextField = null;
			}

			if (StartupPageUrlTextFiled != null) {
				StartupPageUrlTextFiled.Dispose ();
				StartupPageUrlTextFiled = null;
			}

			if (InsertTweetYoutubeTag != null) {
				InsertTweetYoutubeTag.Dispose ();
				InsertTweetYoutubeTag = null;
			}

			if (InsertTweetHoloViewerHashTag != null) {
				InsertTweetHoloViewerHashTag.Dispose ();
				InsertTweetHoloViewerHashTag = null;
			}

			if (UpdateCheckCheckBox != null) {
				UpdateCheckCheckBox.Dispose ();
				UpdateCheckCheckBox = null;
			}
		}
	}
}
