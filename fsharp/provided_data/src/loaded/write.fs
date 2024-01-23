namespace rvinowise.ai.database

open Dapper

open rvinowise
open rvinowise.ai


module Write=

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
                head = appearance.start; 
                tail = appearance.finish
            |}
        ) |> ignore

