namespace rvinowise.ai

    open Xunit
    open FsUnit

    open System.Collections.Generic
    open System.Collections
    


    type Generator_of_orders_enumerator<'Iteration> 
        (orders: 'Iteration seq) 
        =
        
        interface IEnumerator<seq<'Mapped*'Target>> with
            member this.Dispose() =()
            
            member this.Current: seq<'Mapped*'Target> =
                []

        interface IEnumerator with
            member this.MoveNext():bool = 
                false
            
            member this.Reset() =()
            
            member this.Current: obj = box (
                (this:>IEnumerator<seq<'Mapped*'Target>>).Current
            )


    type Generator_of_orders<'Iteration>(orders: 'Iteration seq seq)
        =

        interface IEnumerable< seq<'Iteration> > with
            member this.GetEnumerator () =
                new Generator_of_orders_enumerator<'Iterationt>(orders)
        
        interface IEnumerable with
            member this.GetEnumerator () =
                (this:> IEnumerable<seq<'Iterationt>> ).GetEnumerator():>IEnumerator

    
    

    module ``test enumerating mappings of all figures`` =

        open rvinowise.ai.mapping_stencils

        [<Fact>]
        let ``enumerate over mappings of all figures``()=
            let generator = 
                new Generator_of_orders< seq<Vertex_id*Vertex_id> >
                [
                    new Generator_of_mappings<Vertex_id*Vertex_id> ([
                        Vertex_id "a1", ["a6";"a7";"a8"]|>Seq.map Vertex_id 
                        Vertex_id "a2", ["a7";"a8"]|>Seq.map Vertex_id 
                    ]|>Map.ofSeq);
                    new Generator_of_mappings<Vertex_id*Vertex_id> ([
                        Vertex_id "b1", ["b6";"b7"]|>Seq.map Vertex_id 
                        Vertex_id "b2", ["b7"]|>Seq.map Vertex_id 
                    ]|>Map.ofSeq)
                ]
            |>should equal [
                //1th iteration of the first order (figure a)
                [["a1","a6"; "a2","a7"]; ["b1","b6"; "b2","b7"]];
                [["a1","a6"; "a2","a7"]; ["b1","b7"; "b2","b7"]];
                
                //2th iteration of the first order (figure a)
                [["a1","a7"; "a2","a8"]; ["b1","b6"; "b2","b7"]];
                [["a1","a7"; "a2","a8"]; ["b1","b7"; "b2","b7"]];

                //3th iteration of the first order (figure a)
                [["a1","a8"; "a2","a7"]; ["b1","b6"; "b2","b7"]];
                [["a1","a8"; "a2","a7"]; ["b1","b7"; "b2","b7"]];
                
                
            ]
