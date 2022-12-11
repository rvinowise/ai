namespace rvinowise.ai.figure
    open rvinowise.ai

    (* figure.Edge only connect subfigures, but stencil.Edge can either connect subfigures or stencil outputs  *)

    type Edge = 
        struct
            val tail: Subfigure
            val head: Subfigure

            new (tail, head) =
                {tail = tail; head = head;}
        end

    

namespace rvinowise.ai.stencil
    open rvinowise.ai

    type Edge = 
        struct
            val tail: Node
            val head: Node

            new (tail, head) =
                {tail = tail; head = head;}
        end

    