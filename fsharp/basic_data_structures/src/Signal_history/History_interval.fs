namespace rvinowise.ai
    open System.Collections.Generic

    type Ensemble = {
        fired: Set<Figure_id> 
    }

    type Ensemble' =
        struct
            val fired: Figure_id[]
        end

    type History_interval = {
        interval: Interval
        fired_ensembles: Map<Moment, Ensemble> 
    }

namespace rvinowise.ai
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Ensemble=
        let ofSeq figures =
            {fired=Set.ofSeq figures}

namespace rvinowise.ai.history
    open Xunit
    open FsUnit
    
    module built =
        open rvinowise.ai

        let from_tuples 
            tail
            ensembles
            =
            {
                interval=Interval.regular tail (tail+uint64(Seq.length(ensembles)))

                fired_ensembles=
                    ensembles
                    |>Seq.mapi (fun (fired_figures: Figure_id seq)=
                        
                    )
            }
        
        [<Fact>]
        let ``from tuples``()=
            from_tuples 0UL [
                ["a";"x"];
                ["b";"y"];
                ["a";"z";"x"];
                ["c"];
                ["b";"x"];
                ["b"];
                ["a"];
                ["c"]
            ]


namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module History_interval =

        let 
