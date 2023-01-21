

namespace rvinowise.ai
    open rvinowise

    type Edge = 
        struct
            val tail: Vertex_id
            val head: Vertex_id

            new (tail, head) =
                {tail = tail; head = head;}

        end
        
         
