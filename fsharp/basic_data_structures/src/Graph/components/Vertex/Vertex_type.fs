
namespace rvinowise.ai
    open rvinowise.ai
    open System

    type Vertex=
        interface
            abstract member id:Vertex_id

            inherit IComparable
            //inherit IComparable<Vertex>
            //inherit IEquatable<Vertex>
   
        end


namespace rvinowise.ai.figure
    open rvinowise.ai
    open rvinowise
    open System

    [<CustomComparison; CustomEquality>]
    type Subfigure = 
        struct
            val id: Vertex_id
            val referenced: Figure_id

            new (id, referenced) =
                {id = id; referenced = referenced;}
            new (id) =
                {id = id; referenced = id;}

            interface ai.Vertex with
                member this.id=this.id
            

            interface IComparable<Subfigure> with
                member this.CompareTo(other) =
                    match (compare this.id other.id) with
                    |0-> compare this.referenced other.referenced
                    |unequal -> unequal

            interface IComparable with
                member this.CompareTo other =
                    match other with
                    | :? Subfigure as other -> (this :> IComparable<_>).CompareTo other
                    | _ -> invalidArg "other" "not a Vertex"
            
            
            interface IEquatable<Subfigure> with
                member this.Equals other =
                    this.id = other.id && this.referenced = other.referenced
            interface IEquatable<Vertex> with
                member this.Equals other =
                    match other with
                    | :? Subfigure -> (this :> IEquatable<Subfigure>).Equals other
                    | _ -> false
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
    open System

    [<Struct>]
    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output

    [<CustomComparison; CustomEquality>]
    type Node =
        struct
            val id: Vertex_id
            val referenced: Node_reference

            new (id) =
                {id = id; referenced = Lower_figure id;}
            new (id, referenced: string) =
                {id = id; referenced = Lower_figure referenced;}
            new (id, referenced) =
                {id = id; referenced = referenced;}

            interface ai.Vertex with
                member this.id=this.id

            interface IComparable<Node> with
                member this.CompareTo(other) =
                    match (compare this.id other.id) with
                    |0-> compare this.referenced other.referenced
                    |unequal -> unequal
            interface IComparable with
                member this.CompareTo other =
                    match other with
                    | :? Node as other -> (this :> IComparable<_>).CompareTo other
                    | _ -> invalidArg "other" "not a Vertex"
            interface IEquatable<Node> with
                member this.Equals other =
                    this.id = other.id && this.referenced = other.referenced
            interface IEquatable<Vertex> with
                member this.Equals other =
                    this.id = other.id
            override this.Equals other =
                match other with
                | :? Node as other -> (this :> IEquatable<_>).Equals other
                | :? Vertex as other -> (this :> IEquatable<Vertex>).Equals other
                | _ -> false
            override this.GetHashCode () =
                this.id.GetHashCode() ^^^ this.referenced.GetHashCode()
        end




    

