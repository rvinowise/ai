namespace rvinowise.ai

    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure

    type Figure = {
        id: Figure_id
        edges: Edge seq

        
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

    type Figure2(
        id, 
        edges
    ) =
        member this.id:Figure_id = id
        member this.edges: Edge seq = edges
        
        new (id) =
            Figure2(id,[])

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
    open Xunit
    open FsUnit
    
    //[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure =
        
        let regular (id:string) (edges:Edge seq)=
            {id=id;edges=edges}
            
        let stencil_output (edges:Edge seq)=
            regular "out" edges

        let empty id = regular id []

        [<Fact>]
        let ``equality comparison``()=
            let f1 = regular "F" [
                Edge(Subfigure("a0","a"), Subfigure("b0","b"))
            ]
            let f2 = regular "F" [
                Edge(Subfigure("a0","a"), Subfigure("b0","b"))
            ]
            f1 |>should equal f2


    module Example =

        let a_high_level_relatively_simple_figure = {
            id="F";
            edges=[
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
        }

    