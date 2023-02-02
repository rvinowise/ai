namespace rvinowise.ai.figure_parts
    open System.Collections.Generic
    open rvinowise.ai
    type Vertex_data = Map<Vertex_id, Figure_id>

namespace rvinowise.ai
    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure_parts
    open System
    open System.Linq

    [<CustomEquality; NoComparison>]
    type Figure = {
        edges: Edge seq
        subfigures: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Figure( "
            this.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail
                ++"->"
                ++ edge.head
                +=" "
            )
            result+=")"
            result.ToString()

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
        





    