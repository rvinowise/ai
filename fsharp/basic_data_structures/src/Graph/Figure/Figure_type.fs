﻿namespace rvinowise.ai

    open System.Text
    open rvinowise.extensions
    open System.Collections.Generic

    type Vertex_data = IDictionary<Vertex_id, Figure_id>

    type Figure = {
        id: Figure_id
        edges: Edge seq

        subfigures: Vertex_data
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Figure_{this.id}( "
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

        


namespace rvinowise.ai.figure
    open rvinowise.ai
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
            
            vertex_data_from_edges_of_figure
                (dict ["a0","a";"b1","b"])
                [
                    Edge("a0","b1");
                    Edge("b1","a1")
                ]
            |>should throw typeof<System.ArgumentException>


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
                subfigures=
                    edges
                    |>Seq.map (fun(tail_id,head_id)->
                        [
                            (tail_id, Vertex.remove_number tail_id);
                            (head_id, Vertex.remove_number head_id)
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
                id=id;
                edges=edges;
                subfigures=Linking_vertices_to_data.vertex_data_from_edges_of_figure figure.subfigures edges

            }

        let from_tuples 
            (id:Figure_id)
            (edges:seq<Vertex_id*Figure_id*Vertex_id*Figure_id>) =
            {
                id=id;
                edges=
                    edges
                    |>Seq.map (fun (tail_id, _, head_id,_)->
                        Edge(
                            tail_id, head_id
                        );
                    )
                subfigures=Linking_vertices_to_data.vertex_data_from_tuples edges
            }

        let stencil_output (figure:Figure)(edges:Edge seq)=
            from_edges_of_figure "out" figure edges

        let empty id = from_tuples id []

    module Example =
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

    