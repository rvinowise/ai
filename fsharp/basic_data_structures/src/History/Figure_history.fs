namespace rvinowise.ai
    open System.Collections.Generic

 
    type Figure_history = {
        figure: Figure_id
        interval: Interval
        appearances: seq<Interval>
    }

    type Action=
    |Tail of Figure_id
    |Head of Figure_id

    module History=

        let add_appearances_to_map 
            history 
            map
            = 
            history.appearances
            |>Seq.iter (fun interval->
                Map.add history.figure interval.tail map
                Map.add history.figure interval.head map
            )

        let combine histories=
            let ensembles: Map<Moment, Action> = Map[]
            histories
            |>Seq.map

namespace rvinowise.ai.history
    open Xunit
    open FsUnit
    open rvinowise.ai
    
    module built =
        

        let from_tuples 
            figure
            tuples
            =
            let intervals = 
                tuples
                |>Seq.map Interval.ofPair
            {
                figure=figure
                appearances=intervals
                interval=Interval.bordering_interval intervals
            }
        
        

    module example=
        let short_history_with_some_repetitions=
            built.from_tuples "F" [
                    10,15;
                    11,16;
                    15,17;
                    20,20
                ]
        
        [<Fact>]
        let ``history interval can start from any moment``()=
            short_history_with_some_repetitions.interval
            |>should equal
                (Interval.regular 10 20)


