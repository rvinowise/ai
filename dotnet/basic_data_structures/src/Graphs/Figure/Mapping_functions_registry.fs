namespace rvinowise

open System
open System.Collections.Generic

open rvinowise.ai


module Mapping_functions_registry =
    
    let function_names = ResizeArray<string>()
    let function_names_to_ids = Dictionary<string, Mapping_function_id>()
    
    let mapping_functions = ResizeArray<Figure<_> -> Vertex_id -> bool> ()

    let is_vertex_suitable_for_mapping
        desired_target_subfigure
        (target_figure: Figure<_>)
        checked_vertex
        =
        Figure.is_vertex_referencing_target target_figure.targets desired_target_subfigure checked_vertex
    
    let remember_function name mapping_function =
        let next_id = Mapping_function_id function_names.Count
        function_names_to_ids[name] <- next_id
        mapping_functions.Add(mapping_function)
        function_names.Add(name)
        next_id
    
    let onto_exact_figure target_name =
        let desired_target_subfigure_id = Figure_registry.provide_signal target_name
        let mapping_function_name = $"constant_{target_name}"
        
        let mapping_function =
            is_vertex_suitable_for_mapping desired_target_subfigure_id
        
        extensions.Dictionary.some_value function_names_to_ids mapping_function_name
        |>function
        |Some existing_id -> existing_id
        |None -> remember_function mapping_function_name mapping_function
        

    let id_into_name function_id =
        try
            function_names[Mapping_function_id.value function_id]
        with
        | IndexOutOfRangeException as e ->
            Log.error $"no mapping function with id {function_id}"
        
    let name_into_id function_name =
        function_names_to_ids[function_name]
        
    let id_into_function function_id =
        mapping_functions[Mapping_function_id.value function_id]