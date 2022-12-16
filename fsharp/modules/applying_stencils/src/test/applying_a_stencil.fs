namespace rvinowise.ai.test

open Xunit

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.figure.Applying_stencil
open rvinowise.ai.ui.painted.applying_stencil
open rvinowise.ai.ui
open rvinowise.ai.mapping_stencils


module Simple =
    
    open System.Text.RegularExpressions
    let remove_number label =
                Regex.Replace(label, @"[^a-zA-Z]", "")
                
    let Subfigure id =
        
        ai.figure.Subfigure(
            id,
            remove_number id
        )
        
        
module ``application of stencils``=
    open FsUnit
    open rvinowise.ai.stencil
    open rvinowise.ai
    open System.Collections.Generic
    
    type Used_figures()=
        
        member _.initial_mapping_without_prolongation = dict ["b","b1";"h","h"]
        member _.initial_fruitful_mapping = dict ["b","b0";"h","h"]
        member _.initial_useless_mapping = dict ["b","b2";"h","h"]
        
        member _.a_fitting_stencil: Stencil =
            rvinowise.ai.Stencil(
                "S",
                [
                    stencil.Edge(
                        Node("b"), Node.stencil_out("out1")
                    );
                    stencil.Edge(
                        Node.stencil_out("out1"), Node("f")
                    );
                    stencil.Edge(
                        Node("h"),Node("f")
                    );
                   
                ]
            )

        member _.a_high_level_relatively_simple_figure =
            Figure(
                "F",
                [
                    figure.Edge(
                        Simple.Subfigure("b0"),Simple.Subfigure("c")
                    );
                    figure.Edge(
                        Simple.Subfigure("b0"),Simple.Subfigure("d")
                    );
                    figure.Edge(
                        Simple.Subfigure("c"),Simple.Subfigure("b1")
                    );
                    figure.Edge(
                        Simple.Subfigure("d"),Simple.Subfigure("e")
                    );
                    figure.Edge(
                        Simple.Subfigure("d"),Simple.Subfigure("f0")
                    );
                    figure.Edge(
                        Simple.Subfigure("e"),Simple.Subfigure("f1")
                    );
                    figure.Edge(
                        Simple.Subfigure("h"),Simple.Subfigure("f1")
                    );
                    figure.Edge(
                        Simple.Subfigure("b2"),Simple.Subfigure("h")
                    );
                    
                ]
            )
            
        

    type ``prolongate a mapping with a subfigure``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        [<Fact>]
        member this.``normal case``()=    
            prolongate_mapping_with_subfigure
                this.figures.a_fitting_stencil
                this.figures.a_high_level_relatively_simple_figure
                this.figures.initial_useless_mapping
                (Simple.Subfigure("f"))
            |> should equal
                [
                    dict [
                        "b","b2";
                        "h","h";
                        "f","f1"
                    ]
                ]
            
        [<Fact>]
        member this.``empty sequence is returned, if a mapping can't be prolongated by this subfigure``()=    
            prolongate_mapping_with_subfigure
                this.figures.a_fitting_stencil
                this.figures.a_high_level_relatively_simple_figure
                this.figures.initial_mapping_without_prolongation
                (Simple.Subfigure("f"))
            |> should equal
                []
                
    type ``prolongate mapping``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        [<Fact>]
        member this.``impossible to prolongate, because of no matching following subfigures``()=
            prolongate_mapping 
                this.figures.a_fitting_stencil
                this.figures.a_high_level_relatively_simple_figure
                [Simple.Subfigure("f")]
                this.figures.initial_mapping_without_prolongation
            |> should equal
                []
        
        [<Fact>]
        member this.``prolongation with a single following node``()=
            prolongate_mapping 
                this.figures.a_fitting_stencil
                this.figures.a_high_level_relatively_simple_figure
                [Simple.Subfigure("f")]
                this.figures.initial_useless_mapping
            |> should equal
                [
                     dict [
                        "b","b2";
                        "h","h";
                        "f","f1"
                    ]
                ]
    
    type ``create a complete mapping of stencil onto target``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures
        
        // [<Fact>]
        // member this.``several mappings can be produced by one stencil application``()=
        //     let result =
        //         map_stencil_onto_target
        //             this.figures.a_fitting_stencil
        //             this.figures.a_high_level_relatively_simple_figure
        //             |> Set.ofSeq
        //     let expected =
        //         (Set.ofSeq [
        //             dict [
        //                 "b","b0";
        //                 "h","h";
        //                 "f","f1"
        //             ];
        //             dict [
        //                 "b","b2";
        //                 "h","h";
        //                 "f","f1"
        //             ]
        //         ])
        //     ()
    
    
    type ``apply a stencil``(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>
        member _.figures = used_figures

        [<Fact>]
        member this.``a fitting stencil, applied to a figure, outputs subgraphs``()=
            let target = this.figures.a_high_level_relatively_simple_figure
            let stencil = this.figures.a_fitting_stencil

            let output = results_of_stencil_application stencil target 
            ()
            //Assert.Equal(prolongation.expected, first_subfigures)

        [<Fact>]
        member this.``preparing inputs for permutators, which map initial nodes``()=
            
            let _, subfigures_in_stencil, subfigures_in_target =
                sorted_subfigures_to_map_first
                    (
                        Stencil.first_subfigures this.figures.a_fitting_stencil
                    )
                    this.figures.a_high_level_relatively_simple_figure
            
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
            let figure = this.figures.a_high_level_relatively_simple_figure
            let stencil = this.figures.a_fitting_stencil
            
            (
                map_first_nodes 
                    (Stencil.first_subfigures stencil) 
                    figure
            )
            |> should equal
                [   dict ["b","b0";"h","h"];
                    dict ["b","b1";"h","h"];
                    dict ["b","b2";"h","h"]
                ]
            
        [<Fact>]
        member this.``finding following subfigures referencing a specific figure``()=
            (subfigures_after_other_subfigures
                this.figures.a_high_level_relatively_simple_figure
                "f"
                ["b0"]
            )|> should equal
                ["f0";"f1"]

            (subfigures_after_other_subfigures
                this.figures.a_high_level_relatively_simple_figure
                "f"
                ["d";"b2"]
            )|> should equal
                ["f1"]

        
        
        [<Fact>]
        member this.``complete mapping of stencil onto target can be produced``()=
            let figure = this.figures.a_high_level_relatively_simple_figure
            let stencil = this.figures.a_fitting_stencil
            
            (map_stencil_onto_target stencil figure)
            |> should equal
                [
                    dict [
                        "b","b0";
                        "h","h";
                        "f","f0"; 
                    ];
                    dict [
                        "b","b1";
                        "h","h";
                        "f","f0";
                    ];
                    dict [
                        "b","b2";
                        "h","h";
                        "f","f0";
                    ];
                ]
                //|>Seq.map Set.ofSeq
            


        [<Fact>] //(Skip="ui")
        member this.``paint the target figure and the stencil``()=
            let figure = this.figures.a_high_level_relatively_simple_figure
            let stencil = this.figures.a_fitting_stencil

            figure.id
            |>painted.Graph.empty_root_graph 
            |>painted.Graph.provide_clustered_subgraph_inside_root_graph 
                "target figure" 
                (painted.Figure.painted_edges figure.edges)
            |>painted.Graph.provide_clustered_subgraph_inside_root_graph 
                "stencil"
                (painted.Stencil.painted_edges stencil.edges)
            |>painted.Graph.open_image_of_graph

        