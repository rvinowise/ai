namespace rvinowise.ai

open System
open System.Runtime.InteropServices

open rvinowise
open rvinowise.ai

module Finding_sequences =

    [<DllImport(
        "C:/prj/ai/modules/finding_sequences/build/Debug/finding_sequences",
         CallingConvention = CallingConvention.Cdecl)>]
    extern void find_repeated_pairs(string db_connection)

    //let find_repeated_pairs str =
    //    printfn str