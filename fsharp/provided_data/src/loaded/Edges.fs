module rvinowise.ai.loaded.figure.Edges

open System.Collections.Generic
open System.Data.SqlClient
open Dapper

open rvinowise
open rvinowise.ai
open System.Data
open System.Data.Common;    // for DbProviderFactories
open System.Configuration



type Edge = {
    parent: Figure_id
    tail: Subfigure_id
    head: Subfigure_id
}

type Subfigure = {
    id: Subfigure_id
    parent: Figure_id
    referenced: Figure_id
}

let subfigures figure_id =
    database.Provided.open_connection.Query<Subfigure>(
        @"select * from Subfigure where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )

let write_subfigures_into_edge 
    (subfigures: Subfigure seq) 
    (edge:Edge)  
    =
    let tail_subfigure=
        Seq.find (fun s->s.id=edge.tail) subfigures 
    let head_subfigure=
        Seq.find (fun s->s.id=edge.head) subfigures 

    new ai.figure.Edge(
        new ai.figure.Subfigure(
            tail_subfigure.id,
            tail_subfigure.referenced
        ),
        new ai.figure.Subfigure(
            head_subfigure.id,
            head_subfigure.referenced
        )
    )

let edges figure_id =
    let db_edges = database.Provided.open_connection.Query<Edge>(
        @"select * from Edge where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )
    let db_subfigures = subfigures figure_id
    db_edges
    |>Seq.map (write_subfigures_into_edge db_subfigures)

