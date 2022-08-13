namespace rvinowise.ai

type Figure_appearance = {
    start: int
    ending: int
}

module Figure_appearance =
    let print (appearance: Figure_appearance) =
        printf $"({appearance.start} {appearance.ending}) "