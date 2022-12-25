

namespace rvinowise.ai
    open rvinowise.ai
    type Vertex=
        interface
            abstract member id:Node_id
            //abstract member referenced_figure:Figure_id
        end

namespace rvinowise.ai.figure
    open rvinowise.ai
    open rvinowise

    type Subfigure = 
        struct
            val id: Node_id
            val referenced: Figure_id

            new (id, referenced) =
                {id = id; referenced = referenced;}
            new (id: Node_id) =
                {id = id; referenced = id;}

            interface ai.Vertex with
                member this.id=this.id
                //member this.referenced_figure=this.referenced
        end

namespace rvinowise.ai.stencil
    open rvinowise.ai
    open rvinowise

    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

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

            interface ai.Vertex with
                member this.id=this.id
        end

