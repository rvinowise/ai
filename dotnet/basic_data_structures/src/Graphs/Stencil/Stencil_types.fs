namespace rvinowise.ai

open System.Text
open rvinowise.ai
open rvinowise.extensions

        
type Stencil_output_border = {
    before: Vertex_id Set
    after: Vertex_id Set
}
type Conditional_stencil = {
    figure: Conditional_figure
    output_border: Stencil_output_border
}