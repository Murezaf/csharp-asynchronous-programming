namespace LifeBeforeAsync;

public abstract class Food
{
    readonly TimeSpan _cookTime;
    public string Name { get; }

    protected Food(TimeSpan cookTime)
    {
        _cookTime = cookTime;
        Name = GetType().Name;
    }

    public async Task Cook()
    {
        Console.WriteLine($"Cooking {Name}...");
        await Task.Delay(_cookTime);
        //Task.Delay(_cookTime).Wait();
        Console.WriteLine($"{Name} Complete");
    }
}

//public class Turkey : Food
//{
//    public Turkey() : base(TimeSpan.FromMicroseconds(5))
//    {
//    }
//}
public class Turkey() : Food(TimeSpan.FromSeconds(5));
public class MashedPotatoes() : Food(TimeSpan.FromSeconds(2));
public class Gravy() : Food(TimeSpan.FromSeconds(1));
public class Stuffing() : Food(TimeSpan.FromSeconds(2));
