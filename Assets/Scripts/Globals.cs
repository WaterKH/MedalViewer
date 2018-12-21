using System.Collections.Generic;
using SQLite4Unity3d;

public static class Globals
{
    #region Medals

    public static Dictionary<int, Medal> Medals = new Dictionary<int, Medal>();

    #endregion

    #region Database Table Instance

    public static SQLiteConnection Connection;
    public static TableQuery<Medal> MedalsTable;

    #endregion

    #region Settings

    public static ToggleFilterLogic TierFilter;
    public static ToggleFilterLogic PSM_URFilter;
    public static ToggleFilterLogic StarFilter;
    public static SliderFilterLogic MultiplierFilter;

    #endregion
}
