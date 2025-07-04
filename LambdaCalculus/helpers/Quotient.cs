namespace LambdaCalculus.helpers;

public class QuotientClass<T> where T : QuotientClass<T>
{
    public T? Parent { get; set; }
}

public class QuotientClass : QuotientClass<QuotientClass> { }

public class Quotient<TElement, TClass> 
    where TElement : notnull
    where TClass : QuotientClass<TClass>, new()
{
    private readonly Dictionary<TElement, TClass>? _nodeMap;
    public int ClassCount { get; private set; }

    public Quotient()
    {
        _nodeMap = new Dictionary<TElement, TClass>();
        ClassCount = 0;
    }
    
    public Quotient(params TElement[] nodes) : this()
    {
        foreach (var node in nodes)
            if(GetClass(node) == null)
                SetClass(node);
    }

    private void SetClass(TElement node, TClass color)
    {
        var previous = GetClass(node);
        if (previous == null || previous == color)
        {
            _nodeMap![node] = color;
            return;
        }

        previous.Parent = color;
        ClassCount--;
        _nodeMap!.Remove(node);
        _nodeMap.Add(node, color);
    }

    public void SetClass(TElement node)
    {
        var color = GetClass(node);
        if (color != null)
            return;
        color = new TClass();
        ClassCount++;
        SetClass(node, color);
    }

    public void SetEqual(TElement node1, TElement node2)
    {
        TClass? color = GetClass(node1) ?? GetClass(node2);
        if (color == null)
        {
            SetClass(node1);
            color = GetClass(node1);
        }
        SetClass(node2, color!);
    }

    public bool HasClass(TElement node)
    {
        return _nodeMap!.TryGetValue(node, out TClass? _);
    }

    private TClass? GetClass(TElement node)
    {
        if (!_nodeMap!.TryGetValue(node, out TClass? color))
            return null;

        return GetClass(color, node);
    }

    private TClass GetClass(TClass color, TElement? node)
    {
        var parent = color.Parent;
        if (parent == null)
            return color;
        
        var colorList = new List<TClass>{ color, parent };
        TClass current = parent;
        TClass? next;
        while ((next = current.Parent) != null)
        {
            colorList.Add(next);
            current = next;
        }

        for (int i = 0; i < colorList.Count - 1; i++)
            colorList[i].Parent = current;
        

        if (node != null)
        {
            _nodeMap!.Remove(node);
            _nodeMap.Add(node, current);
        }

        return current;
    }

    public bool AreEqual(TElement node1, TElement node2)
    {
        var color1 = GetClass(node1);
        var color2 = GetClass(node2);
        if (color1 == null && color2 == null)
            return node1.Equals(node2);
        if (color1 == null || color2 == null)
            return false;
        return color1 == color2;
    }

    public List<List<TElement>> GetAllClasses()
    {
        var nodes = _nodeMap!.Keys.ToList();
        var result = new List<List<TElement>>();
        foreach (var node in nodes)
        {
            var matching = result.Where(list => AreEqual(node, list[0])).ToArray();
            if (!matching.Any())
                result.Add(new List<TElement>{ node });
            else
                matching[0].Add(node);
        }
        return result;
    }
}

public class Quotient<TElement> : Quotient<TElement, QuotientClass> 
    where TElement : notnull
{
    public Quotient() : base() { }
    public Quotient(params TElement[] nodes) : base(nodes) { }
}