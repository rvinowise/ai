module rvinowise.ai.database.Write

open Dapper

open rvinowise
open rvinowise.ai


let new_signal (id:string) =
    database.Provided.open_connection.Query<ai.Figure>(
        @"insert into Figure (id)
        values (@id)",
        {|id=id|}
    ) |> ignore

let figure_appearance 
    figure
    (appearance: Interval) 
    =
    database.Provided.open_connection.Query<ai.Figure>(
        @"insert into figure_appearance (figure, head, tail)
        values (@figure, @head, @tail)",
        {|
            figure = figure; 
            head = appearance.head; 
            tail = appearance.tail
        |}
    ) |> ignore

//let sensory_input id =
//    database.Provided.open_connection.Query<ai.Figure>(
//        @"insert into history_line (signal)
//        values (@signal_id)",
//        {|id=id|}
//    ) |> ignore