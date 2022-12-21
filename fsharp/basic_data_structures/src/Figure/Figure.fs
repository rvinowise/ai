namespace rvinowise.ai

    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure

    type Figure(
        id, 
        edges
    ) =
        member this.id:Figure_id = id
        member this.edges: Edge seq = edges
        
        new (id) =
            Figure(id,[])

        override this.ToString() =
            let result = StringBuilder()
            
            result 
            += $"Figure_{this.id}( "
            
            edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail.id
                ++"->"
                ++ edge.head.id
                +=" "
                
            )
            result+=")"
            
            result.ToString()

namespace rvinowise.ai.figure
    open rvinowise.ai
    
    module Example =

        let a_high_level_relatively_simple_figure = Figure(
            "F",
            [
                figure.Edge(
                    Subfigure.simple("b0"),Subfigure.simple("c")
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
        )