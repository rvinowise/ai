namespace rvinowise.ai

open System.Runtime.InteropServices


module Finding_sequences =

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
        [<Out>] Interval& result
    )

    [<DllImport(
        path_to_dll,
        CallingConvention = CallingConvention.Cdecl
    )>]
    extern int passing_array(
        int[] heads, 
        int size_heads,
        int[] tails, 
        int size_tails
    )

    //init_module("host=127.0.0.1;port=5432;dbname=ai;user=postgres;password= ;")
