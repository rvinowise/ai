namespace rvinowise.ai
    open System.Text
    open rvinowise.extensions
    open System.Collections.Generic
    
    type Vertex_data = IDictionary<Vertex_id, Figure_id>

    type Figure = {
        graph: Graph
        subfigures: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Figure_{this.graph.id}( "
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




        





    