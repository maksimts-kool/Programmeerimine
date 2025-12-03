using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace Tund10.Avalonia;

public partial class SearchResultsWindow : Window
{
    public MainWindow MainRef;

    public SearchResultsWindow(MainWindow parent)
    {
        InitializeComponent();
        MainRef = parent;
    }

    public void SetResults(List<string> items)
    {
        ResultsList.ItemsSource = items;
    }

    private void ResultsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ResultsList.SelectedItem == null)
            return;

        string selected = ResultsList.SelectedItem.ToString()!;
        Close();
        MainRef.HandleSearchSelection(selected);
    }

}