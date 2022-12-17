module rvinowise.ai.ui.printed.figure.Appearance

open rvinowise


let moments (appearance: ai.Interval) =
    printf $"({appearance.head} {appearance.tail}) "
