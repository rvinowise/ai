namespace rvinowise.ai

    type Separate_histories = {
        figure_apperances: Figure_id_appearances seq
        mood_change_history: Mood_changes_history
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Separate_histories =
        let figure_id_appearances histories=
            histories.figure_apperances
        
       

        let mood_change_history histories=
            histories.mood_change_history