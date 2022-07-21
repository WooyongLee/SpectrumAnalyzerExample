namespace CustomSpectrumAnalyzer
{
    public class AppData
    {

    }

    public enum ESettingCommandType
    {
        None = -1,
        Applied,
        ResetMarker,
        ViewRefresh,
    }

    public enum EMarkerCommandType
    {
        None = -1,
        Create = 0,
        Update = 1,
        Clear = 2
    }
}
