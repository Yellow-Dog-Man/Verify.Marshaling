# Verify.Marshaling

Documentation is WIP, this library does not work right now!

When performing C# Interop/PInvoke with C++, you sometimes need to send structs from managed memory, to unmanaged memory.

In these cases .NET will perform Marshaling for you on your data. You can [customize this marshaling](https://learn.microsoft.com/en-us/dotnet/framework/interop/marshalling-classes-structures-and-unions) with attributes.

Verify.Marshaling allows you to perform Snapshot Testing on the memory layouts of your structs.

This can be useful in order to compare them to their C++ equivalents, or to lock their layout between versions of a library using the snapshots.


## Example

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
    await Verify(typeof(SimpleStruct));
}
```

Which should produce an output like:
```
{
  "Name": "SimpleStruct",
  "Size": 8,
  NestedRecords: [
    { 
		"Name": "Test",
		"Size": 4
	}
  ]
```

