namespace rvinowise.ai.cpp_impl

open System
open rvinowise.ai

module Finding_repetitions =

  
    let prepared_array_for_results length: Interval array =
        Array.create length (Interval.moment 0)

    let repeated_pair 
        (heads: array<Interval>)
        (tails: array<Interval>)
        =
        let mutable repetitions = 
            (heads.Length, tails.Length)
            ||>min
            |>prepared_array_for_results

        let found_amount = Finding_repetitions_cpp.find_repeated_pairs(
                heads, heads.Length,
                tails, tails.Length,
                repetitions
            )
        Array.Resize(&repetitions, found_amount)
        repetitions

    let many_repetitions
        (figures: array<array<Interval>>)
        =
        ()