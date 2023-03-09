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

    
    let sorted_subfigures_to_map_first 
        stencil
        target 
        =
        let first_subfigures_of_stencil = 
            Stencil.first_subfigures stencil
        
        let figures_to_map = 
            stencil
            |>Stencil.first_referenced_figures
            

        let subfigures_in_stencil = 
            figures_to_map
            |>Seq.map (
                Stencil.vertices_referencing_figure 
                    stencil
                    first_subfigures_of_stencil
                )
            
        let subfigures_in_target = 
            figures_to_map
            |>Seq.map (Figure.vertices_referencing_lower_figure target)
            |>Seq.map Array.ofSeq
            
        (subfigures_in_stencil, subfigures_in_target )

    [<Fact>]
    let ``sorted subfigures to map first``()=
        let target = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil

        sorted_subfigures_to_map_first
            stencil
            target
        |>should equal
            (
                [
                    [Vertex_id "b"];
                    [Vertex_id "h"]
                ],
                [
                    ["b0";"b1";"b2"]|>Seq.map Vertex_id;
                    [Vertex_id "h"]
                ]
            )

    let input_for_first_mappings_permutators subfigures_in_stencil subfigures_in_target =
        
        let amounts_in_stencil = 
            subfigures_in_stencil
            |>Seq.map Seq.length

        let amounts_in_target = 
            subfigures_in_target
            |>Seq.map Seq.length

        Seq.zip amounts_in_stencil amounts_in_target

           
    let prepared_generator_of_first_mappings
        subfigures_in_stencil
        subfigures_in_target
        =
        let generator = Generator_of_order_sequences<int[]>()
        
        (input_for_first_mappings_permutators subfigures_in_stencil subfigures_in_target)
        |>Seq.map Generator_of_mappings
        |>Seq.iter generator.add_order
        
        generator
        
    
    let mapping_of_subfigure
        (mapping: Mapping)
        (target_subfigures:Vertex_id[])
        target_subfigure_index
        subfigure_of_stencil
        =
        mapping.Add(subfigure_of_stencil, target_subfigures[target_subfigure_index])
        
    let mappings_of_figure
        mapping
        (used_occurances,
        subfigures_chosen_by_stencil,
        subfigures_available_in_target)
        =
        (used_occurances, subfigures_chosen_by_stencil)
        ||>Seq.iter2 (mapping_of_subfigure mapping subfigures_available_in_target)
        
        
    let mapping_from_generator_output
        subfigures_in_stencil
        subfigures_in_target
        indices
        =
        Contract.Requires ((Seq.length subfigures_in_stencil) = (Seq.length subfigures_in_target))
        let mapping = Mapping.empty()
        
        (indices, subfigures_in_stencil, subfigures_in_target)
        |||>Seq.zip3
        |>Seq.iter (mappings_of_figure mapping)
        
        mapping
        

    let map_first_nodes
        stencil
        target
        =
        let first_subfigures_of_stencil = Stencil.first_referenced_figures stencil
        let subfigures_in_stencil,
            subfigures_in_target =
                sorted_subfigures_to_map_first stencil target
        
        let generator = 
            (prepared_generator_of_first_mappings subfigures_in_stencil subfigures_in_target)
        
        generator
        |>Seq.map (
            mapping_from_generator_output 
                subfigures_in_stencil 
                subfigures_in_target
        )


    let next_unmapped_subfigures stencil mapped_nodes =
        []

    
        
    let (|Seq|_|) test input =
        if Seq.compareWith Operators.compare input test = 0
            then Some ()
            else None

    

    let copy_of_mapping_with_prolongation
        (mapping:Mapping)
        stencil_subfigure
        target_subfigure
        =
        let mapping = Mapping.copy mapping
        mapping[stencil_subfigure] <- target_subfigure
        mapping


    let mapping_prolongated_by_subfigures
        mapping
        stencil_subfigure
        target_subfigures
        =
        target_subfigures
        |>Seq.map (
            copy_of_mapping_with_prolongation mapping stencil_subfigure
        )

    
    let all_possible_mappings_of_one_next_subfigure
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
        (mappings: Map<Vertex_id, seq<Vertex_id>>) 
        =
        ()

    let prolongate_mapping_with_next_mapped_subfigures 
        (base_mapping: Mapping)
        (added_mappings: seq<Map<Vertex_id, Vertex_id>>)
        =
        added_mappings
        |>Seq.map


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
            (found_next_mappings: Map<Vertex_id, seq<Vertex_id>>)
            =

            match left_subfigures_to_map with
            | [] -> found_next_mappings
            | current_subfigure_to_map::left_subfigures_to_map ->
                let mappings = 
                    all_possible_mappings_of_one_next_subfigure
                        stencil
                        target
                        base_mapping
                        current_subfigure_to_map
                        
                if mappings.Count = 0 then 
                    //if at least one vertex can't be mapped -- 
                    //the whole mapping should be discarded
                    Map.empty
                else
                    let updated_mappings =
                        found_next_mappings
                        |>Map.add (fst current_subfigure_to_map) mappings
                    mappings_of_next_subfigure
                        stencil
                        target
                        mapping
                        left_subfigures_to_map
                        updated_mappings
        
        mappings_of_next_subfigure
            stencil
            target
            base_mapping
            next_subfigures_to_map
            Map.empty

    let prolongate_one_mapping_with_next_subfigures 
        stencil
        target
        (next_subfigures_to_map: seq<Vertex_id*Figure_id>)
        mapping
        =
        let possible_next_mappings =
            find_possible_next_mappings
                stencil
                target
                mapping
                (List.ofSeq next_subfigures_to_map)

        if possible_next_mappings.IsEmpty then
            []
        else
            possible_next_mappings
            |>all_combinations_of_next_mappings
            |>prolongate_mapping_with_next_mapped_subfigures mapping

    let rec prolongate_all_mappings 
        stencil
        target 
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
                |>Seq.collect 
                    (prolongate_one_mapping_with_next_subfigures stencil target next_subfigures_to_map)
        
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

    
