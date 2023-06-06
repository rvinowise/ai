module rvinowise.ai.example.Event_batches
    open rvinowise.ai
    
    let short_history_with_some_repetitions=
        [
            ["a";"x"];
            ["b";"y"];
            ["a";"z";"x"];
            ["c"];
            ["b";"x"];
            ["b"];
            ["a"];
            ["c"]
        ]|>Event_batches.event_history_from_lists