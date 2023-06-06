module rvinowise.ai.built_from_text.Event_batches
    open System
    open FsUnit
    open Xunit
    open FParsec

    open rvinowise.ai
    open rvinowise 
    

    let event_batches_from_text 
        (text:string)
        =
        let good = "ok"
        let bad = "no"

        let separator =
            (skipString ";")
        let particular_mood_change mood_word mood_multiplier =
            (
                pstring mood_word >>. (puint32 <|>% uint32 1) .>> (many separator) .>> spaces
                |>> (
                    int
                    >> ( * ) mood_multiplier
                    >>fun mood -> [],Mood mood
                )
            )
        let any_mood_change =
            (particular_mood_change "ok" 1)
            <|>(particular_mood_change "no" -1)
        let signal = 
            any_mood_change
            <|>(
                anyString 1 
                |>> (fun symbol -> 
                    [symbol|>Figure_id|>Appearance_event.Signal],Mood 0
                )
            )

        let signal_ws =
            signal .>> spaces
        let all_signals = many (spaces >>. signal_ws)
        
        text
        |>run all_signals
        |>function
        |Success (batches,_,_) ->
            batches
        |Failure (error,_,_) -> failwith error


    [<Fact>]
    let ``try from_text``()=
        "a1ok;2 no2;3"
        |>event_batches_from_text
        |>should equal ([
            ["a"],0;    
            ["1"],0; 
            [],1; 
            ["2"],0; 
            [],-2; 
            ["3"],0; 
        ])

  
    let from_text_blocks (text_blocks:string seq)=
        text_blocks
        |>Seq.collect id
        |>String.Concat
        |>event_batches_from_text


    