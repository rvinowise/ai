module rvinowise.ai.database.Read

open System.Collections.Generic
open System.Data.SqlClient
open Dapper

open rvinowise
open rvinowise.ai
open System.Data
open System.Data.Common;    // for DbProviderFactories
open System.Configuration



let appearances_of figure_id =
    database.Provided.open_connection.Query<ai.figure.Appearance>(
        @"select * from Figure_appearance where Figure = @Figure_id",
        {|figure_id=figure_id|}
    )
    //db_connection.Close()


let edges_of figure_id =
    database.Provided.open_connection.Query<ai.figure.Edge>(
        @"select * from Edge where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )

let body_of figure_id =
    database.Provided.open_connection.Query<ai.figure.Figure>(
        @"select * from Figure where Id = @Figure_id",
            {|figure_id=figure_id|}
    )

let current_moment = 
    database.Provided.open_connection.Query<int64>(
        @"select Moment from Caret"
    ) |> Seq.head