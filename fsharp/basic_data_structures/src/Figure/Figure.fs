namespace rvinowise.ai.figure

    open rvinowise.ai
    open rvinowise.extensions
    open System.Text

    type Figure(
        id, 
        edges
    ) =
        member this.id:Figure_id = id
        member this.edges: Edge seq = edges
        
        new (id) =
            Figure(id,[])

        override this.ToString() =
            let result = StringBuilder()
            
            result 
            += $"Figure_{this.id}( "
            
            edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail.id
                ++"->"
                ++ edge.head.id
                +=" "
                
            )
            result+=")"
            
            result.ToString()

