namespace rvinowise.ai

open System.Runtime.InteropServices

[<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)>]
type Interval = 
    struct
        val start: Moment
        val finish: Moment

        new(start, finish) =
            {start=start;finish=finish}

        override this.ToString() =
            $"Interval({this.start}, {this.finish})"
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Interval =

    let moment (moment:Moment): Interval = 
        Interval(moment, moment)
    
    let regular start finish =
        Interval(start, finish)

    let from_int 
        (start: int)
        (finish: int)
        =
        regular (start) (finish)
    
    let ofPair tuple=
        let start, finish = tuple
        Interval(start, finish)
    //let moment (moment:int) = Interval(uint64(moment), uint64(moment))

    let start (interval:Interval)=
        interval.start
    let finish (interval:Interval)=
        interval.finish

    let bordering_interval intervals =
        regular 
            (intervals
            |>Seq.map start 
            |>Seq.min)
            (intervals
            |>Seq.map finish
            |>Seq.max)