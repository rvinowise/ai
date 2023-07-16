module rvinowise.ai.History_from_text
    open System
    open FsUnit
    open Xunit
    open FParsec

    open rvinowise.ai
    open System.IO
    

    let separator =
        (skipString ";")

    
    let no_mood: Parser<Mood, unit> =
        spaces |>> fun _ ->(Mood 0)

    let mood_changes_as_repeated_symbols
        bad_symbol good_symbol
        =
        let particular_mood_change mood_symbol mood_multiplier =
            (
                many1 (pchar mood_symbol) .>> spaces
                |>> (fun symbols -> 
                    symbols.Length * mood_multiplier
                    |>Mood
                )
            )
        (particular_mood_change bad_symbol -1)
        <|>(particular_mood_change good_symbol 1)



    let mood_changes_as_words_and_numbers 
        bad_word good_word
        =
        let particular_mood_change mood_word mood_multiplier =
            (
                pstring mood_word >>. (puint32 <|>% uint32 1) .>> (many separator) .>> spaces
                |>> (
                    int
                    >> ( * ) mood_multiplier
                    >>Mood
                )
            )
        (particular_mood_change bad_word -1)
        <|>(particular_mood_change good_word 1)


    let signals_from_text
        (mood_change: Parser<Mood, unit>)
        (text:string)
        =
        let signal =
            (
                anyString 1 
                |>> Figure_id
            ) .>> spaces
        let batch = 
            signal .>>. (mood_change <|>% Mood 0)

        let batch_ws =
            batch .>> spaces
        let all_batches = many (spaces >>. batch_ws)
        
        text
        |>run all_batches
        |>function
        |Success (batches,_,_) ->
            batches
        |Failure (error,_,_) -> failwith error

    let event_batches_from_text
        (mood_change: Parser<Mood, unit>)
        (text:string)
        =
        signals_from_text
            mood_change
            text
        |>Seq.map (fun (signal,mood) -> 
            signal
            |>Signal
            |>List.singleton 
            ,
            mood
        )


    let event_batches_from_textfile
        mood_changes
        (file:string)
        =
        use input_stream =
            new StreamReader(file)
        input_stream.ReadToEnd()
        |>event_batches_from_text
            mood_changes
            


    [<Fact>]
    let ``try from_text``()=
        "a1ok;2 no2;3"
        |>signals_from_text
            (mood_changes_as_words_and_numbers "no" "ok")
        |>should equal (
            [
                "a",0;    
                "1",1; 
                "2",-2; 
                "3",0; 
            ]|>Seq.map (fun (symbol, mood) ->
                symbol|>Figure_id
                ,
                Mood mood
            )
        )

  
    let event_batches_from_text_blocks (text_blocks:string seq)=
        text_blocks
        |>Seq.collect id
        |>String.Concat
        |>event_batches_from_text
            (mood_changes_as_words_and_numbers "no" "ok")

    let sequential_figure_from_text 
        (mood_change: Parser<Mood, unit>)
        text 
        =
        text
        |>event_batches_from_text mood_change
        |>Event_batches.only_signals
        |>Event_batches.sequential_event_batches_to_figure