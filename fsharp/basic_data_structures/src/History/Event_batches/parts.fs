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
                change=0
                value=0
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
        
        let ofSignalsWithMood signals =
            let is_mood_string (text:string)=
                match (text|>Seq.head) with
                |'+'|'-'->true
                |_->false
            let (|Int|_|) (str:string) =
                match System.Int32.TryParse str with
                | true,int -> Some int
                | _ -> None
            {
                events=
                    signals
                    |>Seq.filter (is_mood_string>>not)
                    |>Seq.map Signal
                mood=
                {
                    change=
                        signals
                        |>Seq.filter is_mood_string
                        |>Seq.tryHead
                        |>function
                        |Some mood_string->
                            match System.Int32.TryParse mood_string with
                            | true,int -> int
                            | _ -> 0
                        |None->0
                    value=0
                }
        }


    