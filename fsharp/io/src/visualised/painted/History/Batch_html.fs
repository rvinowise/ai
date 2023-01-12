namespace rvinowise.ai.ui.painted
    
    
    module Batch_html =
        open System.Drawing
        open Giraffe.ViewEngine
        open rvinowise.ai

        let _port = attr "port"
        
        let cell_of_mood_change mood_change =
            let red,green =
                if (mood_change < 0) then
                    1f , 1f + float32(mood_change) / 5f
                else 
                    1f - float32(mood_change) / 5f , 1f
            
            let mood_color=
                Color.FromArgb(255, int(red*255f), int(green*255f), 255)
                |>ColorTranslator.ToHtml

            let string_change = 
                if mood_change>0 then $"+{mood_change}" else $"{mood_change}"
            tr [] 
                [ td [attr "BGCOLOR" mood_color; _port "mood_change"] [str string_change ]] 

        let cell_for_event 
            event 
            =
            match event with
            |Start figure-> 
                tr [] [ td [_port $"({figure}"] [str $"({figure}" ]] 
            |Finish (figure, _) -> 
                tr [] [ td [_port $"{figure})"] [str $"{figure})" ]] 
            |Signal (figure) -> 
                tr [] [ td [_port $"({figure})"] [str $"({figure})" ]] 
            |Mood_change value ->cell_of_mood_change value
            
        let cell_for_moment moment =
            tr [] 
                [ td 
                    [attr "BORDER" "0"; attr "BGCOLOR" "#aaaaaa"; _port "moment"] 
                    [str (string moment) ]
                ] 

        let layout_for_event_batch
            (moment:Moment)
            (batch:Event_batch)
            =
            table [
                    (attr "BORDER" "0") 
                    (attr "CELLBORDER" "1") 
                    (attr "CELLSPACING" "0") 
                    (attr "CELLPADDING""4")
                ] 
                (
                    batch.events
                    |>Seq.sort
                    |>Seq.map cell_for_event
                    |>Seq.append([
                        cell_for_moment moment
                        
                    ])
                    |>List.ofSeq
                )