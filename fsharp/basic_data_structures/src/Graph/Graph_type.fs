namespace rvinowise.ai

    open System.Text
    open rvinowise.extensions

    type Graph = 
        interface
        
            abstract member id: Figure_id
            abstract member edges: Edge seq

        end
