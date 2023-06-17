module rvinowise.ai.built_from_text.Event_batches
    open System
    open FsUnit
    open Xunit
    open FParsec

    open rvinowise.ai
    open System.IO
    

    let separator =
            (skipString ";")

    
    let no_mood: Parser<Appearance_event list * Mood, unit> =
        spaces |>> fun _ ->([], Mood 0)

    let mood_changes_as_repeated_symbols
        bad_symbol good_symbol
        =
        let particular_mood_change mood_symbol mood_multiplier =
            (
                many (pchar mood_symbol) .>> spaces
                |>> (fun symbols -> 
                    symbols.Length * mood_multiplier
                    |>fun mood -> [],Mood mood
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
                    >>fun mood -> [],Mood mood
                )
            )
        (particular_mood_change bad_word -1)
        <|>(particular_mood_change good_word 1)

    let attach_mood_to_preceding_signal
        (batch_elements: (Appearance_event list * Mood) list)
        =
        batch_elements
        |>List.fold (fun batches element ->
            match element|>snd with
            | Mood 0 ->
                element::batches
            | mood_change ->
                match batches with
                |(last_signals,last_mood)::rest_batches ->
                    let updated_mood =
                        last_mood
                        |>(+) mood_change
                    (last_signals,updated_mood)::rest_batches
                |[]->[]
        ) []
        |>List.rev

    let event_batches_from_text
        (mood_change: Parser<Appearance_event list * Mood, unit>)
        (text:string)
        =
        let batch_element = 
            mood_change
            <|>(
                anyString 1 
                |>> (fun symbol -> 
                    [symbol|>Figure_id|>Signal],Mood 0
                )
            )


        let batch_element_ws =
            batch_element .>> spaces
        let all_batch_elements = many (spaces >>. batch_element_ws)
        
        text
        |>run all_batch_elements
        |>function
        |Success (batches,_,_) ->
            batches
            |>attach_mood_to_preceding_signal
        |Failure (error,_,_) -> failwith error


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
        |>event_batches_from_text
            (mood_changes_as_words_and_numbers "no" "ok")
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
            (mood_changes_as_words_and_numbers "no" "ok")

    