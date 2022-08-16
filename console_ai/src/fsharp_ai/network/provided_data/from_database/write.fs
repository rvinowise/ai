module rvinowise.ai.database.Write

open System.Collections.Generic
open System
open System.Data
open System.Data.SqlClient
open FSharp.Configuration
open Dapper.FSharp
open Dapper.FSharp.PostgreSQL

open rvinowise.ai
open rvinowise.ai.database.dapper_fsharp


type settings = AppSettings<"app.config">


let appearances_of figure =
    
    
    //let db_connection:IDbConnection  = new SqlConnection(
    //    settings.ConnectionStrings.SqliteConnectionString
    //)

    //let figure_appearance = table<Figure_appearance>

    //insert {
    //    into figure_appearance
    //    value figure
    //} |> db_connection.InsertAsync

    ()
    