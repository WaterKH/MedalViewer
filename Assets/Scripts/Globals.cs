using SQLite4Unity3d;

public static class Globals
{

    #region Database Table Instance

    public static TableQuery<Medal> MedalsTable;

    #endregion

    #region Settings

    public static ToggleFilterLogic TierFilter;
    public static ToggleFilterLogic PSM_URFilter;
    public static ToggleFilterLogic StarFilter;
    public static SliderFilterLogic MultiplierFilter;

    #endregion
}
