
module ExampleBasedTest

open Implementation

open NUnit.Framework
open FsUnit


[<Test>]
let ``adding 1 and 2 returns 3`` () =
    add 1 2 |> should equal 3

[<Test>]
let ``adding 0 does not increment the value`` () =
    add 10 0 |> should equal 10

[<Test>]
let ``adding a number and its negative 0 is returned`` () =
    add 120 -120 |> should equal 0

// So, is the implementation error free??