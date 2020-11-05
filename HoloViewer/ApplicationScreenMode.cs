namespace HoloViewer
{
    public class ApplicationScreenMode
    {
        public enum ScreenMode
        {
            Single,
            SplitHorizontal2,
            SplitVertical2,
            SplitHorizontal3,
            SplitVertical3,
            SplitCustom3_1,
            SplitCustom3_2,
            SplitHorizontal4,
            SplitVertical4,
            SplitCustom4,
        }

        public ScreenMode CurrentScreenMode { get; private set; }

        public void ChangeScreenMode (ScreenMode screenMode)
        {
            CurrentScreenMode = screenMode;
        }
    }
}
