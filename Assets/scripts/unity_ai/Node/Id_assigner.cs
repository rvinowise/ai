using System.Numerics;
using UnityEngine;

namespace rvinowise.ai.unity {

public class Id_assigner
{
    static BigInteger last_id;

    public static string get_next_id() {
        string string_id = last_id.ToString();
        last_id++;
        return string_id;
    }
}
}