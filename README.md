# Verify.Marshaling

[![Nuget downloads](https://img.shields.io/nuget/v/YellowDogMan.Verify.Marshaling.svg)](https://www.nuget.org/packages/YellowDogMan.Verify.Marshaling)
[![Nuget](https://img.shields.io/nuget/dt/YellowDogMan.Verify.Marshaling)](https://www.nuget.org/packages/YellowDogMan.Verify.Marshaling)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Yellow-Dog-Man/Verify.Marshaling/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.txt)

When performing C# Interop/PInvoke with C++, you sometimes need to send structs from managed memory, to unmanaged memory.

In these cases .NET will perform Marshaling for you on your data. You can [customize this marshaling](https://learn.microsoft.com/en-us/dotnet/framework/interop/marshalling-classes-structures-and-unions) with attributes.

Verify.Marshaling allows you to perform Snapshot Testing on the memory layouts of your structs.

This can be useful in order to compare them to their C++ equivalents, or to lock their layout between versions of a library.

## Setup

### Setup Verify

If you're new to Verify, we recommend checking out the [Verify getting started wizard](https://github.com/VerifyTests/Verify/blob/main/docs/wiz/readme.md). 

### Install Package

Install [Verify.Marshaling with NuGet](https://www.nuget.org/packages/YellowDogMan.Verify.Marshaling):

```
Install-Package YellowDogMan.Verify.Marshaling
```

### Use

Given a struct:
```cs
[StructLayout(LayoutKind.Sequential)]
public struct SimpleStruct
{
    [MarshalAs(UnmanagedType.I4)]
    public int Test;

    [MarshalAs(UnmanagedType.R4)]
    public float TestF;
}
```

You can verify its layout with:
```cs
[TestMethod]
public async Task BasicTest()
{
    await VerifyMemoryLayout(typeof(SimpleStruct));
}
```

Which should produce an output like:
```
{
  FieldName: SimpleStruct,
  Size: 8,
  Nested: [
    {
      FieldName: Test,
      Size: 4,
      Type: Int32
    },
    {
      FieldName: TestF,
      Size: 4,
      Offset: 4,
      Type: Single
    }
  ],
  Type: struct
}
```
That can be saved as a part of your Snapshot collection, in the same way as [Verify](https://github.com/VerifyTests/Verify)

## Future Plans

### Diagrams
Projects like [StructLayout](https://github.com/Viladoman/StructLayout/tree/main) and even [Visual Studio](https://devblogs.microsoft.com/visualstudio/size-alignment-and-memory-layout-insights-for-c-classes-structs-and-unions/#memory-layout). 
Generate visual diagrams of a struct's layout. 

Tooling to generate these for C# may be handy as an alternative method to verify the layout.

### Formats

There are a number of tools for C++ that can examine memory layouts. 

- [Clang](https://eli.thegreenplace.net/2012/12/17/dumping-a-c-objects-memory-layout-with-clang)

Matching these formats, may allow for automated tests that produce C++ sourced CLang dumps, that we can then compare with Verify.Marshaling snapshots. We could also convert between them.