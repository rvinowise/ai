using NUnit.Framework;


namespace rvinowise.ai.unit_tests.sequence_finder {

[TestFixture]
public partial class two_signals_repeat_twice {
    static readonly string[] raw_input = {
        "0", "1", "0", "1",
    };
    static readonly int[][] expected_appearances = {
        new[]{0,1}, 
        new[]{2,3}
    };

    static string[] raw_input_with_noise = {
        ";", "0", ";", ",", "1", ";", "0", ",", ";", "1", ",",
    };
    static int[][] expected_appearances_with_noise = {
        new[]{1,4},
        new[]{6,9}
    };

    private static object[] sequence_cases = {
        new object[] {raw_input, expected_appearances},
        new object[] {raw_input_with_noise, expected_appearances_with_noise},
    };
}


}