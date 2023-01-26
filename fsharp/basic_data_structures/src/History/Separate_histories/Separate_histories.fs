namespace rvinowise.ai

    type Separate_histories = {
        figure_histories: Figure_history seq
        mood_change_history: Mood_changes_history
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Separate_histories =
        let figure_histories histories=
            histories.figure_histories
        
        let mood_change_history histories=
            histories.mood_change_history