namespace rvinowise.ai
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit
 
    type Figure_history = {
        figure: Figure_id
        appearances: array<Interval>
    }

    type Mood = int

   
    type Mood_history = {
        interval: Interval
        mood_at_moments: seq<Mood>
    }


namespace rvinowise.ai
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure_history=
        open rvinowise.ai
        open FsUnit
        open Xunit


        let figure history =
            history.figure

        let mood_at_moment history moment=
            ()

        let has_repetitions history =
            Seq.length history.appearances > 1

        let start history =
            history.appearances
            |>Array.head 
            |>Interval.start

        let finish history =
            history.appearances
            |>Array.last 
            |>Interval.finish

namespace rvinowise.ai
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Mood_history=
        open rvinowise.ai
        open FsUnit
        open Xunit

        let interval (history:Mood_history) =
            history.interval


        let mood_at_moment history moment=
            ()

        let ofChanges mood_changes=
            ()
        
      



namespace rvinowise.ai.figure_history
    open Xunit
    open FsUnit
    open rvinowise.ai

    module built =
        
        let from_intervals 
            figure
            intervals
            =
            {
                figure=figure
                appearances=intervals
            }
        
        let from_tuples 
            figure
            tuples
            =
            from_intervals 
                figure
                (
                    tuples
                    |>Seq.map Interval.ofPair
                    |>Array.ofSeq
                )
            
        let from_moments 
            figure
            moments
            =
            let intervals = 
                moments
                |>Seq.map Interval.moment
                |>Array.ofSeq
            {
                figure=figure
                appearances=intervals
            }
        
        


namespace rvinowise.ai.mood_history
    open Xunit
    open FsUnit
    open rvinowise.ai
    open rvinowise

    module built =
        

        type Detailed_history={
            initial_mood:Mood
            mood_at_moments:Mood seq
        }
        let complete_from_tuples
            tuples
            =
            let mood_changes = 
                tuples
                |>Map.ofSeq
            
            {
                initial_mood = 0
                mood_at_moments=
                    mood_changes
                    |>Seq.pairwise
                    |>Seq.fold (
                        fun 
                            history
                            (
                                start,
                                finish
                            ) ->
                            let mood_change = start.Value
                            let amount_of_moments = finish.Key-start.Key
                            let new_mood = history.initial_mood + mood_change
                            {
                                initial_mood=new_mood
                                mood_at_moments=
                                    Seq.append 
                                        history.mood_at_moments
                                        (List.init amount_of_moments (fun _->new_mood))
                            }
                        
                        )
                        {
                            Detailed_history.initial_mood=0
                            mood_at_moments=[]
                        }
                    |>fun detailed_history->detailed_history.mood_at_moments
                    |>fun mood_at_moments ->
                        Seq.append
                            mood_at_moments
                            [
                                mood_changes
                                |>Seq.last
                                |>extensions.KeyValuePair.value
                                |>fun last_mood_change->
                                    (mood_at_moments
                                    |>Seq.last)
                                    +
                                    last_mood_change
                            ]

            }
        
        [<Fact>]
        let ``mood history build complete_from_tuples``()=
            [
                10,1; 15,1; 16,-2; 20,3
            ]
            |>complete_from_tuples
            |>should equal {
                interval=Interval.ofPair (10,20)
                mood_at_moments=[
                    1;1;1;1;1;
                    2;
                    0;0;0;0;
                    3
                ]
            }
    module example=
        let short_history_of_mood=
            Map.ofSeq [
                11,1;
                13,1; 
                17,-2;
                20,3
            ]


     
        
namespace rvinowise.ai.figure_history
    open Xunit
    open FsUnit
    open rvinowise.ai

    module example=
        let short_history_with_some_repetitions=
            figure_history.built.from_tuples "a" [
                    10,15;
                    11,16;
                    15,17;
                    20,20
                ]
        
        let another_history_for_combining_togetner=
            figure_history.built.from_tuples "b" [
                    11,12;
                    12,14;
                    13,17;
                    20,24
                ]

        

    


