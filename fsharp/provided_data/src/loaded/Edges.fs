module rvinowise.ai.loaded.figure.Edges

open Dapper

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure


type Edge = {
    parent: Figure_id
    tail: Node_id
    head: Node_id
}

type Subfigure = {
    id: Node_id
    parent: Figure_id
    referenced: string
}

let subfigures figure_id =
    database.Provided.open_connection.Query<Subfigure>(
        @"select * from Subfigure where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )

// let write_subfigures_into_edge 
//     (subfigures: Subfigure seq) 
//     (edge:Edge)  
//     =
//     let tail_subfigure=
//         Seq.find (fun s->s.id=edge.tail) subfigures 
//     let head_subfigure=
//         Seq.find (fun s->s.id=edge.head) subfigures 

//     ai.figure.Edge(
//         ai.figure.Lower_figure(
//             tail_subfigure.id,
//             match tail_subfigure.referenced with
//             |"out" -> Stencil_output
//             | _ -> Lower_figure tail_subfigure.referenced
//         ),
//         ai.figure.Lower_figure(
//             head_subfigure.id,
//             match head_subfigure.referenced with
//             |"out" -> Stencil_output
//             | _ -> Lower_figure head_subfigure.referenced

//         )
//     )

let edges figure_id =
    let db_edges = database.Provided.open_connection.Query<Edge>(
        @"select * from Edge where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )
    let db_subfigures = subfigures figure_id
    //db_edges
    //|>Seq.map (write_subfigures_into_edge db_subfigures)
    ()

