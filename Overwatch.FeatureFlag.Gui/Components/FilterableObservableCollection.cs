using System.Collections.ObjectModel;

namespace Overwatch.FeatureFlag.Gui.Components;

public class FilterableObservableCollection<T> : ObservableCollection<T>
{
    private readonly List<T> originalCollection = [];
    private readonly Func<Task<IEnumerable<T>>> _loadDataAsync;

    /// <summary>
    /// Initializes the collection with an async data loading function.
    /// </summary>
    /// <param name="loadDataAsync">A function that loads data asynchronously.</param>
    public FilterableObservableCollection(Func<Task<IEnumerable<T>>> loadDataAsync)
    {
        _loadDataAsync = loadDataAsync ?? throw new ArgumentNullException(nameof(loadDataAsync));
    }

    /// <summary>
    /// Loads and resets the collection using the provided async data loader.
    /// </summary>
    public async Task LoadAsync()
    {
        var items = await _loadDataAsync();
        Reset(items);
    }

    /// <summary>
    /// Filters the collection based on the provided predicate.
    /// </summary>
    public void Filter(Func<T, bool> filterPredicate)
    {
        var filteredItems = originalCollection.Where(filterPredicate).ToList();
        ReplaceAll(filteredItems);
    }

    /// <summary>
    /// Sorts the collection based on the provided key selector.
    /// </summary>
    public void Sort<TKey>(Func<T, TKey> keySelector, bool ascending = true)
    {
        var sortedItems = ascending
            ? originalCollection.OrderBy(keySelector).ToList()
            : originalCollection.OrderByDescending(keySelector).ToList();

        ReplaceAll(sortedItems);
    }

    /// <summary>
    /// Resets the collection with new items, clearing any filters or sorts.
    /// </summary>
    public void Reset(IEnumerable<T> items)
    {
        originalCollection.Clear();
        originalCollection.AddRange(items);
        ReplaceAll(originalCollection);
    }

    /// <summary>
    /// Replaces all items in the collection.
    /// </summary>
    private void ReplaceAll(IEnumerable<T> items)
    {
        Clear();
        foreach (var item in items)
        {
            Add(item);
        }
    }
}