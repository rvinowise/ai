namespace rvinowise.ai.stencil
    open System.Collections.Generic
    open rvinowise.ai
    open System.Linq

    type Mapping = IDictionary<Node_id,Node_id>


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mapping=
        
        let equals (mapping1:Mapping) (mapping2:Mapping) =
            (
                mapping1.Count = mapping2.Count 
                && 
                mapping1.Except(mapping2).Any() |>not
            );

        let copy (copied:Mapping) :Mapping =
            Dictionary<Node_id,Node_id>(copied)

        let empty: Mapping =
            Dictionary<Node_id,Node_id>()