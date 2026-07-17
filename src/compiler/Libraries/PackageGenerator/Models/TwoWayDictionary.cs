namespace Arc.Compiler.PackageGenerator.Models;

public class TwoWayDictionary<TK1, TK2, TV> where TK1 : notnull where TK2 : notnull
{
    private Dictionary<TK1, TV> D1 { get; } = [];
    private Dictionary<TK2, TV> D2 { get; } = [];

    public void Add(TK1 k1, TK2 k2, TV value)
    {
        D1.Add(k1, value);
        D2.Add(k2, value);
    }

    public TV Get(TK1 k) => D1[k];

    public TV Get(TK2 k) => D2[k];

    public TV Remove(TK1 k)
    {
        var item = D1[k];
        D1.Remove(k);
        D2.Remove(D2.FirstOrDefault(kv => kv.Value?.Equals(item) ?? false).Key);
        return item;
    }

    public TV Remove(TK2 k)
    {
        var item = D2[k];
        D2.Remove(k);
        D1.Remove(D1.FirstOrDefault(kv => kv.Value?.Equals(item) ?? false).Key);
        return item;
    }
    
    public bool ContainsKey(TK1 k) => D1.ContainsKey(k);
    public bool ContainsKey(TK2 k) => D2.ContainsKey(k);

    public TV? this[TK1 k1] => D1.GetValueOrDefault(k1);
    public TV? this[TK2 k2] => D2.GetValueOrDefault(k2);
    
    public int Count => D1.Count;
    public int Length => D1.Count;
    public bool IsEmpty => D1.Count == 0;
}