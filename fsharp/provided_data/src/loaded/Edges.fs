module rvinowise.ai.loaded.figure.Edges

open Dapper

open rvinowise
open rvinowise.ai


let edges (figure_id:string) =
    database.Provided.open_connection.Query<ai.figure.Edge>(
        @"select * from Edge where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )

