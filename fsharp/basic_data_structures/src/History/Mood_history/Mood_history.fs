namespace rvinowise.ai

    type Mood = Mood of int

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mood=
        let value (Mood mood) = mood
   
        let inline (+) (Mood mood1) (Mood mood2) = Mood (mood1+mood2)
        let inline (<>) (Mood mood1) (Mood mood2) = mood1 <> mood2

    type Mood_history = {
        interval: Interval
        mood_at_moments: seq<Mood>
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mood_history=
        open rvinowise.ai
        open FsUnit
        open Xunit

        let interval (history:Mood_history) =
            history.interval


        let mood_at_moment history moment=
            ()

        let ofChanges mood_changes=
            ()