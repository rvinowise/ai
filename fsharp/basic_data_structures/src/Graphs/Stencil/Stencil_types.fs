namespace rvinowise.ai
    open rvinowise.ai

    [<Struct>]
    type Figure_node =
    | Lower_figure of Figure_id
    | Stencil_output


namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai
    open rvinowise.extensions

    type Figure_vertex_data = IDictionary<Vertex_id, Figure_node>

    type Stencil = {
        edges: Edge seq
        nodes: Figure_vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Stencil( "
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