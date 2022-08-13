module rvinowise.ai.database.read

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

let internal_structure_of figure_id sql_uri: Edge list =
    sql_uri
    |> Sql.connect
    |> Sql.query "select * from Edge where parent=@figure_id"
    |> Sql.parameters [ "@figure_id", Sql.text figure_id ]
    |> Sql.execute (
        fun row -> {
            start=row.string "start"
            ending=row.string "ending"
        }    
    )
