namespace rvinowise.extensions

open Xunit
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<AutoOpen>]
module Extensions=
    open System.Text

    let (++) (left : System.Text.StringBuilder) (right : 't) : System.Text.StringBuilder =
        left.Append right

    let (+=) (left : System.Text.StringBuilder) (right : 't) : unit =
        left ++ right |> ignore


module HashSet =
    open System.Collections.Generic
    open System.Linq
    
    
    
    let intersectMany
        (source: IEnumerable<HashSet<'a> >)
        =
        source.Aggregate(fun set1 set2 -> 
            set1.IntersectWith(set2)
            set1
        )
    
    let intersectMany' (sets: HashSet<'a> seq) =
        sets
        |>Seq.skip 1
        |>Seq.fold 
            (
                fun (accumulator:HashSet<'a>) (set1:HashSet<'a>) ->
                    set1.IntersectWith(accumulator)
                    set1
            )
            (Seq.head sets)
        
    

    type Benchmarking_hashSet() =
        
        [<Params(10, 100)>]
        member val items_amount = 0 with get, set
        

        [<Benchmark>]
        member this.intersection()=
            let set1 = HashSet([1..this.items_amount])
            let set2 = HashSet([this.items_amount/2..this.items_amount+this.items_amount/2])
            
            [set1;set2]
            |>intersectMany
        
        [<Benchmark>]
        member this.intersection_2()=
            let set1 = HashSet([1..this.items_amount])
            let set2 = HashSet([this.items_amount/2..this.items_amount+this.items_amount/2])
            
            [set1;set2]
            |>intersectMany'

    [<Fact(Skip="long")>]
    let benchmark_hashSet()=
       benchmark<Benchmarking_hashSet>()

    [<Fact>]
    let ``intersect many hash sets``()=
        let sets = [
            HashSet(["a";"b";"c";"d"]);
            HashSet(["_";"b";"c";"d";"e"]);
            HashSet(["a";"b";"c";"_";"_"]);
        ]
        let intersection = HashSet(["b";"c"])
        
        sets
        |>intersectMany
        |>should equal intersection

    [<Fact>]
    let ``intersect with only one set``()=
        let sets = [
            HashSet(["a";"b";"c";"d"]);
        ]
        
        sets
        |>intersectMany
        |>should equal
            (sets
            |>Seq.head)
    
    [<Fact>]
    let ``intersect with an empty sequence of sets``()=
        []
        |>intersectMany
        |>should equal
            (HashSet())
    
    [<Fact>]
    let ``intersect' many hash sets``()=
        let sets = [
            HashSet(["a";"b";"c";"d"]);
            HashSet(["_";"b";"c";"d";"e"]);
            HashSet(["a";"b";"c";"_";"_"]);
        ]
        let intersection = HashSet(["b";"c"])
        
        sets
        |>intersectMany'
        |>should equal intersection
    
    [<Fact>]
    let ``intersect' with an empty sequence of sets``()=
        []
        |>intersectMany'
        |>should equal
            (HashSet())