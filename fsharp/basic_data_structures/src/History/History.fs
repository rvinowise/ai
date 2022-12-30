namespace rvinowise.ai
    open System.Collections.Generic

    type Ensemble = {
        fired: Set<Figure_id> 
    }

    type History = {
        interval: Interval
        ensembles: Map<Moment, Ensemble> 
    }

namespace rvinowise.ai
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Ensemble=
        let ofSeq figures =
            {fired=Set.ofSeq figures}

namespace rvinowise.ai.history
    open Xunit
    open FsUnit
    open rvinowise.ai
    
    module built =
        

        let from_tuples 
            tail
            ensembles
            =
            {
                interval=Interval.regular tail (tail+uint64(Seq.length(ensembles))-1UL)

                ensembles=
                    ensembles
                    |>Seq.mapi (fun index (fired_figures: Figure_id seq)->
                        (
                            tail+uint64(index),
                            Ensemble.ofSeq fired_figures
                        )
                    )|>Map.ofSeq
            }
        
        

    module example=
        let short_history_with_some_repetitions=
            built.from_tuples 0UL [
                    ["a";"x"];
                    ["b";"y"];
                    ["a";"z";"x"];
                    ["c"];
                    ["b";"x"];
                    ["b"];
                    ["a"];
                    ["c"]
                ]
        
        [<Fact>]
        let ``history interval can start from any moment``()=
            let history = 
                built.from_tuples 0UL [
                    ["a";"x"];
                    ["b";"y"];
                    ["a";"z";"x"];
                    ["c"];
                    ["b";"x"];
                    ["b"];
                    ["a"];
                    ["c"]
                ]
            history.interval
            |>should equal
                (Interval.regular 0UL 7UL)

namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module History =
        ()
