﻿module rvinowise.ai.loaded.Figure

open System.Collections.Generic
open Dapper

open rvinowise
open rvinowise.ai

let body figure_id =
    database.Provided.open_connection.Query<ai.figure.Figure>(
        @"select * from Figure where Id = @Figure_id",
            {|figure_id=figure_id|}
    ) |> Seq.tryHead

let exists id =
    printfn $"figure %s{id} exists requested"
    body id <> None

let all =
    database.Provided.open_connection.Query<ai.figure.Figure>(
        @"select * from Figure"
    )