namespace rvinowise.ai.ui.painted

    module Graph =
        open Rubjerg.Graphviz
        open rvinowise.ai
        open rvinowise.ai.ui
        open rvinowise.ui
        open rvinowise.ui.infrastructure
        open System.IO
        open System.Diagnostics

        let empty_root_graph name =
            let root = External_root.CreateNew(name, GraphType.Directed)
            root.SafeSetAttribute("rankdir", "LR", "")
            External_node.IntroduceAttribute(root, "shape", "circle")
            root
        

        


        


      