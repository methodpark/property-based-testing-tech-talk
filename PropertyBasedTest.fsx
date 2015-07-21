#r "packages/NUnit.2.6.4/lib/nunit.framework.dll"
#r "packages/FsCheck.2.0.1/lib/net45/FsCheck.dll"
#r "bin/Debug/proptest-techtalk.exe"

open Implementation

open System
open FsCheck

type Tests() =
    static member ``addition is commutative`` x y =
        add x y = add y x

    static member ``addition is associative`` x =
        x |> add 1 |> add 1 = add x 2

    static member ``adding zero changes nothing`` x =
        add x 0 = x

Check.QuickAll typeof<Tests>

// How does it work?

/////////////
// Generators
/////////////

type Scale = int

let generator : Gen<Scale> = Gen.oneof [ gen { return 1 }; gen { return 3 }; gen { return 5; } ]

type MyGenerators () =
    static member Scale = 
        {new Arbitrary<Scale>() with 
            override x.Generator = generator
            }

Arb.register<MyGenerators>()

let ``Scale is always smaller than 6`` (s: Scale) =
    s < 6

Check.Quick ``Scale is always smaller than 6``


////////////
// Shrinking
////////////
type Person = {
    Name: string
    Age: int
}

// Based on the type this won't hold
let ``A person is not older than 75`` (p: Person) =
    p.Age < 75

Check.Quick ``A person is not older than 75``