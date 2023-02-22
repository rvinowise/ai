module rvinowise.ai.built.Figure_from_event_batches
    open System
    open FsUnit
    open Xunit
    open System.Collections.Generic

    open rvinowise.ai
    open rvinowise 

    type Built_figure = {
        figure: Figure
        last_vertices: Vertex_id Set
    }

    let add_event_batch_to_figure 
        (built_figure: Built_figure)
        (moment:Moment, batch:Event_batch) 
        =
        let added_edges =
            batch.events
            |>Seq.map (fun event->
                match event with
                |Signal figure ->()
                |Start figure ->()
                |Finish (figure, start_moment) ->()
            )
        {
            figure={
                edges=Set.union built_figure.figure.edges added_edges
                subfigures=Map.union built_figure.figure.subfigures added_edges
            }
            last_vertices=Set.empty
        }

    
    let to_figure (batches:Event_batches) =
        batches
        |>extensions.Map.toPairs
        |>Seq.fold 
            add_event_batch_to_figure
            {
                Built_figure.figure=built.Figure.empty
                last_vertices=Set.empty
            }

    [<Fact>]
    let ``try converting events to_figure``()=
        let expected_figure = built.Figure.simple [
            ("a1","c1");
            ("b1","d1");
            ("c1","d1");
            
        ]
        
        [
            Signal "a";
            Start "b";
        ]
        |>to_figure
        |>should equal expected_figure