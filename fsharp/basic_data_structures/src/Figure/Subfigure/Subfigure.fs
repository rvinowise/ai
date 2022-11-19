namespace rvinowise.ai.figure

open rvinowise.ai


type Subfigure = 
    struct
        val id: Subfigure_id
        val referenced: Figure_id
    end

