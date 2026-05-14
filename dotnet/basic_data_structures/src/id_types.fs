namespace rvinowise.ai

open System

type Figure_id = |Figure_id of string
with 
    static member value (Figure_id value) = value
    static member (+) (this, other) =
        Figure_id (Figure_id.value this + Figure_id.value other)

type Numerical_id =
    abstract member value: int
    //static abstract member nonexistent: unit -> 'Numerical_id
    //abstract member nonexistent: unit -> Numerical_id

type Mapping_function_id = Mapping_function_id of int
with
    static member value (Mapping_function_id value) = value
    static member nonexistent () = Mapping_function_id -1
    
    interface Numerical_id with
        //static member nonexistent () = Mapping_function_id.nonexistent ()
        member this.value = Mapping_function_id.value this
        
    
    
type Constant_figure_id = Constant_figure_id of int
with
    static member value (Constant_figure_id value) = value
    static member (+) (this, other) =
        Constant_figure_id (Constant_figure_id.value this + Constant_figure_id.value other)
    
    static member nonexistent () = Constant_figure_id -1
    
    interface Numerical_id with
        //member nonexistent () = Constant_figure_id.nonexistent ()
        member this.value = Constant_figure_id.value this




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
        
        
module Numeric_id =
    let inline nonexisting_id< ^Target when ^Target : (static member nonexistent: unit -> ^Target)>  = 
     (^Target : (static member  nonexistent: unit -> ^Target) () )