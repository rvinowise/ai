module rvinowise.ai.loaded.figure.Appearances

open Dapper

open rvinowise
open rvinowise.ai


let all_appearances figure_id =
    database.Provided.open_connection.Query<ai.figure.Appearance>(
        @"select * from Figure_appearance where Figure = @Figure_id",
        {|figure_id=figure_id|}
    )
    //db_connection.Close()
let appearances_in_interval figure_id head tail =
    database.Provided.open_connection.Query<ai.figure.Appearance>(
        @"select * from Figure_appearance
        where Figure = @Figure_id
        and Head >= @Head
        and Tail < @Tail",
        {|figure_id=figure_id; head=head; tail=tail|}
    )



let current_moment = 
    database.Provided.open_connection.Query<int64>(
        @"select Moment from Caret"
    ) |> Seq.head