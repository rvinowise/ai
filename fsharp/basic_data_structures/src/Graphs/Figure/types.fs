namespace rvinowise.ai.figure_parts
    open System.Collections.Generic
    open rvinowise.ai
    type Vertex_data = IDictionary<Vertex_id, Figure_id>

namespace rvinowise.ai
    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure_parts

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




        





    