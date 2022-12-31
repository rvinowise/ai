namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit
 
    type Figure_history = {
        figure: Figure_id
        interval: Interval
        appearances: seq<Interval>
    }

    type Action=
    |Tail of Figure_id
    |Head of Figure_id


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

namespace rvinowise.ai
    module History=
        open rvinowise
        open FsUnit
        open Xunit

        let add_appearances_to_map 
            history 
            map
            = 
            history.appearances
            |>Seq.fold (fun map interval->
                map
                |>extensions.Map.add_by_key interval.tail history.figure 
                |>extensions.Map.add_by_key interval.head history.figure 
            ) map

        let combine histories =
            histories
            |>Seq.fold (fun map history->
                add_appearances_to_map history map
            ) Map.empty

        [<Fact>]
        let ``combine figure histories``()=
            let history_of_a = history.built.from_tuples "a" [
                0,1; 2,4
            ]
            let history_of_b = history.built.from_tuples "b" [
                0,2; 4,4
            ]
            [history_of_a; history_of_b]
            |>combine 
            |>should equal




        
        
namespace rvinowise.ai.history
    open Xunit
    open FsUnit
    open rvinowise.ai

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


