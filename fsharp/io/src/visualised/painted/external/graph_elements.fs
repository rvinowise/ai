namespace rvinowise.ui.painted


    open rvinowise
    open rvinowise.ai

    type Vertex =
        struct
            val id:Vertex_id
            val label:string

            new(id, label) = {
                id=id; label=label;
            }
        end

    type Edge=
        struct
            val tail: Vertex
            val head: Vertex

            new (tail, head) = {
                tail=tail; head=head
            }
        end


    
    