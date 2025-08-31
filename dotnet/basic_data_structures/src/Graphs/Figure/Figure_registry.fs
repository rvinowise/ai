namespace rvinowise

open System.Collections.Generic

open rvinowise.ai


module Figure_registry =
    
    let signal_names = ResizeArray<string>()
    let signal_names_to_ids = Dictionary<string, Constant_figure_id>()
    
    
    let figure_names_to_ids = Dictionary<string, Constant_figure_id>()
    let figures = Set<Figure<Mapping_function_id>>()
    
    
    
    let private create_signal name =
        let index = Constant_figure_id signal_names.Count
        signal_names_to_ids[name] <- index
        signal_names.Add name
        index
    
    let provide_signal name =
        extensions.Dictionary.some_value signal_names_to_ids name
        |>function
        |Some existing_id -> existing_id
        |None -> create_signal name
        

    let provide_figure_id name figure =
        extensions.Dictionary.some_value figures figure
        |>function
        |Some existing_id -> existing_id
        |None -> create_signal name
    
    let id_into_name figure_id =
        signal_names[Constant_figure_id.value figure_id]
        
    