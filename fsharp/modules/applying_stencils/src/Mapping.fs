namespace rvinowise.ai.stencil
    open System.Collections.Generic
    open rvinowise.ai
    open System.Linq
    open System
    open rvinowise.ai.figure

    type Mapped_vertex = KeyValuePair<Vertex_id,Vertex_id>
    type Mapping_dict = Dictionary<Vertex_id, Vertex_id>
    type IMapping_dict = IDictionary<Vertex_id, Vertex_id>

    (* mapping ot a stencil onto a figure, so that its subgraph can be retrieved,
    which is the output *)
    type Mapping = 
        inherit Dictionary<Vertex_id,Vertex_id>

        new() = {
            inherit Mapping_dict();
        }
    
        new(copied: IDictionary<Vertex_id,Vertex_id>) = {
            inherit Mapping_dict(copied);
        }
        new(content: (Vertex_id*Vertex_id) seq) as this = 
            {inherit Mapping_dict();} 
            then
                content
                |>Seq.iter (fun (tail, target) ->
                    this[tail] <- target
                )

        interface IComparable with
            
            member this.CompareTo other =
                
                let tail_of_mapped_node (pair:Mapped_vertex) =
                    pair.Key
                let target_of_mapped_node (pair:Mapped_vertex) =
                    pair.Value
                let compare_mapped_nodes
                    (this:Mapping) 
                    (other:Mapping) 
                    =
                    let this_items = 
                        this
                        |>Seq.map (fun pair->(pair.Key,pair.Value))
                        |>Seq.sort 
                    let other_items = 
                        other
                        |>Seq.map (fun pair->(pair.Key,pair.Value))
                        |>Seq.sort 

                    (this_items, other_items)
                    ||>Seq.zip
                    |>Seq.tryFind (fun (this_element, other_element) ->
                        this_element <> other_element
                    )
                    |> fun pair_with_difference ->
                        match pair_with_difference with
                        |Some (this, other) ->
                            //this.CompareTo(other)
                            if this < other then -1 else 1
                        |None -> 0

                match other with
                | :? Mapping as other -> 
                    if (this.Count <> other.Count) then
                        this.Count - other.Count
                    else 
                        compare_mapped_nodes this other
                | _ -> 1


        override this.Equals (other: obj) = 
           match other with 
           | :? Mapping as other -> 
            if (this.Count <> other.Count) then
                false
            else if (this.Keys.Except(other.Keys).Any()) then
                false
            else if (other.Keys.Except(this.Keys).Any()) then
                false
            else 
                this
                |>Seq.exists (fun pair ->
                    pair.Value <> other[pair.Key]
                )
                |>not
           | _ -> false

        override this.GetHashCode() =
            let mutable hash_code = 0
            this
            |>Seq.iter (fun (pair: Mapped_vertex) ->
                hash_code <- hash_code ^^^ pair.GetHashCode()
            )
            hash_code


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mapping=
        open rvinowise.ai.figure
        open rvinowise.ai
        open rvinowise
        
        let copy (copied:Mapping) =
            Mapping(copied)

        let empty(): Mapping =
            Mapping()

        
        let targets_of_mapping 
            (mapping:Mapping)
            (subfigures: Vertex_id seq) 
            =
            subfigures
            |>Seq.map (fun subfigure->
                mapping[subfigure]
            )

        let retrieve_result stencil target mapping =
            //i don't know how to handle stencils with multiple outputs. assume it's one for now
            let output_node = 
                stencil
                |>Stencil.outputs
                |>Seq.head

            let output_beginning =
                output_node
                |>ai.Edges.previous_vertices stencil.graph.edges
                |>targets_of_mapping mapping
                |>Edges.vertices_reacheble_from_other_vertices
                    Figure.need_every_vertex
                    target.graph.edges
                |>Set.ofSeq

            let output_ending =
                output_node
                |>ai.Edges.next_vertices stencil.edges
                |>targets_of_mapping mapping
                |>Edges.vertices_reaching_other_vertices
                    Figure.need_every_vertex
                    target.graph.edges
                |>Set.ofSeq
            
            output_beginning
            |>Set.intersect output_ending
            |>Figure.subgraph_with_vertices target