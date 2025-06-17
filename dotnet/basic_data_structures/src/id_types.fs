namespace rvinowise.ai

open System

type Figure_id = |Figure_id of string
with 
    static member value (Figure_id value) = value
    static member (+) (this, other) =
        Figure_id (Figure_id.value this + Figure_id.value other)

type Vertex_id = |Vertex_id of string
with 
    static member value (Vertex_id value) = value
    static member (+) (this, other) =
        Vertex_id (Vertex_id.value this + Vertex_id.value other)

type Moment = int


module Vertex_id =
    open System.Text.RegularExpressions

    let remove_number label =
        Regex.Replace(Vertex_id.value label, @"[0-9]", "")
        |>Vertex_id
    let remove_number_with_hash label =
        Regex.Replace(Vertex_id.value label, @"#[0-9]", "")
        |>Vertex_id
    
    let ofFigure_id (figure_id:Figure_id) =
        figure_id
        |>Figure_id.value
        |>Vertex_id
        
        
 