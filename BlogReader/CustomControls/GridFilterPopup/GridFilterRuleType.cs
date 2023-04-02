using System.ComponentModel;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public enum GridFilterRuleType
    {
        [Description("Starts with")]
        StartsWith = 0,

        [Description("Contains")]
        Contains = 1,

        [Description("Not contains")]
        NotContains = 2,

        [Description("Ends with")]
        EndsWith = 3,

        [Description("Equals")]
        Equals = 4,

        [Description("Not equals")]
        NotEquals = 5
    }
}
