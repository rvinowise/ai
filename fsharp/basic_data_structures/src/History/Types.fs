﻿namespace rvinowise.ai
    open FsUnit
    open Xunit
    
    open rvinowise.ai

    type Appearance_event=
    |Start of Figure_id
    |Finish of Figure_id * Moment
    |Signal of Figure_id
    |Mood_change of int


    type Event_batch = {
        events: Appearance_event seq
        mood:Mood
    }

    module Event_batch=
        let empty =
            {
                events= []
                mood= 0
            }
        
        let ofSignals mood figures =
            {
                mood=mood
                events= 
                    figures
                    |>Seq.map Signal
            }

    type Combined_history = {
        //interval: Interval
        batches: Map<Moment, Event_batch>
    }