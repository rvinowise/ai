namespace rvinowise.ai

open System
open System.Runtime.InteropServices

open rvinowise
open rvinowise.ai

module Finding_sequences =

   [<DllImport(
       "C:/prj/ai/modules/finding_sequences/build/bin/finding_sequences",
        CallingConvention = CallingConvention.Cdecl)>]
   extern void find_repeated_pairs(string db_connection, int a, float b)

    // let find_repeated_pairs (str:string, a, b)=
    //     printfn $"%s{str}"