namespace rvinowise.ai
open System

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
    interface System.IComparable<int> with
        member this.CompareTo(other) =
            (Mood.value this).CompareTo(other)

    interface IComparable with
        member this.CompareTo(other) =
            match other with 
            | null -> 1
            | :? Mood as other -> 
                (Mood.value this).CompareTo(Mood.value other)
            | :? int as other -> 
                (Mood.value this).CompareTo(other)
            | _ ->
                invalidArg (nameof other) "Other is not a Mood"

module Mood=
    let is_good (mood: Mood)=
        mood > Mood 0
    let is_bad (mood: Mood)=
        mood < Mood 0


