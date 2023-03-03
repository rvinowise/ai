namespace rvinowise.ai
    open rvinowise.ai

    [<Struct>]
    type Stencil_node =
    | Lower_figure of Lower_figure: Figure_node
    | Stencil_output


namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai
    open rvinowise.extensions

    type Stencil_vertex_data = IDictionary<Vertex_id, Stencil_node>

    type Stencil = {
        edges: Edge seq
        nodes: Stencil_vertex_data
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