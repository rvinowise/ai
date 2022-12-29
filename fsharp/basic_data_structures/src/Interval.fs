namespace rvinowise.ai

open System.Runtime.InteropServices

[<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)>]
type Interval = 
    struct
        val head: Moment
        val tail: Moment

        new(head, tail) =
            {head=head;tail=tail}
        // new(head: int, tail: int) =
        //     {head=uint64(head);tail=uint64(tail)}

        override this.ToString() =
            $"Interval({this.head}, {this.tail})"
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Interval =

    let moment (moment:Moment): Interval = 
        Interval(moment, moment)
    
    let regular head tail =
        Interval(head, tail)

    let from_int 
        (head: int)
        (tail: int)
        =
        regular (uint64(head)) (uint64(tail))
    
    //let moment (moment:int) = Interval(uint64(moment), uint64(moment))