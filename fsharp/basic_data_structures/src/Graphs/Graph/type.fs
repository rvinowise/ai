namespace rvinowise.ai
    open System.Text
    
    open rvinowise.extensions
    open System
    open System.Linq

    [<CustomEquality; NoComparison>]
    type Graph = {
        id: Figure_id
        edges: Edge seq
    } with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Graph_{this.id}( "
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

        override this.GetHashCode() =
            (hash this.id) ^^^ (hash this.edges)

        override this.Equals other =
            match other with
            | :? Graph as other ->
                this.id = other.id 
                && 
                Enumerable.SequenceEqual(this.edges, other.edges)
            |_->false

        interface IEquatable<Graph> with
            member this.Equals other=
                this.Equals(other)