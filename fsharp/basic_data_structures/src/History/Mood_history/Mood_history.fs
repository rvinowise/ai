namespace rvinowise.ai

    type Mood = int
   
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