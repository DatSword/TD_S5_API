using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TD1BlazorApp.Models;
using TD1BlazorApp.Services;

namespace TD1BlazorApp.ViewModels;
internal sealed partial class MarqueViewModel : ObservableObject
{
    private readonly IService<Marque> _service;

    [ObservableProperty] private ObservableCollection<Marque> _marques = new();

    [ObservableProperty] private bool _isLoading;

    public MarqueViewModel(IService<Marque> service)
    {
        _service = service;
        LoadMarquesCommand = new AsyncRelayCommand(GetDataOnLoadAsync);
    }

    public IAsyncRelayCommand LoadMarquesCommand { get; }

    public async Task GetDataOnLoadAsync()
    {
        IsLoading = true;
        try
        {
            var marquesList = await _service.GetItemsAsync();
            if (marquesList != null)
            {
                Marques = new ObservableCollection<Marque>(marquesList);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
