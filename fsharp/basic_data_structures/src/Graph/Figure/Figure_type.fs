namespace rvinowise.ai.figure
    open System.Collections.Generic
    open rvinowise.ai

    type Vertex_data = IDictionary<Vertex_id, Figure_id>


namespace rvinowise.ai
    open System.Text
    open rvinowise.extensions
    open rvinowise.ai.figure
    
    type Figure = {
        graph: Graph
        subfigures: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Figure_{this.graph.id}( "
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

        


namespace rvinowise.ai.figure
    open rvinowise.ai
    open rvinowise.extensions
    open System.Diagnostics.Contracts
    open Xunit
    open FsUnit

    module Linking_vertices_to_data =

        let vertex_data_from_edges_of_figure (vertex_data:Vertex_data) (edges:Edge seq) =
            edges
            |>Edges.all_vertices
            |>Seq.map (fun vertex->
                let (data_exists, src_vertex_data) = vertex_data.TryGetValue(vertex)
                Contract.Assume(
                    (data_exists = true), 
                    "the taken edges of the provided figure must not have verticex, which are not in that figure"
                )
                if (not data_exists) then
                    invalidArg 
                        (nameof edges + " or " + nameof vertex_data)
                        "the taken edges of the provided figure must not have verticex, which are not in that figure"
                else
                    (vertex,src_vertex_data)
            )
            |>dict
            
        [<Fact>]
        let ``contract violation accessing verticex of a figure``()=
            Assert.Throws<System.ArgumentException>(fun()->
                vertex_data_from_edges_of_figure
                    (dict ["a0","a";"b1","b"])
                    [
                        Edge("a0","b1");
                        Edge("b1","a1")
                    ]
                |>ignore
            )


        let vertex_data_from_tuples edges=
            edges
            |>Seq.map (fun(tail_id,tail_fig,head_id,head_fig)->
                [
                    (tail_id, tail_fig);
                    (head_id, head_fig)
                ]
            )
            |>Seq.concat
            |>dict 


    module built=
        let simple id (edges:seq<Vertex_id*Vertex_id>) =
            {
                graph=graph.built.simple id edges
                subfigures=
                    edges
                    |>Seq.map (fun(tail_id,head_id)->
                        [
                            (tail_id, String.remove_number tail_id);
                            (head_id, String.remove_number head_id)
                        ]
                    )
                    |>Seq.concat
                    |>dict 
            }
        
        let from_edges_of_figure
            id
            (figure:Figure)
            (edges:Edge seq) =
            {
                graph={
                    id=id;
                    edges=edges
                }
                subfigures=Linking_vertices_to_data.vertex_data_from_edges_of_figure figure.subfigures edges

            }

        let from_tuples 
            (id:Figure_id)
            (edges:seq<Vertex_id*Figure_id*Vertex_id*Figure_id>) =
            {
                graph=graph.built.from_tuples id edges
                subfigures=Linking_vertices_to_data.vertex_data_from_tuples edges
            }

        let stencil_output figure (edges:Edge seq)=
            from_edges_of_figure "out" figure edges

        let empty id = from_tuples id []

    module example =
        open rvinowise.ai

        let a_high_level_relatively_simple_figure = 
            built.simple 
                "F" 
                [
                    ("b0","c");
                    ("b0","d");
                    ("c","b1");
                    ("d","e");
                    ("d","f0");
                    ("e","f1");
                    ("h","f1");
                    ("b2","h")
                ]

    