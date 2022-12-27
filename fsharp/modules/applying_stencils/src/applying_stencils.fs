namespace rvinowise.ai.figure


module Applying_stencil = 
    open System.Collections.Generic
    open System.Diagnostics.Contracts
    open Rubjerg.Graphviz
    open rvinowise.ai.mapping_stencils
    open rvinowise.ai.stencil
    open rvinowise.ai
    open rvinowise

    
    let sorted_subfigures_to_map_first 
        (first_subfigures_of_stencil: Vertex_id seq) 
        target =
        
        let figures_to_map = 
            first_subfigures_of_stencil
            |>Figure.referenced_figures target

        let subfigures_in_target = 
            figures_to_map
            |>Seq.map (Figure.vertices_referencing_lower_figure target)
            |>Seq.map Array.ofSeq
            
        let subfigures_in_stencil = 
            figures_to_map
            |>Seq.map (fun referenced_figure->
                Figure.referenced_figures 
                    referenced_figure 
                    first_subfigures_of_stencil
            )
            
        (subfigures_in_stencil, subfigures_in_target )


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
        first_subfigures_of_stencil
        target
        =
        let subfigures_in_stencil,
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


    let next_unmapped_subfigures stencil mapped_nodes =
        []

    
        
    let (|Seq|_|) test input =
        if Seq.compareWith Operators.compare input test = 0
            then Some ()
            else None


    let next_subfigures subfigures (stencil: Stencil)=
        subfigures
        |>Seq.collect (ai.stencil.Edges.next_nodes stencil.edges)
        |>Seq.distinct
        |>Nodes.only_subfigures

    

    let rec subfigures_reacheble_from_edges
        (reached_goals: HashSet<Vertex_id>)
        all_edges
        goal_figure
        reaching_edges
        =
        reaching_edges
        |>Seq.filter (fun (edge:figure.Edge)->
            edge.head.referenced = goal_figure
        )
        |>Seq.iter (fun edge -> 
            reached_goals.Add(edge.head.id) |> ignore
        )

        reaching_edges
        |>Seq.map (Edges.next_edges all_edges)
        |>Seq.iter (
            subfigures_reacheble_from_edges 
                reached_goals 
                all_edges
                goal_figure
        )
        |>ignore

    let subfigures_reacheble_from_subfigure 
        (figure: Figure)
        goal_figure
        (starting_subfigure:Vertex_id)
        =
        let edges = Edges.outgoing_edges figure.edges starting_subfigure
        let reached_goals = HashSet<Vertex_id>()
        subfigures_reacheble_from_edges 
            reached_goals
            figure.edges
            goal_figure
            edges
        |>ignore

        reached_goals

    let subfigures_after_other_subfigures
        (figure_in_which_search: Figure)
        figure_referenced_by_goal_subfigures
        (subfigures_before_goals: Vertex_id seq)
        =
        subfigures_before_goals
        |>Seq.map (
            subfigures_reacheble_from_subfigure 
                figure_in_which_search 
                figure_referenced_by_goal_subfigures
        )
        |>Seq.map Set.ofSeq
        |>Seq.reduce Set.intersect

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
        (stencil: Stencil)
        target
        mapping  
        (prolongating_stencil_subfigure: Subfigure)
        =
        prolongating_stencil_subfigure.id
        |>Edges.previous_subfigures_jumping_over_outputs stencil.edges
        |>Vertex.ids
        |>Mapping.targets_of_mapping mapping
        |>subfigures_after_other_subfigures
            target
            (prolongating_stencil_subfigure.referenced)
        |>mapping_prolongated_by_subfigures
            mapping
            prolongating_stencil_subfigure.id

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
            |>next_subfigures last_mapped_subfigures

        match next_subfigures_to_map with
        | Seq [] -> 
            mappings
        | _ ->
            let mappings =
                mappings
                |>Seq.collect (prolongate_mapping stencil target next_subfigures_to_map)
            prolongate_mappings
                stencil 
                target 
                (Vertex.ids next_subfigures_to_map)
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
                |>Vertex.ids
            )
        

    let private not_empty_figure (figure:Figure) =
        figure.edges
        |>Seq.isEmpty|>not

    let results_of_stencil_application
        stencil
        target
        =
        target
        |>map_stencil_onto_target stencil
        |>Seq.map (Mapping.retrieve_result stencil target)
        |>Seq.filter not_empty_figure

    
