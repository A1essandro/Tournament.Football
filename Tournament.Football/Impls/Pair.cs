using Tournament.Contracts;

namespace Tournament.Football;

public class Pair<T> : IPair<T>
{

    public static Pair<T> Create(T item1, T item2) => new Pair<T>
    {
        Item1 = item1,
        Item2 = item2
    };

    public T Item1 { get; private set; }

    public T Item2 { get; private set; }
}
