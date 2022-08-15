namespace rvinowise.ai

open System.Collections.Generic


type Figure = {
    id: string
    appearances: Figure_appearance list
    edges: Edge list
} and Edge = {
    start: Figure
    ending: Figure
}

 
    
    