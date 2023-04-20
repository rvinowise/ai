namespace rvinowise.ai



module Mapping_graph = 
    open System.Collections.Generic
    open rvinowise.ai.generating_combinations
    open rvinowise.ai.stencil
    open rvinowise
    open rvinowise.ai.mapping_graph_impl


    let map_first_nodes = Map_first_nodes.map_first_nodes

    let all_combinations_of_next_mappings 
        (mappings: Map<Figure_id, struct (Vertex_id*seq<Vertex_id>) list>) 
        =
        mappings
        |>Seq.map (fun pair->
            Generator_of_mappings<Vertex_id,Vertex_id> pair.Value
        )
        |>Work_with_generators.mapping_combinations_from_generators


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

    let prolongate_mapping_with_next_mapped_subfigures 
        (base_mapping: Mapping)
        //                                                       all_vertices   combinations               
        (added_mappings: Element_to_target<Vertex_id, Vertex_id> seq            seq         )
        =
        added_mappings
        |>Seq.map (copied_mapping_with_prolongation base_mapping)

    let possible_targets_for_mapping_subfigure
        mappee
        target
        mapping
        (prolongating_stencil_subfigure: Vertex_id*Figure_id)
        =
        let prolongating_vertex = prolongating_stencil_subfigure|>fst
        let prolongating_figure = prolongating_stencil_subfigure|>snd
        prolongating_vertex
        |>Edges.previous_vertices mappee.edges
        |>Mapping.targets_of_mapping mapping
        |>Figure.subfigures_after_other_subfigures
            Edges.only_first_suitable_vertices
            target
            prolongating_figure

        

    let next_mapping_targets_for_mapped_subfigures
        mappee
        target
        base_mapping
        next_subfigures_to_map
        =
        let rec mapping_targets_for_next_subfigure
            (mappee:Figure)
            (target:Figure)
            (mapping:Mapping)
            (left_subfigures_to_map:  list<Vertex_id*Figure_id>)
            //                   mapping_generator  stencil_vertex possible_targets
            (found_mappings: Map<Figure_id,  struct(Vertex_id   *  seq<Vertex_id>)  list>)
            =

            match left_subfigures_to_map with
            | [] -> found_mappings
            | current_subfigure_to_map::left_subfigures_to_map ->
                let targets = 
                    possible_targets_for_mapping_subfigure
                        mappee
                        target
                        base_mapping
                        current_subfigure_to_map
                    
                if targets.Count = 0 then 
                    Map.empty
                else
                    let updated_mappings =
                        let figure = snd current_subfigure_to_map
                        let updated_targets =
                            struct(current_subfigure_to_map|>fst, targets|>Seq.cast)
                            ::
                            (found_mappings
                            |>extensions.Map.getOrDefault figure [])
                        found_mappings
                        |>Map.add figure updated_targets
                    mapping_targets_for_next_subfigure
                        mappee
                        target
                        mapping
                        left_subfigures_to_map
                        updated_mappings
        
        mapping_targets_for_next_subfigure
            mappee
            target
            base_mapping
            next_subfigures_to_map
            Map.empty

    let prolongate_one_mapping_with_next_subfigures 
        (mappee:Figure)
        (target:Figure)
        (next_subfigures_to_map: seq<Vertex_id*Figure_id>)
        (mapping:Mapping)
        =
        let possible_next_mappings =
            next_mapping_targets_for_mapped_subfigures
                mappee
                target
                mapping
                (List.ofSeq next_subfigures_to_map)

        if possible_next_mappings.IsEmpty then
            Seq.empty
        else
            possible_next_mappings
            |>all_combinations_of_next_mappings
            |>prolongate_mapping_with_next_mapped_subfigures mapping
    
    let rec prolongate_all_mappings 
        (mappee:Figure)
        (target:Figure)
        (last_mapped_vertices: Vertex_id seq )
        (mappings: Mapping seq)
        =
        let next_vertices_to_map = 
            last_mapped_vertices
            |>Edges.next_vertices_of_many mappee.edges

        let mappings =
            let next_subfigures_to_map =
                next_vertices_to_map
                |>Figure.vertices_with_referenced_figures mappee
            if Seq.isEmpty next_subfigures_to_map then
                mappings
            else
                mappings
                |>Seq.map (prolongate_one_mapping_with_next_subfigures mappee target next_subfigures_to_map)
                |>Seq.collect id
        
        if Seq.isEmpty next_vertices_to_map then
            mappings
        else
            prolongate_all_mappings
                mappee 
                target 
                next_vertices_to_map
                mappings


    let map_figure_onto_target
        target
        mappee
        =
        target
        |>map_first_nodes mappee
        |>prolongate_all_mappings
            mappee 
            target
            (Figure.first_vertices mappee)