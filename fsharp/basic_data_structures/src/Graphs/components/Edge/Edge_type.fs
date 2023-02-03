

namespace rvinowise.ai
    open rvinowise

    type Edge = 
        struct
            val tail: Vertex_id
            val head: Vertex_id

            new (tail, head) =
                {tail = tail; head = head;}

        end
        
         
    module Edge=

        let ofPair pair =
            Edge(fst pair, snd pair)