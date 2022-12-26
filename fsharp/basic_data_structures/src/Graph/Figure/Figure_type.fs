namespace rvinowise.ai

    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure
    open System.Collections.Generic

    type Figure = {
        id: Figure_id
        edges: Edge seq

        subfigures: IDictionary<Vertex_id, Subfigure>
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Figure_{this.id}( "
            this.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail.id
                ++"->"
                ++ edge.head.id
                +=" "
            )
            result+=")"
            result.ToString()

        static member simple (id:Figure_id) (edges:seq<Vertex_id*Vertex_id>) =
            {
                id=id;
                edges=
                    edges
                    |>Seq.map (fun (tail_id, head_id)->
                        ai.Edge(
                            tail_id, head_id
                        );
                    )
                subfigures=dict ["a","b"]
            }

namespace rvinowise.ai.figure
    open rvinowise.ai

    module Example =

        let a_high_level_relatively_simple_figure = {
            id="F";
            edges=[
                figure.Edge(
                    Subfigure.simple "b0",Subfigure.simple("c")
                );
                figure.Edge(
                    Subfigure.simple("b0"),Subfigure.simple("d")
                );
                figure.Edge(
                    Subfigure.simple("c"),Subfigure.simple("b1")
                );
                figure.Edge(
                    Subfigure.simple("d"),Subfigure.simple("e")
                );
                figure.Edge(
                    Subfigure.simple("d"),Subfigure.simple("f0")
                );
                figure.Edge(
                    Subfigure.simple("e"),Subfigure.simple("f1")
                );
                figure.Edge(
                    Subfigure.simple("h"),Subfigure.simple("f1")
                );
                figure.Edge(
                    Subfigure.simple("b2"),Subfigure.simple("h")
                );
            ]
        }

    