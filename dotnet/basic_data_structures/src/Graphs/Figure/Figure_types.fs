﻿
namespace rvinowise.ai

open rvinowise
open System
open System.Linq


type Mapping_function_id = Mapping_function_id of int

 
type Mapped_figure = {
    edges: Edge Set
    targets: Map<Vertex_id, Figure_id >
}

type Unmapped_figure = {
    edges: Edge Set
    targets: Map<Vertex_id, Figure_id >
}
         
[<CustomEquality; CustomComparison>]
type Figure = {
    edges: Edge Set
    subfigures: Map<Vertex_id, Figure_id >
}
with 
    override this.ToString()=
        this.subfigures
        |>Figure_printing.figure_to_string this.edges
    
    override this.Equals(other) =
        match other with
        | :? Figure as other ->
            Enumerable.SequenceEqual(this.edges, other.edges)
            && 
            Enumerable.SequenceEqual(this.subfigures, other.subfigures)
        | _ -> false
    
    override this.GetHashCode() =
        this.edges.GetHashCode() ^^^ this.subfigures.GetHashCode()

    interface IEquatable<Figure> with   
        member this.Equals other =
            this.Equals other
    
    member this.compare (other:Figure) =
        let subfigures_diff = 
            this.subfigures
            |>extensions.Map.compareWith other.subfigures
        if (subfigures_diff = 0) then
            this.edges
            |>Seq.compareWith Operators.compare other.edges
        else
            subfigures_diff
    
    interface IComparable with
        member this.CompareTo other =
            match other with
            | :? Figure as other ->
                this.compare other
            | _ -> invalidArg "other" "cannot compare value of different types"
    
    interface IComparable<Figure> with
        member this.CompareTo other =
            this.compare other



type Conditional_figure = {
    existing: Figure
    impossibles: Conditional_figure Set
}

module Conditional_figure =
    let from_figure figure =
        {
            existing = figure
            impossibles = Set.empty 
        }