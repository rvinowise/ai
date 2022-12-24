namespace rvinowise.ai.stencil
    open System.Collections.Generic
    open rvinowise.ai
    open System.Linq
    open System

    type Mapping = 
        inherit Dictionary<Node_id,Node_id>

        new() = {
            inherit Dictionary<Node_id, Node_id>();
        }
    
        new(copied: IDictionary<Node_id, Node_id>) = {
            inherit Dictionary<Node_id,Node_id>(copied);
        }
        new(content: (Node_id*Node_id) seq) as this = 
            {inherit Dictionary<Node_id,Node_id>();} 
            then
                content
                |>Seq.iter (fun (tail, target) ->
                    this[tail] <- target
                )

        interface IComparable with
            
            member this.CompareTo other =
                
                let tail_of_mapped_node (pair:KeyValuePair<Node_id, Node_id>) =
                    pair.Key
                let target_of_mapped_node (pair:KeyValuePair<Node_id, Node_id>) =
                    pair.Value
                let compare_mapped_nodes
                    (this:Mapping) 
                    (other:Mapping) 
                    (compared_element: KeyValuePair<Node_id, Node_id> -> Node_id)
                    =
                    let this_tails = 
                        this
                        |>Seq.map (fun pair -> compared_element pair)
                        |>Seq.sort 
                    let other_tails = 
                        other
                        |>Seq.map (fun pair -> compared_element pair)
                        |>Seq.sort 

                    (this_tails, other_tails)
                    ||>Seq.zip
                    |>Seq.tryFind (fun (this_element, other_element) ->
                        this_element <> other_element
                    )
                    |> fun pair_with_difference ->
                        match pair_with_difference with
                        |Some (this, other) ->
                            this.CompareTo(other)
                        |None -> 0

                match other with
                | :? Mapping as other -> 
                    if (this.Count <> other.Count) then
                        this.Count - other.Count
                    else 
                        let comparison_of_targets = 
                            compare_mapped_nodes this other target_of_mapped_node
                        if comparison_of_targets = 0 then
                            compare_mapped_nodes this other tail_of_mapped_node
                        else
                            comparison_of_targets
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
            |>Seq.iter (fun (pair: KeyValuePair<Node_id,Node_id>) ->
                hash_code <- hash_code ^^^ pair.GetHashCode()
            )
            hash_code


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mapping=
        open rvinowise.ai.figure
        
        let copy (copied:IDictionary<Node_id, Node_id>): Mapping =
            Mapping(copied)

        let empty(): Mapping =
            Mapping()

        
        let targets_of_mapping 
            (mapping:Mapping)
            subfigures 
            =
            subfigures
            |>Seq.map (fun s->
                mapping[s]
            )

        let retrieve_result stencil target mapping =
            //i don't know how to handle stencils with multiple outputs. assume it's one for now
            let output_node = 
                stencil
                |>Stencil.outputs
                |>Seq.head

            let output_beginning =
                output_node.id
                |>Edges.previous_vertices stencil
                |>Subfigures.ids
                |>targets_of_mapping mapping
                |>figure.Edges.subfigures_reacheble_from_other_subfigures
                    (fun _->true)
                    target.edges
                |>Set.ofSeq

            let output_ending =
                output_node.id
                |>Figure.next_subfigures stencil
                |>Subfigures.ids
                |>targets_of_mapping mapping
                |>Edges.subfigures_reaching_other_subfigures
                    target
                |>Set.ofSeq
            
            output_beginning
            |>Set.intersect output_ending
            |>Figure.subgraph_with_subfigures target