
namespace rvinowise.ai

open rvinowise
open System
open System.Linq



type IFigure =
    abstract member edges: Edge Set
    abstract member targets: Map<Vertex_id, int>


//can be used by a function which is referenced by a Mapping_function_id
type Unmapped_figure = {
    edges: Edge Set
    targets: Map<Vertex_id, Mapping_function_id>
}
with
    interface IFigure with
        member this.edges = this.edges
        member this.targets =
            this.targets
            |>Map.map (fun _ target -> Mapping_function_id.value target)
 
// type Constant_figure = { //can be referenced by a Constant_figure_id
//     edges: Edge Set
//     targets: Map<Vertex_id, Constant_figure_id>
// }

         
[<CustomEquality; CustomComparison>]
type Constant_figure = { //can be referenced by a Constant_figure_id
    edges: Edge Set
    targets: Map<Vertex_id, Constant_figure_id >
}
with
    interface IFigure with
        member this.edges = this.edges
        member this.targets =
            this.targets
            |>Map.map (fun _ target -> Constant_figure_id.value target)
    override this.ToString()=
        this.targets
        |>Map.map (fun vertex _ -> Vertex_id.value vertex )
        |>Figure_printing.figure_to_string this.edges 
    
    override this.Equals(other) =
        match other with
        | :? Constant_figure as other ->
            Enumerable.SequenceEqual(this.edges, other.edges)
            && 
            Enumerable.SequenceEqual(this.targets, other.targets)
        | _ -> false
    
    override this.GetHashCode() =
        this.edges.GetHashCode() ^^^ this.targets.GetHashCode()

    interface IEquatable<Constant_figure> with   
        member this.Equals other =
            this.Equals other
    
    member this.compare (other:Constant_figure) =
        let subfigures_diff = 
            this.targets
            |>extensions.Map.compareWith other.targets
        if (subfigures_diff = 0) then
            this.edges
            |>Seq.compareWith Operators.compare other.edges
        else
            subfigures_diff
    
    interface IComparable with
        member this.CompareTo other =
            match other with
            | :? Constant_figure as other ->
                this.compare other
            | _ -> invalidArg "other" "cannot compare value of different types"
    
    interface IComparable<Constant_figure> with
        member this.CompareTo other =
            this.compare other



type Conditional_figure = {
    existing: Constant_figure
    impossibles: Conditional_figure Set
}

module Conditional_figure =
    let from_figure figure =
        {
            existing = figure
            impossibles = Set.empty 
        }

