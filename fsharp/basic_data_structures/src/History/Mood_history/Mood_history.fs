namespace rvinowise.ai

    [<CustomEquality; CustomComparison>]
    type Mood = Mood of int
        with 
        static member value (Mood mood) = mood
        override this.Equals(other) =
            match other with
            | :? Mood as other ->
                Mood.value this = Mood.value other
            | _ -> false
        override this.GetHashCode() =
            Mood.value this
        static member (+) (this, other) =
            Mood (Mood.value this + Mood.value other)
        interface System.IComparable<Mood> with
            member this.CompareTo(other) =
                (Mood.value this).CompareTo(Mood.value other)

    // [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    // module Mood=
    //     let value (Mood mood) = mood
    //     let inline (+) (Mood mood1) (Mood mood2) = Mood (mood1+mood2)

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