module rvinowise.ai.ui.printed.Figure_appearance

open rvinowise


let moments (appearance: ai.Figure_appearance) =
    printf $"({appearance.start} {appearance.ending}) "
