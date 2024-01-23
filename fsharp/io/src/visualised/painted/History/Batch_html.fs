namespace rvinowise.ai.ui.painted

open System.Drawing
open Giraffe.ViewEngine
open rvinowise.ai

module Batch_html =
    

    let _port = attr "port"
    
    let mood_color mood =
        let red,green =
            if (mood < 0) then
                1f , 1f + float32(mood) / 5f
            else 
                1f - float32(mood) / 5f , 1f
    
        Color.FromArgb(255, int(red*255f), int(green*255f), 255)
        |>ColorTranslator.ToHtml
    
    let cell_of_mood_change mood_change =
        if mood_change = 0 then
            None
        else
            let string_change = 
                if mood_change>0 then $"+{mood_change}" else $"{mood_change}"
            tr [] 
                [ td [attr "BGCOLOR" (mood_color mood_change); _port "mood_change"] [str string_change ]] 
            |>Some

    let cell_of_mood_value (mood_value) =
        if mood_value = 0 then
            None
        else
            tr [] 
                [td 
                    [attr "BGCOLOR" (mood_color mood_value); _port "mood_value"] 
                    [str (string mood_value) ]
                ] 
            |>Some

    let cells_of_mood 
        (mood_change:Mood) 
        (mood_value:Mood) 
        =
        [
        (cell_of_mood_change (Mood.value mood_change));
        (cell_of_mood_value (Mood.value mood_value))
        ]|>Seq.choose id

    let cell_of_event 
        event 
        =
        match event with
        |Start figure-> 
            tr [] [ td [_port $"({figure}"] [str $"({Figure_id.value figure}" ]] 
        |Finish (figure, _) -> 
            tr [] [ td [_port $"{figure})"] [str $"{Figure_id.value figure})" ]] 
        |Signal (figure) -> 
            tr [] [ td [_port $"{figure}"] [str $"{Figure_id.value figure}" ]] 

        // match event with
        // |Start figure-> 
        //     tr [] [ td [_port $"({figure}"] [str $"({figure}" ]] 
        // |Finish (figure, _) -> 
        //     tr [] [ td [_port $"{figure})"] [str $"{figure})" ]] 
        // |Signal (figure) -> 
        //     tr [] [ td [_port $"{figure}"] [str $"{figure}" ]] 
        
    let cell_for_moment moment =
        tr [] 
            [ td 
                [attr "BORDER" "0"; attr "BGCOLOR" "#aaaaaa"; _port "moment"] 
                [str (string moment) ]
            ] 
    

    let layout_for_event_batch
        (moment:Moment)
        (mood_change: Mood)
        (mood_value: Mood)
        (events: Appearance_event seq)
        =
        table [
                (attr "BORDER" "0") 
                (attr "CELLBORDER" "1") 
                (attr "CELLSPACING" "0") 
                (attr "CELLPADDING""4")
            ] 
            (
                let cells_of_events = 
                    events
                    |>Seq.sort
                    |>Seq.map cell_of_event
                
                [cell_for_moment moment]
                |>Seq.append(
                    cells_of_mood mood_change mood_value
                )
                |>Seq.append cells_of_events
                |>List.ofSeq
            )