using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;

namespace Tund10.Avalonia;

public static class SearchHelper
{
    public static List<string> GetSearchResults(string query, DataGrid ownersGrid,
        DataGrid carsGrid, DataGrid servicesGrid, DataGrid logsGrid)
    {
        var results = new List<string>();

        // OWNERS
        foreach (dynamic o in ownersGrid.ItemsSource!)
        {
            if (((string)o.FullName).ToLower().Contains(query) ||
                ((string)o.Phone).ToLower().Contains(query))
            {
                results.Add($"{LanguageManager.Get("Owners").ToUpper()} • {o.FullName} • {o.Phone}");
            }
        }

        // CARS
        if (results.Count < 3)
        {
            foreach (dynamic c in carsGrid.ItemsSource!)
            {
                if (((string)c.Brand).ToLower().Contains(query) ||
                    ((string)c.Model).ToLower().Contains(query) ||
                    ((string)c.RegistrationNumber).ToLower().Contains(query) ||
                    ((string)c.OwnerName).ToLower().Contains(query))
                {
                    results.Add($"{LanguageManager.Get("Cars").ToUpper()} • {c.Brand} {c.Model} ({c.RegistrationNumber})");
                }
            }
        }

        // SERVICES
        if (results.Count < 3)
        {
            foreach (dynamic s in servicesGrid.ItemsSource!)
            {
                if (((string)s.Name).ToLower().Contains(query))
                {
                    results.Add($"{LanguageManager.Get("ServicesLogs").ToUpper()} • {s.Name}");
                }
            }
        }

        // LOGS
        if (results.Count < 3 && logsGrid.ItemsSource is IEnumerable<object> logs)
        {
            foreach (dynamic log in logs)
            {
                if (((string)log.Car).ToLower().Contains(query) ||
                    ((string)log.Service).ToLower().Contains(query) ||
                    log.Mileage.ToString().Contains(query))
                {
                    results.Add($"{LanguageManager.Get("ServicesLogs").ToUpper()} • {log.Car} → {log.Service} → {log.DateOfService:yyyy-MM-dd}");
                    if (results.Count == 3) break;
                }
            }
        }

        return results;
    }
}