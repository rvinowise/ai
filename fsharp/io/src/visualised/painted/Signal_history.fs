namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise
    open rvinowise.ai


    module Signal_history =
        
        let painted_ensamble 
            stencil
            vertex
            =
            let label = 
                match 
                    Stencil.referenced_node stencil vertex
                with
                |Lower_figure id -> id
                |Stencil_output -> "out"
            painted.Node(vertex, label)