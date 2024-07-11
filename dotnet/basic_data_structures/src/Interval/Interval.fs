namespace rvinowise.ai

open System.Runtime.InteropServices
open System


[<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)>]
type Interval = 
    struct
        val start: Moment
        val finish: Moment

        new(start, finish) =
            {start=start;finish=finish}

        override this.ToString() =
            if this.start = this.finish then
                $"{this.start}"
            else
                $"{this.start}-{this.finish}"
    end

module Interval =

    let moment (moment:Moment): Interval = 
        Interval(moment, moment)
    
    let regular start finish =
        Interval(start, finish)

    let from_int 
        (start: int)
        (finish: int)
        =
        regular start finish
    
    let ofPair tuple=
        let start, finish = tuple
        Interval(start, finish)

    let toPair (interval:Interval) =
        interval.start, interval.finish

    let start (interval:Interval)=
        interval.start
    let finish (interval:Interval)=
        interval.finish

    let bordering_interval_of_intervals intervals =
        regular 
            (intervals
            |>Seq.map start 
            |>Seq.min)
            (intervals
            |>Seq.map finish
            |>Seq.max)
    
    let bordering_interval_of_moments moments =
        regular 
            (moments|>Seq.min)
            (moments|>Seq.max)

    let intervals_to_string (intervals: Interval seq) =
        if Seq.isEmpty intervals then
            "[]"
        else
            String.Join (" ", 
                intervals
                |>Seq.map string 
            )
    
    let intersection_of_intervals
        (interval1:Interval) 
        (interval2:Interval) 
        =
        if 
            interval1.start > interval2.finish 
            ||
            interval2.start > interval1.finish 
        then
            None
        else
            Interval (
                max interval1.start interval2.start,
                min interval1.finish interval2.finish
            )    
            |>Some

    let intervals_intersect
        (interval1:Interval) 
        (interval2:Interval)
        =
        intersection_of_intervals
            interval1
            interval2
            <> None
        