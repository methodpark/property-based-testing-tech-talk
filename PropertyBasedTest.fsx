#r "packages/NUnit.2.6.4/lib/nunit.framework.dll"
#r "packages/FsCheck.2.0.1/lib/net45/FsCheck.dll"
#r "Core/bin/Debug/Core.dll"

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



// For many of the default types:

(Arb.from<String>).Generator.Sample (10, 10)
(Arb.from<System.IO.DriveType>).Generator.Sample (10, 10)

type Cache = Map<String, float>
(Arb.from<Cache>).Generator.Sample (10, 10)

// For F# types
type Foo = 
    | Bar
    | Baz of DateTime
    | Bazinga of int * String

(Arb.from<Foo>).Generator.Sample (10, 10)

type Poco (bytes: byte[]) = // see? even arrays
    member x.MP3 = bytes

(Arb.from<Poco>).Generator.Sample (10, 10)

// Define your own:
type Scale = Scale of int // Should only be 1, 3 or 5

type MyGenerators () =
    static member Scale = 
        Arb.from<int>
        |> Arb.filter (fun s -> match s with
                                | 1 | 3 | 5 -> true
                                | _ -> false)
        |> Arb.convert (fun s -> Scale(s)) (fun (Scale(s)) -> s)

Arb.register<MyGenerators>()

let ``Scale is always smaller than 6`` (scale: Scale) =
    let (Scale(s)) = scale
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
    p.Age <= 75

Check.Quick ``A person is not older than 75``