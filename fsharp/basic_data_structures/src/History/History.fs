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


