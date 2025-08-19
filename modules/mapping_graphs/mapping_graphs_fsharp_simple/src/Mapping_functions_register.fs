namespace rvinowise.ai.stencil
    
open System.Collections.Generic
open rvinowise.ai
open System.Linq
open System

module Mapping_functions_register = 
    
    //let Mapping_type: Vertex_id -> bool
    let mapping_functions = Dictionary<Mapping_function_id, Vertex_id -> bool >()
    
    let mapping_id_to_function mapping_id =
        mapping_functions[mapping_id]
        
module Mapping_functions =
    
    let vertex_referencing_figure referenced_figure owner_figure vertex =
        Figure.is_vertex_referencing_target
            owner_figure.targets
            referenced_figure
            vertex