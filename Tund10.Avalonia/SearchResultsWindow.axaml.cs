using Avalonia.Controls;

namespace Tund10.Avalonia;

public partial class SearchResultsWindow : Window
{
    public MainWindow MainRef;

    public SearchResultsWindow()
    {
        InitializeComponent();
        MainRef = null!;
    }

    public SearchResultsWindow(MainWindow parent)
    {
        InitializeComponent();
        MainRef = parent;
    }

    public void SetResults(object items) => ResultsList.ItemsSource = (System.Collections.IEnumerable)items;

    private void ResultsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            Close();
            MainRef.HandleSearchSelection(e.AddedItems[0]!.ToString()!);
        }
    }
}