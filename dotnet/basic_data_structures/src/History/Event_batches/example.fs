module rvinowise.ai.example.Event_batches
    open rvinowise.ai
    
    let short_history_with_some_repetitions=
        built_from_text.Event_batches.from_contingent_signals 0 [
                ["a";"x"];
                ["b";"y"];
                ["a";"z";"x"];
                ["c"];
                ["b";"x"];
                ["b"];
                ["a"];
                ["c"]
            ]