namespace rvinowise.ai

type Edge = 
    struct
        val tail: Vertex_id
        val head: Vertex_id

        new (tail, head) =
            {tail = tail; head = head;}
        new (tail, head) =
            {tail = Vertex_id tail; head = Vertex_id head;}
        override this.ToString() =
            $"Edge({this.tail}->{this.head})"
    end
    
     
module Edge=

    let ofPair (pair: Vertex_id*Vertex_id) =
        Edge(fst pair, snd pair)

    let ofStringPair (pair: string*string) =
        Edge(fst pair, snd pair)