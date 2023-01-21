namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai.stencil
    open rvinowise.extensions


    type Stencil = {
        graph: Graph
        nodes: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Stencil_{this.graph.id}( "
            this.graph.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail
                ++"->"
                ++ edge.head
                +=" "
            )
            result+=")"
            result.ToString()