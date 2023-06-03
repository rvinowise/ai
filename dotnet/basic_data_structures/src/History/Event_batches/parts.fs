namespace rvinowise.ai
    open FsUnit
    open Xunit
    
    open rvinowise.ai

    type Appearance_event=
    |Start of Figure_id
    |Finish of Figure_id * Moment
    |Signal of Figure_id

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
        
        let ofSignals mood figures =
            {
                mood=mood
                events= 
                    figures
                    |>Seq.map Signal
            }
        
        let is_mood_string (text:string)=
                match (text|>Seq.head) with
                |'+'|'-'->true
                |_->false

        let mood_change_from_string (text:string)=
            match (text|>Seq.head) with
            |'+'|'-'->
                match System.Int32.TryParse text with
                | true,int -> Some (Mood int)
                | _ -> None
            |_->None

        let ofSignalsWithMood signals =
            {
                events=
                    signals
                    |>Seq.filter (fun signal->
                        mood_change_from_string signal = None
                    )
                    |>Seq.map (Figure_id>>Signal)
                mood=
                {
                    change=
                        signals
                        |>Seq.choose mood_change_from_string
                        |>Seq.tryHead
                        |>function
                        |Some mood->mood
                        |None->Mood 0
                    value=Mood 0
                }
        }

        let ofMood mood =
            {
                events=[]
                mood={change=Mood mood;value=Mood 0}
            }

    