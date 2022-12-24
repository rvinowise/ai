namespace rvinowise.ai

    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output


namespace rvinowise.ai.figure
    open rvinowise.ai

    type Subfigure = 
        struct
            val id: Node_id
            val referenced: Figure_id

            new (id, referenced) =
                {id = id; referenced = referenced;}
            new (id: Node_id) =
                {id = id; referenced = id;}
        end

namespace rvinowise.ai.stencil
    open rvinowise.ai

    type Node =
        struct
            val id: Node_id
            val referenced: Node_reference

            new (id) =
                {id = id; referenced = Lower_figure id;}
            new (id, referenced: string) =
                {id = id; referenced = Lower_figure referenced;}
            new (id, referenced) =
                {id = id; referenced = referenced;}
        end

