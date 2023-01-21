namespace rvinowise.ai.figure
open Xunit
open FsUnit

module Applying_stencil = 
    open System.Collections.Generic
    open System.Diagnostics.Contracts
    open Rubjerg.Graphviz
    open rvinowise.ai.mapping_stencils
    open rvinowise.ai.stencil
    open rvinowise.ai
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
                    ["b"];
                    ["h"]
                ],
                [
                    [|"b0";"b1";"b2"|];
                    [|"h"|]
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

    let prolongate_mapping_with_subfigure
        stencil
        target
        mapping  
        (prolongating_stencil_subfigure: Vertex_id)
        =
        prolongating_stencil_subfigure
        |>Stencil.previous_subfigures_jumping_over_outputs stencil
        |>Mapping.targets_of_mapping mapping
        |>Figure.subfigures_after_other_subfigures
            target
            prolongating_stencil_subfigure
        |>mapping_prolongated_by_subfigures
            mapping
            prolongating_stencil_subfigure

    let prolongate_mapping 
        stencil
        target
        next_subfigures_to_map
        mapping
        =
        next_subfigures_to_map
        |>Seq.collect (
            prolongate_mapping_with_subfigure
                stencil
                target
                mapping
        )

    let rec prolongate_mappings 
        stencil
        target 
        (last_mapped_subfigures: Vertex_id seq )
        (mappings: Mapping seq)
        =
        let next_subfigures_to_map = 
            stencil
            |>Stencil.next_subfigures last_mapped_subfigures

        match next_subfigures_to_map with
        | Seq [] -> 
            mappings
        | _ ->
            let (mappings:Mapping seq) = 
                mappings
                |>Seq.collect 
                    (prolongate_mapping stencil target next_subfigures_to_map)
            prolongate_mappings
                stencil 
                target 
                next_subfigures_to_map
                mappings
        

    let map_stencil_onto_target
        stencil
        target
        =
        target
        |>map_first_nodes stencil
        |>prolongate_mappings
            stencil 
            target
            (Stencil.first_referenced_figures stencil)
            
        
    let results_of_stencil_application
        stencil
        target
        =
        target
        |>map_stencil_onto_target stencil
        |>Seq.map (Mapping.retrieve_result stencil target)
        |>Seq.filter Figure.has_edges

    
