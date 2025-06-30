using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Verify.Marshaling.Utilities;

internal class MarshalRecord
{
    public string FieldName = string.Empty;
    public int Size = 0;
    public int Offset = 0;
    public int Count = 0;
    public IEnumerable<MarshalRecord> Nested = [];

    public static MarshalRecord From(Type t)
    {
        var fields = t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            .Select(f => From(f, t));

        return new MarshalRecord()
        {
            FieldName = t.Name,
            Size = Marshal.SizeOf(t),
            Nested = fields
        };
    }

    public static MarshalRecord From(FieldInfo f, Type parentType)
    {
        var a = f.GetCustomAttribute<MarshalAsAttribute>();
        var offset = (int)Marshal.OffsetOf(parentType, f.Name);

        // Nested
        if ((f.FieldType.IsClass || a?.Value == UnmanagedType.Struct) && !f.FieldType.IsArray && f.FieldType != typeof(String))
        {
            var m = MarshalRecord.From(f.FieldType);
            m.Offset = offset;
            return m;
        }

        if (f.FieldType.IsArray && f.FieldType.HasElementType)
        {
            var m = MarshalRecord.From(f.FieldType.GetElementType()!);
            m.Size = GetSize(f);
            m.Count = a?.SizeConst ?? 1;
            m.Offset = offset;
            return m;
        }

        return new MarshalRecord()
        {
            FieldName = f.Name,
            Size = GetSize(f),
            Offset = offset
        };
    }

    internal static int GetSize(FieldInfo f)
    {
        var a = f.GetCustomAttribute<MarshalAsAttribute>();
        
        if (a == null)
            return Marshal.SizeOf(f.FieldType);

        if (f.FieldType.IsArray && f.FieldType.HasElementType)
        {
            var elementType = f.FieldType.GetElementType();
            if (elementType == null)
                return 0;

            if (!elementType.IsPrimitive)
                return MarshalRecord.From(elementType).Size * a.SizeConst;
            else
                return Marshal.SizeOf(elementType!) * a.SizeConst;
        }
        return a.GetSize() ?? 0;
    }
}

