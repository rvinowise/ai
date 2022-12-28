namespace rvinowise.ai
    open System.Text
    open rvinowise.extensions

    type Graph = {
        id: Figure_id
        edges: Edge seq
    } with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Graph_{this.id}( "
            this.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail
                ++"->"
                ++ edge.head
                +=" "
            )
            result+=")"
            result.ToString()

namespace rvinowise.ai.graph
    open rvinowise.ai

    module built=

        let simple (id:Figure_id) (edges:seq<Vertex_id*Vertex_id>) =
            {
                id=id;
                edges=
                    edges
                    |>Seq.map (fun (tail_id, head_id)->
                        Edge(
                            tail_id, head_id
                        );
                    )
            }

        let from_tuples id edges =
            {
                id=id;
                edges=
                    edges
                    |>Seq.map (fun (tail_id, _, head_id,_)->
                        Edge(
                            tail_id, head_id
                        );
                    )
            }