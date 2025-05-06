namespace rvinowise.ai

open System.Diagnostics
open BenchmarkDotNet.Engines
open Xunit
open FsUnit
open rvinowise.extensions


module Concept =
    let appearances_of_concept_incarnations
        concept_incarnations
        (history: Figure)
        =
        let vertices_of_incarnations = 
            concept_incarnations
            |>Seq.map (fun figure->
                figure.subfigures
                |>Map.keys
                |>Set.ofSeq
            )
            |>Set.ofSeq

        concept_incarnations
        |>Seq.map (
            Mapping_graph_with_immutable_mapping.map_figure_onto_target
                history
        )|>Seq.concat
        |>Seq.filter (fun (appearance)->
            let appearance_vertices = 
                appearance.Keys
                |>Set.ofSeq
            
            vertices_of_incarnations
            |>Set.exists (fun incarnation_vertices->
                incarnation_vertices = appearance_vertices
            ) 
            |>not
        )
