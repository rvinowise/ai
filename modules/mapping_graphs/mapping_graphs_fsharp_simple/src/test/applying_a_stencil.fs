namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise.ai.Applying_stencil
open rvinowise.ai.Mapping_graph_with_immutable_mapping
open rvinowise.ai.ui
open rvinowise.ai.stencil
open rvinowise.ui
        
        
module ``application of stencils``=
    
    
        
    let initial_mapping_without_prolongation = Immutable_mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"]
    let initial_fruitful_mapping = Immutable_mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"]
    let initial_useless_mapping = Immutable_mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        
    let result_of_fruitful_stencil_application =
        built.Figure.simple
            [
                "d","e"
            ]
              

    [<Fact>]
    let ``impossible to prolongate, because of no matching following subfigures``()=
        let possible_targets_for_mapping_vertex =
            targets_for_mapping_prolongation
                example.Figure.fitting_stencil_as_figure
                example.Figure.a_high_level_relatively_simple_figure
                initial_mapping_without_prolongation
                Map.empty
        
        prolongate_one_mapping_with_next_subfigures 
            possible_targets_for_mapping_vertex
            [(Vertex_id "f#1", Figure_id "f")]
            initial_mapping_without_prolongation
        |> should equal
            []
    
    [<Fact>]
    let``prolongation with a single following node``()=
        let possible_targets_for_mapping_vertex =
            targets_for_mapping_prolongation
                example.Figure.fitting_stencil_as_figure
                example.Figure.a_high_level_relatively_simple_figure
                initial_useless_mapping
                Map.empty
        
        prolongate_one_mapping_with_next_subfigures
            possible_targets_for_mapping_vertex
            [(Vertex_id "f#1", Figure_id "f")]
            initial_useless_mapping
        |> should equal
            [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2"
                ]
            ]
    

    
    [<Fact>]
    let ``several mappings can be produced by one stencil application``()=
        
        (
             map_figure_onto_target
                example.Figure.a_high_level_relatively_simple_figure
                example.Figure.fitting_stencil_as_figure
                |> Set.ofSeq
        )
        |>should equal (
            Set.ofSeq [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#1"
                    "h#1","h#1"
                    "f#1","f#2"
                ];
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2"
                    "h#1","h#1"
                    "f#1","f#2"
                ]
            ])

    [<Fact>]
    let ``a full mapping can be produced if the stencil has only "out" in the middle``()=
        let mappee = 
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]|>Figure_from_stencil.convert

        let target =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text
   
        let mappings =
            map_figure_onto_target
                mappee
                target
                |>Set.ofSeq
        
        mappings
        |>Seq.iter (fun mapping->
            mapping
            |>Seq.map (_.Key)
            |>Set.ofSeq
            |>should equal (
                ["N";",#1";",#2";";"]
                |>Seq.map Vertex_id
                |>Set.ofSeq
            )
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with several vertices)``()=
        results_of_stencil_application
            example.Figure.a_high_level_relatively_simple_figure
            example.Stencil.a_fitting_stencil
        |>Set.ofSeq
        |>should equal (
            [
                ["d","e"]|>built.Figure.simple;
                "h"|>built.Figure.signal 
            ]|>Set.ofList
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with only one vertex)``()=
        built.Stencil.simple_with_separator [
            "N","out";
            "out",";";
        ]
        |>results_of_stencil_application (built.Figure.sequential_figure_from_text "N0;")
        |>should equal [
            built.Figure.signal "0"
        ]
    
    [<Fact>]
    let ``a long fitting stencil, applied to a long figure, outputs subgraphs``()=
        results_of_stencil_application
            example.Figure.a_long_figure
            example.Stencil.a_long_stencil
        |>Set.ofSeq
        |>should equal (
            [
                ["k","l"]|>built.Figure.simple;
                ["m","n"]|>built.Figure.simple;
            ]|>Set.ofList
        )
    
    
    [<Fact>]
    let ``mapping of first stencil subfigures onto target produces initial mapping``()=
        Map_first_nodes.map_first_nodes_with_immutable_mapping
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
        |>Set.ofSeq
        |> should equal ([   
            Immutable_mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"];
            Immutable_mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"];
            Immutable_mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        ]|>Set.ofSeq)
    
    [<Fact>]
    let ``initial mapping when the target lacks some figures``()=
        Map_first_nodes.map_first_nodes_with_immutable_mapping
            (
                built.Figure.simple [
                    "b","f";
                    "h","f";
                    "x","f";
                ]
            )
            example.Figure.a_high_level_relatively_simple_figure
        |> should be Empty

    
    [<Fact>]
    let ``complete mapping of stencil onto target can be produced``()=
        
        map_figure_onto_target
            example.Figure.a_high_level_relatively_simple_figure
            example.Figure.fitting_stencil_as_figure
        |> should equal
            [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#1";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
            ]

            
    let ``paint the target figure and the stencil``()=
        let figure = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil

        "F"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_circle_vertices
        |>infrastructure.Graph.with_filled_vertex "target figure" 
            (painted.Graph.add_graph figure.edges)
        |>infrastructure.Graph.with_filled_vertex "stencil"
            (painted.Graph.add_graph stencil.edges)
        |>painted.image.open_image_of_graph

    [<Fact>]
    let ``apply stencil to a long mathematical sequence``()=
        let middle_digit_stencil =
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        middle_digit_stencil
        |>results_of_stencil_application history_as_figure
        |>Set.ofSeq
        |>should equal (
            "12345678"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )
        
    [<Fact>]
    let ``apply stencil, when some first mapped vertices whould be skipped``()=
        let last_digit_stencil =
            {
                built.Stencil.simple_with_separator [
                    "N","out";
                    ",#1","out";
                    "out",";";
                ] with
                    output_without=
                        ","|>built.Figure.signal|>Set.singleton
            }
        
        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        last_digit_stencil
        |>results_of_stencil_application history_as_figure
        |>should equal (
            "9"
            |>built.Figure.signal
            |>Seq.singleton
        )

    