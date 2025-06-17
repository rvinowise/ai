namespace rvinowise.ai


type Apply_stencil_action = {
    stencil: Conditional_stencil
}

type Algorithm_action =
    |Apply_stencil_action 
    

type Algorithm_type = {
    actions: Algorithm_action seq
}
    
