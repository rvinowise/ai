namespace rvinowise.ai

open System.Runtime.InteropServices
open System

module Finding_repetitions_cpp =

    [<Literal>]
    //let path_to_dll = "C:/prj/ai/modules/finding_sequences/build/library/bin/finding_sequences"
    let path_to_dll = "C:/prj/ai/modules/finding_sequences/build/library/bin/finding_sequences"
    
    type size_t = uint64

    [<DllImport(
        path_to_dll,
        CallingConvention = CallingConvention.Cdecl
    )>]
    extern int find_repeated_pairs(
        Interval[] heads, 
        int size_heads,
        Interval[] tails, 
        int size_tails,
        [<Out>] Interval[] result
    )

    // [<DllImport(
    //     path_to_dll,
    //     CallingConvention = CallingConvention.Cdecl,
    //     CharSet = CharSet.Ansi
    // )>]
    // extern int find_many_repeated_pairs(
    //     Interval[] appearances, 
    //     int size_appearances,
    //     Interval[] tails, 
    //     int size_tails,
    //     [<Out>] Interval[] result
    // )

    // [<DllImport(
    //     path_to_dll,
    //     CallingConvention = CallingConvention.Cdecl,
    //     CharSet = CharSet.Ansi
    // )>]
    // extern int get_result(
    //     Interval[] heads, 
    //     int size_heads,
    //     Interval[] tails, 
    //     int size_tails,
    //     [<Out>] Interval[] result
    // )