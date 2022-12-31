namespace rvinowise.ai

open System.Runtime.InteropServices
open System

module Finding_repetitions_cpp =

    [<Literal>]
    let path_to_dll = "C:/prj/ai/modules/finding_sequences/build/library/bin/finding_sequences"
    
//    [<DllImport(
//        path_to_dll,
//        CallingConvention = CallingConvention.Cdecl
//    )>]
//    extern void init_module(string db_connection)
//    extern void find_repeated_pairs_in_database(string head, string tail)
    
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

    let prepared_array_for_results length: Interval array =
        Array.create length (Interval.moment 0)

    let repeated_pairs 
        (heads: array<Interval>)
        (tails: array<Interval>)
        =
        let mutable repetitions = 
            (heads.Length, tails.Length)
            ||>min
            |>prepared_array_for_results

        let found_amount = find_repeated_pairs(
                heads, heads.Length,
                tails, tails.Length,
                repetitions
            )
        Array.Resize(&repetitions, found_amount)
        repetitions


    //init_module("host=127.0.0.1;port=5432;dbname=ai;user=postgres;password= ;")
