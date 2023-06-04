namespace rvinowise.ai.ui.painted

    type Mood_state = {
        change:Mood
        value:Mood
    }

    type Event_batch = {
        events: Appearance_event seq
        mood:Mood_state
    }

    type Event_batches = Map<Moment, Event_batch>


    module Mood_state=
        let empty=
            {
                change=Mood 0
                value=Mood 0
            }

    module Event_batch=
        let empty =
            {
                events= []
                mood= Mood_state.empty
            }
        
        let mood_change_from_string (text:string)=
            match (text|>Seq.head) with
            |'+'|'-'->
                match System.Int32.TryParse text with
                | true,int -> Some (Mood int)
                | _ -> None
            |_->None