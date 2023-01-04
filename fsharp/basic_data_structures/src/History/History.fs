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

    type Appearance_event=
    |Start of Figure_id
    |Finish of Figure_id * Moment


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

        let interval history =
            history.interval

        let figure history =
            history.figure

        let add_events_to_map 
            history 
            (map: Map<Moment, Set<Appearance_event> >)
            = 
            history.appearances
            |>Seq.fold (fun map interval->
                map
                |>extensions.Map.add_by_key 
                    interval.start
                    (Start history.figure)
                |>extensions.Map.add_by_key
                    interval.finish
                    (Finish (history.figure, interval.start))
            ) map

        let combine histories =
            histories
            |>Seq.fold (fun map history->
                add_events_to_map history map
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
            |>should equal Map[
                0,[
                    Start "a";
                    Start "b"
                ];
                1,[
                    Finish "a"
                ];
                2,[
                    Start "a";
                    Finish "b"
                ];
                4,[
                    Start "b";
                    Finish "a";
                    Finish "b"
                ]
            ]




        
        
namespace rvinowise.ai.history
    open Xunit
    open FsUnit
    open rvinowise.ai

    module example=
        let short_history_with_some_repetitions=
            built.from_tuples "a" [
                    10,15;
                    11,16;
                    15,17;
                    20,20
                ]
        
        let another_history_for_combining_togetner=
            built.from_tuples "b" [
                    11,12;
                    12,14;
                    13,17;
                    20,24
                ]

        [<Fact>]
        let ``history interval can start from any moment``()=
            short_history_with_some_repetitions.interval
            |>should equal
                (Interval.regular 10 20)


