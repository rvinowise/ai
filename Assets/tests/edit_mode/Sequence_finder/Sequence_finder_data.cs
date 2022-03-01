using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;
using Action_history = rvinowise.ai.unity.Action_history;
using Figure_storage = rvinowise.ai.unity.Figure_storage;
using Network_initialiser = rvinowise.ai.simple.Network_initialiser;

namespace rvinowise.ai.unit_tests.sequence_finder {

[TestFixture]
public partial class two_signals_repeat_twice {
    
    string[] raw_input = {
        "0", "1", "0", "1",
    };
    int[][] expected_appearances = {
        new int[]{0,1}, 
        new int[]{2,3}
    };

    string[] raw_input_with_noise = {
        ";", "0", ";", ",", "1", ";", "0", ",", ";", "1", ",",
    };
    int[][] occurances_with_noise = {
        new int[]{1,4},
        new int[]{6,9}
    };
}


}