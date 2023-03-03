namespace rvinowise.ai
    open FsUnit
    open Xunit

    [<Struct>]
    type Figure_node =
    | Constant of Constant: Figure_id
    | Concept of Concept: Figure_id


    type Figure_vertex_data = Map<Vertex_id, Figure_node>


    module Figure_node =
        let to_string (node:Figure_node) =
            match node with 
            |Constant figure_id -> figure_id
            |Concept concept_id -> "["+concept_id+"]"

        let ofString (name:string) =
            if Seq.length name >2 then
                match (Seq.head name, Seq.last name) with
                |('[',']') -> 
                    Concept (
                        name.[1..Seq.length name-2]
                    )
                |_->Constant name
            else 
                Constant name
        
        [<Fact>]
        let ``try ofString``()=
            "[number]"
            |>ofString
            |>should equal 
                (Concept "number")
            
            "constant"
            |>ofString
            |>should equal 
                (Constant "constant")