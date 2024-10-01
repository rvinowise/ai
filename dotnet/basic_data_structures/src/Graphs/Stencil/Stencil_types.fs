namespace rvinowise.ai

open System.Text
open rvinowise.ai
open rvinowise.extensions



type Stencil = {
    figure: Figure
    vertices_before_out: Vertex_id Set
    vertices_after_out: Vertex_id Set
    output_without: Figure Set
    blocking_vertices:
        Map<Edge,Set<Figure_id>>
}
with 
    override this.ToString() =
        let result = StringBuilder()
        result 
        += "Stencil( "
        this.figure.edges
        |>Seq.iter(fun edge ->
            result 
            ++ edge.tail
            ++"->"
            ++ edge.head
            +=" "
        )
        result+=")"
        result.ToString()