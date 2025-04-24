namespace rvinowise.ai

open System.Text
open rvinowise.ai
open rvinowise.extensions


[<Struct>]
type Stencil_node =
| Lower_figure of Lower_figure: Figure_id
| Stencil_output

type Stencil = {
    edges: Edge Set
    nodes: Map<Vertex_id, Stencil_node>
    output_without: Figure Set
}
with 
    override this.ToString() =
        let result = StringBuilder()
        result 
        += "Stencil( "
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