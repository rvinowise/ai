namespace rvinowise.ai

    open rvinowise.ai

    type Concept = 
    |Leaf of Stencil
    |Or of Concept Set
    |And of Concept Set
    with 
        override this.ToString() =
            (match this with
            |Leaf stencil->
                stencil
                |>string
            |Or children->
                children
                |>Seq.map string
                |>String.concat ";"
                |>sprintf "Or(%s)"
            |And children->
                children
                |>Seq.map string
                |>String.concat ";"
                |>sprintf "And(%s)"
            )|>sprintf "Concept(%s)"