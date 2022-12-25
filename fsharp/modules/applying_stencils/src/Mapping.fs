namespace rvinowise.ai.stencil
    open System.Collections.Generic
    open rvinowise.ai
    open System.Linq
    open System
    open rvinowise.ai.figure

    type Mapping = 
        inherit Dictionary<Node_id,Subfigure>

        new() = {
            inherit Dictionary<Node_id, Subfigure>();
        }
    
        new(copied: IDictionary<Node_id, Subfigure>) = {
            inherit Dictionary<Node_id,Subfigure>(copied);
        }
        new(content: (Node_id*Subfigure) seq) as this = 
            {inherit Dictionary<Node_id,Subfigure>();} 
            then
                content
                |>Seq.iter (fun (tail, target) ->
                    this[tail] <- target
                )

        interface IComparable with
            
            member this.CompareTo other =
                
                let tail_of_mapped_node (pair:KeyValuePair<Node_id, Subfigure>) =
                    pair.Key
                let target_of_mapped_node (pair:KeyValuePair<Node_id, Subfigure>) =
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
            |>Seq.iter (fun (pair: KeyValuePair<Node_id,Subfigure>) ->
                hash_code <- hash_code ^^^ pair.GetHashCode()
            )
            hash_code


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mapping=
        open rvinowise.ai.figure
        open rvinowise.ai
        open rvinowise
        
        let copy (copied:IDictionary<Node_id, Subfigure>): Mapping =
            Mapping(copied)

        let empty(): Mapping =
            Mapping()

        
        let targets_of_mapping 
            (mapping:Mapping)
            (subfigures: Subfigure seq) 
            =
            subfigures
            |>Seq.map (fun subfigure->
                mapping[subfigure.id]
            )

        let retrieve_result stencil target mapping =
            //i don't know how to handle stencils with multiple outputs. assume it's one for now
            let output_node = 
                stencil
                |>Stencil.outputs
                |>Seq.head

            let output_beginning =
                output_node
                |>ai.Edges.previous_vertices stencil.edges
                |>targets_of_mapping mapping
                |>Edges.vertices_reacheble_from_other_vertices
                    Figure.need_every_subfigure
                    target.edges
                |>Set.ofSeq

            let output_ending =
                output_node
                |>ai.Edges.next_vertices stencil.edges
                |>targets_of_mapping mapping
                |>Edges.vertices_reaching_other_vertices
                    Figure.need_every_subfigure
                    target.edges
                |>Set.ofSeq
            
            output_beginning
            |>Set.intersect output_ending
            |>Figure.subgraph_with_vertices target