namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure

    type Node =
        struct
            val id:Node_id
            val label:string

            new(id, label) = {
                id=id; label=label;
            }
        end

    type Edge=
        struct
            val tail: Node
            val head: Node

            new (tail, head) = {
                tail=tail; head=head
            }
        end

    
