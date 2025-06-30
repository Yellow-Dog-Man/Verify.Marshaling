using System.Reflection;
using System.Runtime.InteropServices;

namespace Verify.Marshaling.Utilities;

internal class MarshalRecord
{
    public string FieldName = string.Empty;
    public int Size = 0;
    public int Offset = 0;
    public IEnumerable<MarshalRecord> NestedRecords = [];

    public static MarshalRecord From(Type t)
    {
        var fields = t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            .Select(f => From(f, t));

        return new MarshalRecord()
        {
            FieldName = t.Name,
            Size = Marshal.SizeOf(t),
            NestedRecords = fields
        };
    }

    public static MarshalRecord From(FieldInfo f, Type parentType)
    {
        int size = 0;
        var a = f.GetCustomAttribute<MarshalAsAttribute>();

        if (f.FieldType.IsClass || a?.Value == UnmanagedType.Struct)
            return MarshalRecord.From(f.FieldType);
        else if (a == null)
            size = Marshal.SizeOf(f.FieldType);
        else if (f.FieldType.IsArray && f.FieldType.HasElementType)
        {
            var elementType = f.FieldType.GetElementType();
            size = Marshal.SizeOf(elementType!) * (a.GetSize() ?? 1);
        }
        else
            size = a.GetSize() ?? 0;

        return new MarshalRecord()
        {
            FieldName = f.Name,
            Size = size,
            Offset = (int)Marshal.OffsetOf(parentType, f.Name),
        };
    }
}

