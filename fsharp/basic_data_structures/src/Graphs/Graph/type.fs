namespace rvinowise.ai
    open rvinowise.extensions

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

