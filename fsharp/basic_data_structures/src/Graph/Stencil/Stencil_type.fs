namespace rvinowise.ai.stencil
    open System.Collections.Generic
    open rvinowise.ai


    [<Struct>]
    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

    type Vertex_data = IDictionary<Vertex_id, Node_reference>

    

namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai.stencil
    open rvinowise.extensions


    type Stencil = {
        graph: Graph
        nodes: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Stencil_{this.graph.id}( "
            this.graph.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail
                ++"->"
                ++ edge.head
                +=" "
            )
            result+=")"
            result.ToString()


namespace rvinowise.ai.stencil

    module built=
        open rvinowise.ai
        open rvinowise.extensions

        let node_reference_from_string name=
            match name with
            | "out" -> Node_reference.Stencil_output
            | subfigure -> Node_reference.Lower_figure subfigure

        let vertex_data_from_tuples edges =
            edges
            |>Seq.map (fun(tail_id,tail,head_id,head)->
                [
                    (tail_id, node_reference_from_string tail);
                    (head_id, node_reference_from_string head)
                ]
            )
            |>Seq.concat
            |>dict 

        let simple id (edges:seq<string*string>) =
            {
                graph=graph.built.simple id edges
                nodes=edges
                    |>Seq.map (fun(tail_id,head_id)->
                        [
                            (
                                tail_id, 
                                tail_id
                                |>String.remove_number 
                                |>node_reference_from_string
                            );
                            (
                                head_id,
                                head_id
                                |>String.remove_number 
                                |>node_reference_from_string
                            );
                        ]
                    )
                    |>Seq.concat
                    |>dict 
            }

        let from_tuples
            (id:Figure_id)
            (edges:seq<Vertex_id*string*Vertex_id*string>) =
            {
                graph=graph.built.from_tuples id edges
                nodes=vertex_data_from_tuples edges
            }

    module example=
        open rvinowise.ai

        let a_fitting_stencil =
            built.simple
                "S"
                [
                    "b","out1";
                    "out1","f";
                    "h","f";
                ]