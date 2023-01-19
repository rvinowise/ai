module module rvinowise.ai.event_batches.example=
    open rvinowise.ai.event_batches
    
    let short_history_with_some_repetitions=
        built.from_contingent_signals 0 [
                ["a";"x"];
                ["b";"y"];
                ["a";"z";"x"];
                ["c"];
                ["b";"x"];
                ["b"];
                ["a"];
                ["c"]
            ]