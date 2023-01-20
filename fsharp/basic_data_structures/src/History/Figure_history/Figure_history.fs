namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit
 
    type Figure_history = {
        figure: Figure_id
        appearances: array<Interval>
    }


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure_history=
        open rvinowise.ai
        open FsUnit
        open Xunit


        let figure history =
            history.figure

        let mood_at_moment history moment=
            ()

        let has_repetitions history =
            Seq.length history.appearances > 1

        let start history =
            history.appearances
            |>Array.head 
            |>Interval.start

        let finish history =
            history.appearances
            |>Array.last 
            |>Interval.finish


        
      



     
  


