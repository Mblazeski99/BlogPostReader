using System.ComponentModel;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public enum GridFilterRuleType
    {
        // GENERIC
        [Description("Is equal to")]
        IsEqualTo = 1,

        [Description("Is not equal to")]
        IsNotEqualTo = 2,

        // STRINGS
        [Description("Starts with")]
        StartsWith = 3,

        [Description("Contains")]
        Contains = 4,

        [Description("Does not contain")]
        DoesNotContain = 5,

        [Description("Ends with")]
        EndsWith = 6,

        // NUMBERS, DATE
        [Description("Is greater than or equal to")]
        IsGreaterThanOrEqualTo = 7,

        [Description("Is greater than")]
        IsGreaterThan = 8,

        [Description("Is less than or equal to")]
        IsLessThanOrEqualTo = 9,

        [Description("Is less than")]
        IsLessThan = 10
    }

    public enum GridFilterConditionType
    {
        [Description("And")]
        AND = 1,
        [Description("Or")]
        OR = 2
    }
}
