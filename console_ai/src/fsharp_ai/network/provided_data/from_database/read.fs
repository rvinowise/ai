module rvinowise.ai.database.Read

open System.Collections.Generic
open Npgsql.FSharp

open rvinowise.ai

let appearances_of figure_id sql_uri : Figure_appearance list =
    sql_uri
    |> Sql.connect
    |> Sql.query "SELECT * FROM Figure_appearance where figure=@figure_id"
    |> Sql.parameters [ "@figure_id", Sql.text figure_id ]
    |> Sql.execute (
        fun row -> {
            start=row.int "start"
            ending=row.int "ending"
        }    
    )

let internal_structure_of
    figure_id
    sql_uri
    (loaded_figures:Dictionary<string, Figure>):
    Edge list
    =
    sql_uri
    |> Sql.connect
    |> Sql.query "select * from Edge where parent=@figure_id"
    |> Sql.parameters [ "@figure_id", Sql.text figure_id ]
    |> Sql.execute (
        fun row -> {
            start = provided.Figure.with_id (row.string "start") loaded_figures
            ending = provided.Figure.with_id (row.string "ending") loaded_figures
        }    
    )
    
