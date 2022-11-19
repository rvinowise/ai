module rvinowise.ai.database.Write

open Dapper
open System.Collections.Generic
open System
open System.Data
open System.Data.SqlClient
open Dapper.FSharp
open Dapper.FSharp.PostgreSQL

open rvinowise
open rvinowise.ai


let new_signal id =
    database.Provided.open_connection.Query<ai.figure.Figure>(
        @"insert into Figure (id)
        values (@id)",
        {|id=id|}
    ) |> ignore

let figure_appearance 
    figure
    (appearance:ai.figure.Appearance) 
    =
    database.Provided.open_connection.Query<ai.figure.Figure>(
        @"insert into figure_appearance (figure, head, tail)
        values (@figure, @head, @tail)",
        {|
            figure = figure; 
            head = appearance.interval.head; 
            tail = appearance.interval.tail
        |}
    ) |> ignore

//let sensory_input id =
//    database.Provided.open_connection.Query<ai.figure.Figure>(
//        @"insert into history_line (signal)
//        values (@signal_id)",
//        {|id=id|}
//    ) |> ignore