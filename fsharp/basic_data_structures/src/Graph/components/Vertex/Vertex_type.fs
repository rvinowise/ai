namespace rvinowise.ai.stencil
    open rvinowise.ai

    [<Struct>]
    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

   