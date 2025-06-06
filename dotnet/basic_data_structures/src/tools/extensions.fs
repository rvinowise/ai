﻿namespace rvinowise.extensions

open Xunit
open FsUnit
open BenchmarkDotNet.Attributes

[<AutoOpen>]
module StringBuilder=

    let (++) (left : System.Text.StringBuilder) (right : 't) : System.Text.StringBuilder =
        left.Append right

    let (+=) (left : System.Text.StringBuilder) (right : 't) : unit =
        left ++ right |> ignore


module Dictionary=
    open System.Collections.Generic
    open System.Linq

    let inline some_value (dictionary: IDictionary<'a,'b>) key  =
        let (exist, value) = dictionary.TryGetValue(key)
        if exist then
            Some value
        else    
            None

    let keys_with_value (dictionary: IDictionary<'a,'b>) value  =
        dictionary
            .Where(fun pair -> pair.Value = value)
            .Select(fun pair -> pair.Key);

    let getOrDefault key default' (dictionary: IDictionary<_,_>) =
        match dictionary.TryGetValue key with
        | true, value -> value
        | _ -> default'


module HashSet =
    open System.Collections.Generic
    open System.Linq
    
    let intersectMany
        (sets: IEnumerable<HashSet<'a> >)
        =
        if Seq.isEmpty sets then
            HashSet<'a>()
        else
            sets.Aggregate(fun set1 set2 -> 
                set1.IntersectWith(set2)
                set1
            )
    
    let intersectMany' (sets: HashSet<'a> seq) =
        if Seq.isEmpty sets then
            HashSet<'a>()
        else
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
       Benchmarking.run_benchmark<Benchmarking_hashSet>()

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


module String =
    open System.Text.RegularExpressions
    
    let remove_number label =
        Regex.Replace(label, @"[0-9]", "")
    let remove_number_with_hash label =
        Regex.Replace(label, @"#[0-9]", "")

    let remove_text_with_hash label =
        Regex.Replace(label, @"#.*", "")
    
    let split_into_same_symbols s =
        Regex("""(.)\1*""").Matches(s)
        |> Seq.cast<Match> 
        |> Seq.map (fun m -> m.Value)
        |> Seq.toList
    
    
module Map =

    let add_by_key 
        key
        element
        (map:Map< 'Key, Set<'Value> >)
        =
        let elements = 
            map.TryFind(key) 
            |>function
            |Some existing_elements -> existing_elements
            |None -> Set.empty
            |>Set.add element
        Map.add key elements map


    let getOrDefault key default' (map: Map<_,_>) =
        match map.TryGetValue key with
        | true, value -> value
        | _ -> default'

    let toPairs (map: Map<'Key,'Value>) = 
        map
        |>Seq.map(fun pair -> pair.Key, pair.Value)

    let add_map 
        (map1: Map<'Key, 'Value>)
        (map2: Map<'Key, 'Value>)
        = 
        map1
        |>Seq.append map2
        |>Seq.map(fun pair->pair.Key,pair.Value)
        |>Map.ofSeq
    
    let compareWith<'Key, 'Value when 'Key: comparison and 'Value: comparison>  
        (map1: Map<'Key, 'Value>)
        (map2: Map<'Key, 'Value>)
        =
        map1
        |>Seq.compareWith (fun pair1 pair2->
                let key_diff = compare pair1.Key pair2.Key
                if key_diff = 0 then
                    compare pair1.Value pair2.Value
                else
                    key_diff
            ) 
            map2

    let reverse_with_list_of_keys 
        (map: Map<'Key,'Value>) 
        :Map<'Value, 'Key list>
        = 
        Map.fold (fun (map: Map<'Value, 'Key list>) orig_key orig_value -> 
            let existing_keys = 
                map
                |>Map.tryFind orig_value
                |>function
                |None->[]
                |Some list -> list

            map.Add(orig_value,existing_keys@[orig_key])
        ) Map.empty map

    [<Fact>]
    let ``try reverse_with_list_of_keys``()=
        [
            "a1","a"; "a2","a"; "b1","b"; "b2","b";
        ]
        |>Map.ofList
        |>reverse_with_list_of_keys
        |>should equal (
            [
                "a",["a1";"a2"];
                "b",["b1";"b2"];
            ]|>Map.ofList
        )






module Option=
    exception LackingDataException of string
    
    let value_exc (option: 'T option) =
        match option with
        |Some value -> value
        |None -> raise (LackingDataException "the option must have a value, but it's None") 

    type OptionBuilder() =
        member x.Bind(v,f) = Option.bind f v
        member x.Return v = Some v
        member x.ReturnFrom o = o
        member x.Zero () = None

    let opt = OptionBuilder()