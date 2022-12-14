namespace rvinowise.ai.figure

open System.Diagnostics.Contracts
open rvinowise.ai.mapping_stencils
open rvinowise.ai

module Applying_stencil = 
    open System.Collections.Generic

    type Mapped_stencil = {
        subfigures: (Node_id*Node_id) Set
    }

    
    let sorted_subfigures_to_map_first first_subfigures_of_stencil target =
        //let first_subfigures_of_stencil = Stencil.first_subfigures stencil
        
        let figures_to_map = 
            first_subfigures_of_stencil
            |>Subfigures.referenced_figures

        let subfigures_in_target = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)
            |>Seq.map Subfigures.ids
            |>Seq.map Array.ofSeq
            
        let subfigures_in_stencil = 
            figures_to_map
            |>Seq.map (fun f->
                Subfigures.pick_referencing_figure f first_subfigures_of_stencil
            )
            |>Seq.map Subfigures.ids
            
        (figures_to_map, subfigures_in_stencil, subfigures_in_target )


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
        (mapping: Dictionary<Node_id, Node_id>)
        (target_subfigures:Node_id[])
        target_subfigure_index
        subfigure_of_stencil
        =
        mapping.Add(subfigure_of_stencil, target_subfigures[target_subfigure_index])
        
    let mappings_of_figure
        (mapping: Dictionary<Node_id, Node_id>)
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
        let mapping = Dictionary<Node_id, Node_id>()
        
        (indices, subfigures_in_stencil, subfigures_in_target)
        |||>Seq.zip3
        |>Seq.iter (mappings_of_figure mapping)
        |>ignore
        
        mapping
        

    let map_first_nodes
        first_subfigures_of_stencil
        target
        =
        let figures_to_map, 
            subfigures_in_stencil,
            subfigures_in_target =
                sorted_subfigures_to_map_first first_subfigures_of_stencil target
        
        let generator = 
            (prepared_generator_of_first_mappings subfigures_in_stencil subfigures_in_target)
        
        generator
        |>Seq.map (
            mapping_from_generator_output 
                subfigures_in_stencil 
                subfigures_in_target
        )

        
    // let next_subfigures_in_stencil (stencil:Stencil) mapped_subfigure =
    //     let stencil_subfigure, _ = mapped_subfigure
    //     stencil_subfigure
    //     |> Node.next_nodes stencil.edges
    //     |> Nodes.only_subfigures

    // let next_subfigures_in_target (target: Figure) mapped_subfigure =
    //     let _ , target_subfigure = mapped_subfigure
    //     mapped_subfigure
    //     |> Subfigure.next_subfigures target.edges



    let next_unmapped_subfigures stencil mapped_nodes =
        []

    
        
    let (|Seq|_|) test input =
        if Seq.compareWith Operators.compare input test = 0
            then Some ()
            else None


    let next_subfigures subfigures (stencil: Stencil)=
        subfigures
        |>Seq.collect (Node.next_nodes stencil.edges)
        |>Seq.distinct
        |>Nodes.only_subfigures

    let ``targets of mapping of stencil subfigures`` 
        (mapping: Dictionary<Node_id, Node_id>)
        subfigures 
        =
        subfigures
        |>Seq.map (fun s->
            mapping[s]
        )

    let subfigures_after_other_subfigures
        figure
        referenced_figure
        previous_subfigures
        =
        figure.edges

    let copy_of_mapping_with_prolongation
        (mapping: Dictionary<Node_id,Node_id>)
        stencil_subfigure
        target_subfigure
        =
        let mapping = Dictionary<Node_id,Node_id>(mapping)
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
        (stencil: Stencil)
        target
        mapping 
        subfigure
        =
        subfigure
        |>Node.previous_subfigures stencil.edges
        |>Subfigures.ids
        |>``targets of mapping of stencil subfigures`` mapping
        |>subfigures_after_other_subfigures
            target
            subfigure
        |>mapping_prolongated_by_subfigures
            mapping
            subfigure

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
        (last_mapped_subfigures: (Node_id)seq )
        mappings
        =
        let next_subfigures_to_map = 
            stencil
            |>next_subfigures last_mapped_subfigures
            |>Subfigures.ids

        let mappings =
            mappings
            |>Seq.collect (prolongate_mapping stencil target next_subfigures_to_map)

        match next_subfigures_to_map with
        | Seq [] -> 
            mappings
        | _ -> 
            prolongate_mappings
                stencil 
                target 
                next_subfigures_to_map
                mappings


    let map_stencil_onto_target
        stencil
        target 
        =
        let first_subfigures_of_stencil = Stencil.first_subfigures stencil
        
        target
        |>map_first_nodes first_subfigures_of_stencil
        |>prolongate_mappings
            stencil 
            target
            (
                first_subfigures_of_stencil
                |>Subfigures.ids
            )
        

    let retrieve_result target mapping =
        ()

    let results_of_stencil_application
        stencil
        target
        =
        target
        |>map_stencil_onto_target stencil
        //|>Seq.map (retrieve_result target)
    
