module rvinowise.ai.built.Figure_from_event_batches
    open System
    open FsUnit
    open Xunit
    open System.Collections.Generic

    open rvinowise.ai
    open rvinowise 


    let add_event_batch_to_figure 
        (built_figure: Figure)
        (batch:Event_batch) 
        =
        batch.events
        |>Seq.map (fun event->
            match event with
            |Signal figure ->()
            |Start figure ->()
            |Finish (figure, start_moment) ->()
        )

    
    let to_figure (batches:Event_batches) =
        batches
        |>extensions.Map.toPairs
        |>Seq.fold