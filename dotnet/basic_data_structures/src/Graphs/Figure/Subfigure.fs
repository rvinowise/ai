namespace rvinowise.ai

open System
open rvinowise.ai
open rvinowise.ai.built
open rvinowise.extensions


[<CustomEquality; CustomComparison>]
type Subfigure = {
    name: Figure_id
    is_mappable: Subfigure -> bool
}
with

    override this.ToString()=
        Figure_id.value this.name
    
    override this.Equals(other) =
        match other with
        | :? Subfigure as other ->
            this.name = other.name
        | _ -> false
    
    override this.GetHashCode() =
        this.name.GetHashCode()

    interface IEquatable<Subfigure> with   
        member this.Equals other =
            this.Equals other
    
    member this.compare (other:Subfigure) =
        compare this.name other.name
    
    interface IComparable with
        member this.CompareTo other =
            match other with
            | :? Subfigure as other ->
                this.compare other
            | _ -> invalidArg "other" "cannot compare value of different types"
    
    interface IComparable<Subfigure> with
        member this.CompareTo other =
            this.compare other
            
            
