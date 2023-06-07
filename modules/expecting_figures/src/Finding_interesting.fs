module rvinowise.ai.Finding_interesting
    
    open FsUnit
    open Xunit
    open rvinowise


    let find_good_sequences
        (sequence_history: (Sequence*Interval array) seq)
        (mood_changes: (Moment*Mood) seq)
        =
        mood_changes
        |>Seq.filter (snd>>Mood.is_good)
        |>Mood_history.intervals_changing_mood