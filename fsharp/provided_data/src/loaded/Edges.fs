module rvinowise.ai.loaded.figure.Edges

open System.Collections.Generic
open System.Data.SqlClient
open Dapper

open rvinowise
open rvinowise.ai
open System.Data
open System.Data.Common;    // for DbProviderFactories
open System.Configuration





let edges figure_id =
    database.Provided.open_connection.Query<ai.figure.Edge>(
        @"select * from Edge where Parent = @Figure_id",
            {|figure_id=figure_id|}
    )

