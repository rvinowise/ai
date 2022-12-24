

namespace rvinowise.ai

    type Edge =
        interface
            abstract member tail_id:Node_id
            abstract member head_id:Node_id
            abstract member tail:Vertex
            abstract member head:Vertex

        end

namespace rvinowise.ai.figure
    open rvinowise

    (* figure.Edge only connects subfigures, but stencil.Edge can either connect subfigures or stencil outputs  *)
    type Edge = 
        struct
            val tail: Subfigure
            val head: Subfigure

            new (tail, head) =
                {tail = tail; head = head;}

            interface ai.Edge with
                member this.tail_id = this.tail.id
                member this.head_id = this.head.id
                member this.tail = this.tail
                member this.head = this.head
        end
        
         

namespace rvinowise.ai.stencil
    open rvinowise

    type Edge = 
        struct
            val tail: Node
            val head: Node

            new (tail, head) =
                {tail = tail; head = head;}

            interface ai.Edge with
                member this.tail_id = this.tail.id
                member this.head_id = this.head.id
                member this.tail = this.tail
                member this.head = this.head
        end
