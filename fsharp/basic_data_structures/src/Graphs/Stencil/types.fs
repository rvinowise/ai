namespace rvinowise.ai
    open rvinowise.ai

    [<Struct>]
    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

namespace rvinowise.ai.stencils_parts
    open System.Collections.Generic
    open rvinowise.ai
    type Vertex_data = IDictionary<Vertex_id, Node_reference>


namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai
    open rvinowise.ai.stencils_parts
    open rvinowise.extensions

    type Stencil = {
        edges: Edge seq
        nodes: Vertex_data
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