namespace rvinowise.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure

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


    
    