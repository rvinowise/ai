namespace rvinowise.ai
    open FsUnit
    open Xunit

    [<Struct>]
    type Figure_node =
    | Lower_figure of Lower_figure: Figure_id
    | Concept of Concept: Figure_id
    // with 
    //     override this.ToString()=
    //         printed.Figure.to_string this.edges this.subfigures

    type Figure_vertex_data = Map<Vertex_id, Figure_node>


    module Figure_node =
        let to_string (node:Figure_node) =
            match node with 
            |Lower_figure figure_id -> figure_id
            |Concept concept_id -> "["+concept_id+"]"

        let ofString (name:string) =
            if Seq.length name >2 then
                match (Seq.head name, Seq.last name) with
                |('[',']') -> 
                    Concept (
                        name.[1..Seq.length name-2]
                    )
                |_->Lower_figure name
            else 
                Lower_figure name
        
        [<Fact>]
        let ``try ofString``()=
            "[number]"
            |>ofString
            |>should equal 
                (Concept "number")
            
            "constant"
            |>ofString
            |>should equal 
                (Lower_figure "constant")