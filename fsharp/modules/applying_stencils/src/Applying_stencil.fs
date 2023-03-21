namespace rvinowise.ai
open Xunit
open FsUnit

module Applying_stencil = 
    open System.Collections.Generic
    open System.Diagnostics.Contracts
    open Rubjerg.Graphviz
    open rvinowise.ai.mapping_stencils
    open rvinowise.ai.stencil
    open rvinowise   

 
   
    let mapping_from_generator_output
        (chosen_targets: seq<Element_to_target<Vertex_id,Vertex_id>>)
        =
        let mapping = Mapping.empty()
        
        chosen_targets
        |>Seq.iter (fun pair ->
            mapping.Add(pair.element, pair.target)    
        )
        mapping

    let map_first_nodes
        stencil
        target
        =
        let figures_to_map = 
            stencil
            |>Stencil.first_referenced_figures
            
        let first_subfigures_of_stencil = 
            Stencil.first_subfigures stencil

        figures_to_map
        |>Seq.map (fun figure->
            let subfigures_in_target =
                Figure.vertices_referencing_lower_figure target figure
            figure
            |>Stencil.vertices_referencing_figure 
                    stencil
                    first_subfigures_of_stencil
            |>Seq.map (fun subfigure_in_stencil->
                Element_to_targets<Vertex_id,Vertex_id> (subfigure_in_stencil,subfigures_in_target);
            )
            |>Generator_of_individualised_mappings<Vertex_id, Vertex_id>
        )
        |>Seq.cast
        |>Generator_of_orders<IEnumerable<Element_to_target<Vertex_id,Vertex_id>>>
        |>Seq.map (Seq.collect id)
        |>Seq.map mapping_from_generator_output
        
       
        
    let (|Seq|_|) test input =
        if Seq.compareWith Operators.compare input test = 0
            then Some ()
            else None

    

    let copied_mapping_with_prolongation
        mapping
        (added_mappings: seq<Element_to_target<Vertex_id, Vertex_id>>)
        =
        let mapping = Mapping.copy mapping
        added_mappings
        |>Seq.iter (fun added_mapping ->
            mapping[added_mapping.element] <- added_mapping.target
        )
        mapping

    
    let possible_targets_for_mapped_subfigure
        stencil
        target
        mapping  
        (prolongating_stencil_subfigure: Vertex_id*Figure_id)
        =
        let prolongating_vertex = prolongating_stencil_subfigure|>fst
        let prolongating_figure = prolongating_stencil_subfigure|>snd
        prolongating_vertex
        |>Stencil.previous_subfigures_jumping_over_outputs stencil
        |>Mapping.targets_of_mapping mapping
        |>Figure.subfigures_after_other_subfigures
            target
            prolongating_figure

    let all_combinations_of_next_mappings 
        (mappings: (Vertex_id*(Vertex_id seq)) seq) 
        =
        ()

    let prolongate_mapping_with_next_mapped_subfigures 
        (base_mapping: Mapping)
        (added_mappings: seq<seq<Element_to_target<Vertex_id, Vertex_id>>>)
        =
        added_mappings
        |>Seq.map (copied_mapping_with_prolongation base_mapping)


    let find_possible_next_mappings
        stencil
        target
        base_mapping
        next_subfigures_to_map
        =
        let rec mappings_of_next_subfigure
            (stencil:Stencil)
            (target:Figure)
            (mapping:Mapping)
            (left_subfigures_to_map:  list<Vertex_id*Figure_id>)
            (found_next_mappings: list<(Vertex_id*seq<Vertex_id>)>)
            =

            match left_subfigures_to_map with
            | [] -> found_next_mappings
            | current_subfigure_to_map::left_subfigures_to_map ->
                let targets = 
                    possible_targets_for_mapped_subfigure
                        stencil
                        target
                        base_mapping
                        current_subfigure_to_map
                        
                if targets.Count = 0 then 
                    []
                else
                    mappings_of_next_subfigure
                        stencil
                        target
                        mapping
                        left_subfigures_to_map
                        ((current_subfigure_to_map|>fst, targets)::found_next_mappings)
        
        mappings_of_next_subfigure
            stencil
            target
            base_mapping
            next_subfigures_to_map
            []

    let prolongate_one_mapping_with_next_subfigures 
        (stencil:Stencil)
        (target:Figure)
        (next_subfigures_to_map: seq<Vertex_id*Figure_id>)
        (mapping:Mapping)
        =
        let possible_next_mappings =
            find_possible_next_mappings
                stencil
                target
                mapping
                (List.ofSeq next_subfigures_to_map)

        if possible_next_mappings.IsEmpty then
            Seq.singleton mapping
        else
            possible_next_mappings
            |>all_combinations_of_next_mappings
            |>prolongate_mapping_with_next_mapped_subfigures mapping

    let rec prolongate_all_mappings 
        (stencil:Stencil)
        (target:Figure)
        (last_mapped_vertices: Vertex_id seq )
        (mappings: Mapping seq)
        =
        let next_vertices_to_map = 
            stencil
            |>Stencil.next_vertices last_mapped_vertices
        

        let mappings =
            let next_subfigures_to_map =
                next_vertices_to_map
                |>Stencil.only_subfigures_with_figures stencil
            match next_subfigures_to_map with
            | Seq [] -> mappings
            |_->
                mappings
                |>Seq.collect (
                    prolongate_one_mapping_with_next_subfigures stencil target next_subfigures_to_map
                )
        
        match next_vertices_to_map with
        | Seq [] -> 
            mappings
        | _ ->
            prolongate_all_mappings
                stencil 
                target 
                next_vertices_to_map
                mappings
        
        

    let map_stencil_onto_target
        stencil
        target
        =
        target
        |>map_first_nodes stencil
        |>prolongate_all_mappings
            stencil 
            target
            (Stencil.first_subfigures stencil)
            
        
    let results_of_stencil_application
        target
        stencil
        =
        target
        |>map_stencil_onto_target stencil
        |>Seq.map (Mapping.retrieve_result stencil target)
        |>Seq.filter Figure.has_edges

    
