namespace LambdaCalculus.lambda;

public class Definition : Expression
{
    private static uint _serial = 0;
    private static uint Serial => _serial++;
    public string Name { get; }
    public int Calls { get; internal set; }
    internal uint Id { get; }
    internal uint? PreId { get; }

    public Definition(string name, int calls, uint? preId = null)
    {
        PreId = preId;
        Id = Serial;
        Name = name;
        Calls = calls;
    }

    private static uint _charLength = 'z' - 'a' + 1;

    private string FindSimplestName(HashSet<string> taken)
    {
        uint value = 1;
        while (true)
        {
            uint copy = value;
            string result = "";
            while (copy != 0)
            {
                char c = (char) ('a' + (int)(copy % _charLength));
                result += c;
                copy /= _charLength;
            }

            if (!taken.Contains(result))
                return result;
            value++;
        }
    }

    internal override string ToString(Dictionary<uint, string> cache, HashSet<string> taken)
    {
        var isCached = cache.TryGetValue(Id, out var name);
        if (isCached)
            return name!;
        if (!taken.Contains(Name))
        {
            cache.Add(Id, Name);
            taken.Add(Name);
            return Name;
        }
        if (Parent?.Parent?.GetLocalVariableByName(Name) is null)
        {
            cache.Add(Id, Name);
            return Name;
        }
        var newName = FindSimplestName(taken);
        cache.Add(Id, newName);
        taken.Add(newName);
        return newName;
    }

    public override string GetHashCode()
    {
        return GetContextSize().ToString();
    }

    public override bool IsWellFormatted()
    {
        if (Parent is not Lambda lambda)
            throw new Exception("Not well formatted");
        if(lambda.Definition != this)
            throw new Exception("Not well formatted");
        return true;
    }

    public override Expression EtaReduction()
    {
        return this;
    }
}