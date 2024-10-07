using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TD1BlazorApp.Models;
using TD1BlazorApp.Services;

namespace TD1BlazorApp.ViewModels;
internal sealed partial class TypeProduitViewModel : ObservableObject
{
    private readonly IService<TypeProduit> _service;

    [ObservableProperty] private ObservableCollection<TypeProduit> _typeProduits = new();

    [ObservableProperty] private bool _isLoading;

    public TypeProduitViewModel(IService<TypeProduit> service)
    {
        _service = service;
        LoadTypeProduitsCommand = new AsyncRelayCommand(GetDataOnLoadAsync);
    }

    public IAsyncRelayCommand LoadTypeProduitsCommand { get; }

    public async Task GetDataOnLoadAsync()
    {
        IsLoading = true;
        try
        {
            var typeProduitsList = await _service.GetItemsAsync();
            if (typeProduitsList != null)
            {
                TypeProduits = new ObservableCollection<TypeProduit>(typeProduitsList);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
