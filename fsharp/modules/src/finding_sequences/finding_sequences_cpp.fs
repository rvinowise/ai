namespace rvinowise.ai

open System.Runtime.InteropServices


module Finding_sequences =

    [<DllImport(
        "C:/prj/ai/modules/finding_sequences/build/Debug/finding_sequences",
         CallingConvention = CallingConvention.Cdecl
    )>]
    extern void init_module(string db_connection)
    extern void find_repeated_pairs(string head, string tail)

    init_module("host=127.0.0.1;port=5432;dbname=ai;user=postgres;password= ;")
