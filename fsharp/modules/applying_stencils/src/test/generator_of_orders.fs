namespace rvinowise.ai.test

open Xunit
open FsUnit
open rvinowise.ai.stencil
open rvinowise.ai
open rvinowise.ai.generating_combinations

module ``generator of orders``=
    
        
    [<Fact>]
    let ``using generator_of_orders in f#``()=    
        let generator = Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>> [
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct (
                    (Vertex_id "A1"),
                    seq{(Vertex_id "a1");(Vertex_id "a2");(Vertex_id "a3");(Vertex_id "a4")}
                );
                
                struct (
                    (Vertex_id "A2"),
                    seq{(Vertex_id "a1");(Vertex_id "a2");(Vertex_id "a3")}
                );
                
                struct (
                    (Vertex_id "A3"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                )
            });
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct ((Vertex_id "B1"), seq{(Vertex_id "b1");(Vertex_id "b2")});
                struct ((Vertex_id "B2"), seq{(Vertex_id "b1");(Vertex_id "b2")});
            });
        ]
    
        generator
        |>Seq.take 2
        |>should equal [
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a3"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A3"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a4"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A3"),(Vertex_id "a1"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
        ]
        
        
    [<Fact>]
    let ``using generator_of_orders in f# simple``()=    
        let generator = Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>> [
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct (
                    (Vertex_id "A1"),
                    seq{(Vertex_id "a1")}
                );
                
                struct (
                    (Vertex_id "A2"),
                    seq{(Vertex_id "a2")}
                );

            });
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct ((Vertex_id "B1"), seq{(Vertex_id "b1");(Vertex_id "b2")});
                struct ((Vertex_id "B2"), seq{(Vertex_id "b1");(Vertex_id "b2")});
            });
        ]
    
        generator
        |>Seq.take 2
        |>should equal [
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
        ]
        
    [<Fact>]
    let ``using generator_of_orders in f# average``()=    
        let generator = Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>> [
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct (
                    (Vertex_id "A1"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );
                
                struct (
                    (Vertex_id "A2"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );

            });
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct ((Vertex_id "B1"), seq{(Vertex_id "b1");(Vertex_id "b2")});
                struct ((Vertex_id "B2"), seq{(Vertex_id "b1");(Vertex_id "b2")});
            });
        ]
    
        generator
        |>should equal [
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            }
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
        ]
  
    [<Fact>]
    let ``preparing input for generator via sequence functions2``() =
        seq{
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct (
                    (Vertex_id "A1"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );
                
                struct (
                    (Vertex_id "A2"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );

            });
            Generator_of_mappings<Vertex_id,Vertex_id> (seq{
                struct ((Vertex_id "B1"), seq{(Vertex_id "b1");(Vertex_id "b2")});
                struct ((Vertex_id "B2"), seq{(Vertex_id "b1");(Vertex_id "b2")});
            });
        }
        |>Seq.cast< Element_to_target<Vertex_id, Vertex_id> seq seq> 
        |>Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>>
        |>should equal [
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            }
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
        ]
        
    [<Fact>]
    let ``preparing input for generator via sequence functions``() =
        seq{
            [
                struct (
                    (Vertex_id "A1"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );
                
                struct (
                    (Vertex_id "A2"),
                    seq{(Vertex_id "a1");(Vertex_id "a2")}
                );

            ];
            [
                struct (
                    (Vertex_id "B1"),
                    seq{(Vertex_id "b1");(Vertex_id "b2")}
                );
                
                struct (
                    (Vertex_id "B2"),
                    seq{(Vertex_id "b1");(Vertex_id "b2")}
                );

            ]
        }
        |>Seq.map Generator_of_mappings<Vertex_id,Vertex_id>
        |>Seq.cast< Element_to_target<Vertex_id, Vertex_id> seq seq> 
        |>Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>>
        |>should equal [
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b1"));
                ]
            }
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a2"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a1"));
                ]; 
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
            seq {
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A1"),(Vertex_id "a1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "A2"),(Vertex_id "a2"));
                ];
                [
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B1"),(Vertex_id "b1"));
                    Element_to_target<Vertex_id, Vertex_id>((Vertex_id "B2"),(Vertex_id "b2"));
                ]
            };
        ]