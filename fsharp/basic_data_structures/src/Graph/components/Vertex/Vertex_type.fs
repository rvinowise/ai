

namespace rvinowise.ai
    open rvinowise.ai
    open System

    type Vertex=
        interface
            abstract member id:Node_id

            inherit IComparable
            inherit IComparable<Vertex>
            inherit IEquatable<Vertex>
   
        end

namespace rvinowise.ai.figure
    open rvinowise.ai
    open rvinowise
    open System

    [<CustomComparison; CustomEquality>]
    type Subfigure = 
        struct
            val id: Node_id
            val referenced: Figure_id

            new (id, referenced) =
                {id = id; referenced = referenced;}
            new (id: Node_id) =
                {id = id; referenced = id;}

            interface ai.Vertex with
                member this.id=this.id
            
            interface IComparable with
                member this.CompareTo other =
                    match other with
                    | :? Subfigure as other -> (this :> IComparable<_>).CompareTo other
                    | _ -> invalidArg "other" "not a Vertex"
            
            interface IComparable<Subfigure> with
                member this.CompareTo(other) =
                    compare this.id other.id
            interface IComparable<Vertex> with
                member this.CompareTo(other) =
                    compare this.id other.id
            interface IEquatable<Subfigure> with
                member this.Equals other =
                    this.id = other.id && this.referenced = other.referenced
            interface IEquatable<Vertex> with
                member this.Equals other =
                    this.id = other.id
            override this.Equals other =
                match other with
                | :? Subfigure as other -> (this :> IEquatable<_>).Equals other
                | :? Vertex as other -> (this :> IEquatable<Vertex>).Equals other
                | _ -> false
            override this.GetHashCode () =
                this.id.GetHashCode() ^^^ this.referenced.GetHashCode()

        end

namespace rvinowise.ai.stencil
    open rvinowise.ai
    open rvinowise

    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

    type Node =
        struct
            val id: Node_id
            val referenced: Node_reference

            new (id) =
                {id = id; referenced = Lower_figure id;}
            new (id, referenced: string) =
                {id = id; referenced = Lower_figure referenced;}
            new (id, referenced) =
                {id = id; referenced = referenced;}

            interface ai.Vertex with
                member this.id=this.id
        end

