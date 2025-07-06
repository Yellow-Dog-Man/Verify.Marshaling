using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Verify.Marshaling.Utilities;

internal class MarshalRecord
{
    public string FieldName = string.Empty;
    public int Size = 0;
    public int Offset = 0;
    public int Count = 0;
    public IEnumerable<MarshalRecord> Nested = [];
    public int Pack = 0;

    public string Type = string.Empty;

    public static MarshalRecord From(Type t)
    {
        // Thanks to: https://www.soinside.com/question/hZiTTQsLgN5fbX7JyMunTe
        // Although, this most of the time is never null.
        if (t.StructLayoutAttribute == null)
            throw new InvalidOperationException($"Cannot verify layout of {t}, it does not have a [StructLayout] Attribute.");

        if (t.StructLayoutAttribute.Value == LayoutKind.Auto)
            throw new InvalidOperationException($"Cannot verify layout of {t}, it is using automatic layouts.");

        var fields = t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            .Select(f => From(f, t));

        return new MarshalRecord()
        {
            FieldName = t.Name,
            Size = Marshal.SizeOf(t),
            Nested = fields,
            Type = "struct",
            Pack = t.StructLayoutAttribute.Pack
        };
    }

    internal static bool ShouldNest(FieldInfo f)
    {
        var t = f.FieldType;

        if (t.IsArray)
            return false;

        if (t.IsPrimitive)
            return false;

        if (t.IsEnum)
            return false;

        // Special case, becaue it looks like a class because it is, but needs to be treated as a value.
        if (t == typeof(String))
            return false;

        if (t.IsTypeDefinition)
            return true;

        if (t.IsValueType)
            return false;

        if (t.IsLayoutSequential || t.IsExplicitLayout)
            return true;

        if (t.IsClass)
            return true;

        return false;
    }

    public static MarshalRecord From(FieldInfo f, Type parentType)
    {
        var a = f.GetCustomAttribute<MarshalAsAttribute>();
        var offset = (int)Marshal.OffsetOf(parentType, f.Name);

        // Nested
        if (ShouldNest(f))
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
            m.Type = "array";
            return m;
        }

        return new MarshalRecord()
        {
            FieldName = f.Name,
            Size = GetSize(f),
            Offset = offset,
            Type = f.FieldType.Name,
        };
    }

    internal static int GetSize(FieldInfo f)
    {
        var a = f.GetCustomAttribute<MarshalAsAttribute>();

        if (a == null)
        {
            if (f.FieldType == typeof(String))
                throw new InvalidOperationException("Strings must be tagged with [MarshalAs] in order to set their length");

            return Marshal.SizeOf(f.FieldType);
        }

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

