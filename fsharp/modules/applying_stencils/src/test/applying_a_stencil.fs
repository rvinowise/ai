namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise.ai.figure.Applying_stencil
open rvinowise.ai.ui
open rvinowise.ai.mapping_stencils


        
module ``application of stencils``=
    
    open rvinowise.ai.stencil
    
    type Used_figures()=
        
        member _.initial_mapping_without_prolongation = Mapping ["b","b1";"h","h"]
        member _.initial_fruitful_mapping = Mapping ["b","b0";"h","h"]
        member _.initial_useless_mapping = Mapping ["b","b2";"h","h"]
            
        member _.result_of_fruitful_stencil_application =
            figure.built.simple
                "out"
                [
                        "d","e"
                ]
            

    type ``prolongate a mapping with a subfigure``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        [<Fact>]
        member this.``normal case``()=    
            prolongate_mapping_with_subfigure
                stencil.example.a_fitting_stencil
                figure.example.a_high_level_relatively_simple_figure
                this.figures.initial_useless_mapping
                "f"
            |> should equal
                [
                    Mapping [
                        "b","b2";
                        "h","h";
                        "f","f1"
                    ]
                ]
            
        [<Fact>]
        member this.``empty sequence is returned, if a mapping can't be prolongated by this subfigure``()=    
            prolongate_mapping_with_subfigure
                stencil.example.a_fitting_stencil
                figure.example.a_high_level_relatively_simple_figure
                this.figures.initial_mapping_without_prolongation
                "f"
            |> should equal
                []
                
    type ``prolongate mapping``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        [<Fact>]
        member this.``impossible to prolongate, because of no matching following subfigures``()=
            prolongate_mapping 
                stencil.example.a_fitting_stencil
                figure.example.a_high_level_relatively_simple_figure
                ["f"]
                this.figures.initial_mapping_without_prolongation
            |> should equal
                []
        
        [<Fact>]
        member this.``prolongation with a single following node``()=
            prolongate_mapping 
                stencil.example.a_fitting_stencil
                figure.example.a_high_level_relatively_simple_figure
                ["f"]
                this.figures.initial_useless_mapping
            |> should equal
                [
                    Mapping [
                        "b","b2";
                        "h","h";
                        "f","f1"
                    ]
                ]
    
    type ``create a complete mapping of stencil onto target``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        [<Fact>]
        member this.``several mappings can be produced by one stencil application``()=
            let result =
                map_stencil_onto_target
                    stencil.example.a_fitting_stencil
                    figure.example.a_high_level_relatively_simple_figure
                    |> Set.ofSeq
            let expected =
                (Set.ofSeq [
                    Mapping [
                        "b","b0";
                        "h","h";
                        "f","f1"
                    ];
                    Mapping [
                        "b","b2";
                        "h","h";
                        "f","f1"
                    ]
                ])
            ()
    
    
    type ``apply a stencil``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures

        [<Fact>]
        member this.``a fitting stencil, applied to a figure, outputs subgraphs``()=
            let target = figure.example.a_high_level_relatively_simple_figure
            let stencil = stencil.example.a_fitting_stencil
            target
            |>results_of_stencil_application stencil
            |>should equal [
                this.figures.result_of_fruitful_stencil_application
            ]


        [<Fact>]
        member this.``preparing inputs for permutators, which map initial nodes``()=
            
            let subfigures_in_stencil, subfigures_in_target =
                sorted_subfigures_to_map_first
                    stencil.example.a_fitting_stencil
                    figure.example.a_high_level_relatively_simple_figure
            
            let permutator_input = 
                input_for_first_mappings_permutators 
                    subfigures_in_stencil
                    subfigures_in_target
                                    
            permutator_input |> should equal [|
                (* b *)(1,3) ;
                (* h *)(1,1) ;
            |]

        [<Fact>]
        member this.``generator of initial mappings returns combinations``()=
            
            let generator = Generator_of_order_sequences<int[]>()
        
            generator.add_order(Generator_of_mappings(1, 3))
            let enumerator = generator.GetEnumerator()
            enumerator.MoveNext()
            |> should equal true
            enumerator.Current
            |> should equal [| [|0|] |]
            enumerator.MoveNext()
            |> should equal true
            enumerator.Current
            |> should equal [| [|1|] |]
            enumerator.MoveNext()
            |> should equal true
            enumerator.Current
            |> should equal [| [|2|] |]
            enumerator.MoveNext()
            |> should equal false

        
        [<Fact>]
        member this.``mapping of first stencil subfigures onto target produces initial mapping``()=
            let figure = figure.example.a_high_level_relatively_simple_figure
            let stencil = stencil.example.a_fitting_stencil
            
            (
                map_first_nodes 
                    stencil
                    figure
            )
            |> should equal
                [   Mapping ["b","b0";"h","h"];
                    Mapping ["b","b1";"h","h"];
                    Mapping ["b","b2";"h","h"]
                ]
            
        [<Fact>]
        member this.``finding following subfigures referencing a specific figure``()=
            (Figure.subfigures_after_other_subfigures
                figure.example.a_high_level_relatively_simple_figure
                "f"
                ["b0"]
            )|> should equal
                ["f0";"f1"]

            (Figure.subfigures_after_other_subfigures
                figure.example.a_high_level_relatively_simple_figure
                "f"
                ["d";"b2"]
            )|> should equal
                ["f1"]
        
        [<Fact>]
        member this.``complete mapping of stencil onto target can be produced``()=
            let figure = figure.example.a_high_level_relatively_simple_figure
            let stencil = stencil.example.a_fitting_stencil
            
            (map_stencil_onto_target stencil figure)
            |> should equal
                [
                    Mapping [
                        "b","b0";
                        "h","h";
                        "f","f1"; 
                    ];
                    Mapping [
                        "b","b2";
                        "h","h";
                        "f","f1";
                    ];
                ]
                //|>Seq.map Set.ofSeq
            


        [<Fact>] //(Skip="ui")
        member this.``paint the target figure and the stencil``()=
            let figure = figure.example.a_high_level_relatively_simple_figure
            let stencil = stencil.example.a_fitting_stencil

            figure.graph.id
            |>painted.Graph.empty_root_graph 
            |>painted.Graph.provide_clustered_subgraph_inside_root_graph 
                "target figure" 
                (painted.Figure.painted_edges figure)
            |>painted.Graph.provide_clustered_subgraph_inside_root_graph 
                "stencil"
                (painted.Stencil.painted_edges stencil)
            |>painted.Graph.open_image_of_graph

        