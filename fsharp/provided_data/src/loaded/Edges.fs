namespace rvinowise.ai.loaded.figure

open Dapper
open rvinowise.ai


type Edge = {
    parent: Figure_id
    tail: Vertex_id
    head: Vertex_id
}

type Subfigure = {
    id: Vertex_id
    parent: Figure_id
    referenced: string
}

module Edges =

    let subfigures figure_id =
        database.Provided.open_connection.Query<Subfigure>(
            @"select * from Subfigure where Parent = @Figure_id",
                {|figure_id=figure_id|}
        )

    let edges figure_id =
        let db_edges = database.Provided.open_connection.Query<Edge>(
            @"select * from Edge where Parent = @Figure_id",
                {|figure_id=figure_id|}
        )
        let db_subfigures = subfigures figure_id
        //db_edges
        //|>Seq.map (write_subfigures_into_edge db_subfigures)
        ()

