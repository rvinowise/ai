module rvinowise.ai.database.Read

open System.Collections.Generic
open System.Data.SqlClient
open FSharp.Configuration
open Dapper

open rvinowise
open rvinowise.ai
open rvinowise.ai.database.dapper_fsharp
open System.Data
open System.Data.Common;    // for DbProviderFactories
open System.Configuration

type settings = FSharp.Configuration.AppSettings<"app.config">


let appearances_of figure_id: Figure_appearance list =
    
    

    let result = database.Provided.open_connection.Query<ai.Figure_appearance>(
        @"select * from Figure_appearance where figure = @Figure_id",
        {|figure_id=figure_id|}
    )
    //db_connection.Close()
    Seq.toList result
    //sql_uri
    //|> Sql.connect
    //|> Sql.query "SELECT * FROM Figure_appearance where figure=@figure_id"
    //|> Sql.parameters [ "@figure_id", Sql.text figure_id ]
    //|> Sql.execute (
    //    fun row -> {
    //        start=row.int "start"
    //        ending=row.int "ending"
    //    }    
    //)

let internal_structure_of
    figure_id
    sql_uri
    (loaded_figures:Dictionary<string, Figure>):
    Edge list
    =
    []
//    sql_uri
//    |> Sql.connect
//    |> Sql.query "select * from Edge where parent=@figure_id"
//    |> Sql.parameters [ "@figure_id", Sql.text figure_id ]
//    |> Sql.execute (
//        fun row -> {
//            start = provided.Figure.with_id (row.string "start") loaded_figures
//            ending = provided.Figure.with_id (row.string "ending") loaded_figures
//        }    
//    )
    