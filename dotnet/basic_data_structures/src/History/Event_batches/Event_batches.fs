namespace rvinowise.ai
    open FsUnit
    open Xunit


    open rvinowise
    open rvinowise.extensions
    open rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Event_batches =
        
        let interval (history:Event_batches) =
            let moments = 
                history
                |>Seq.map extensions.KeyValuePair.key
            (
                moments|>Seq.min,
                moments|>Seq.max
            )|>Interval.ofPair


        let get_mood_history (event_batches: Event_batches) =
            event_batches
            |>Seq.map (fun moment_batch->
                (moment_batch.Key, moment_batch.Value.mood)
            )
            |>Map.ofSeq


        