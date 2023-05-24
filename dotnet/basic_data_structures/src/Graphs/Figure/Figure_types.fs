

namespace rvinowise.ai
    open System.Text
    open rvinowise
    open rvinowise.extensions
    open System
    open System.Linq

            
    [<CustomEquality; CustomComparison>]
    type Figure = {
        edges: Edge Set
        subfigures: Map<Vertex_id, Figure_id>
        without: Figure Set
    }
    with 
        override this.ToString()=
            Figure_printing.figure_to_string this.edges this.subfigures
        
        override this.Equals(other) =
            match other with
            | :? Figure as other ->
                Enumerable.SequenceEqual(this.edges, other.edges)
                && 
                Enumerable.SequenceEqual(this.subfigures, other.subfigures)
            | _ -> false
        
        override this.GetHashCode() =
            this.edges.GetHashCode() ^^^ this.subfigures.GetHashCode()

        interface IEquatable<Figure> with   
            member this.Equals other =
                this.Equals other
        
        member this.compare (other:Figure) =
            let subfigures_diff = 
                this.subfigures
                |>extensions.Map.compareWith other.subfigures
            if (subfigures_diff = 0) then
                this.edges
                |>Seq.compareWith Operators.compare other.edges
            else
                subfigures_diff
        
        interface IComparable with
            member this.CompareTo other =
                match other with
                | :? Figure as other ->
                    this.compare other
                | _ -> invalidArg "other" "cannot compare value of different types"
        
        interface IComparable<Figure> with
            member this.CompareTo other =
                this.compare other



    