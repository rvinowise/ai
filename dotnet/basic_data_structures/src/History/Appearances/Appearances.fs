module rvinowise.ai.Appearances
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit
    open rvinowise.ai
    open FsUnit
    open Xunit


    let has_repetitions appearances =
        Seq.length appearances > 1

    let start appearances =
        appearances
        |>Array.head 
        |>Interval.start

    let finish appearances =
        appearances
        |>Array.last 
        |>Interval.finish


        
      



     
  


