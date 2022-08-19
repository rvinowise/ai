namespace rvinowise.ai.test.ui

open System
open NUnit.Framework

open rvinowise.ai.ui.console

[<TestFixture>]
type TestClass () =

    [<Test>]
    member this.TestMethodPassing() =
        rvinowise.ai.ui.console.process_input "show appearances a"
        Assert.True(true)

    [<Test>]
     member this.FailEveryTime() = Assert.True(false)