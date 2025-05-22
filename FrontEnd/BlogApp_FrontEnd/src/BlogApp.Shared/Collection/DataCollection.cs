namespace BlogApp.Shared.Collection;

public class DataCollection<T>
{
    public static DataCollection<T> CreateInstance(int page = 1, int take = 50)
    {
        if (page < 1)
            throw new ArgumentException("Page must be greater than or equal to 1.", nameof(page));
        if (take < 1)
            throw new ArgumentException("Take must be greater than or equal to 1.", nameof(take));

        return new DataCollection<T>()
        {
            Page = page,
            Take = take
        };
    }

    public int Page { get; set; } = 1;
    public int Take { get; set; } = 50;
    public int Pages { get; set; }
    public int Total { get; set; }

    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();

    public bool HasItems => Items.Any();
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < Pages;

}