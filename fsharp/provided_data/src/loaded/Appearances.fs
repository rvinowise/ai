namespace rvinowise.ai.loaded.figure

open Dapper
open rvinowise.ai


module Appearances=

    let all_appearances figure_id =
        database.Provided.open_connection.Query<Interval>(
            @"select * from Figure_appearance where Figure = @Figure_id",
            {|figure_id=figure_id|}
        )
        
    let appearances_in_interval figure_id head tail =
        database.Provided.open_connection.Query<Interval>(
            @"select * from Figure_appearance
            where Figure = @Figure_id
            and Head >= @Head
            and Tail < @Tail",
            {|figure_id=figure_id; head=head; tail=tail|}
        )



    let current_moment = 
        database.Provided.open_connection.Query<Moment>(
            @"select Moment from Caret"
        ) |> Seq.head