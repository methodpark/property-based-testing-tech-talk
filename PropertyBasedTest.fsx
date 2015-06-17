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



type Person = {
    Birthday: DateTime
    Name: string
}

let aNameIsNeverShorterThan4Chars (p: Person) =
    not (p.Name.Length < 4)

Check.Quick aNameIsNeverShorterThan4Chars

