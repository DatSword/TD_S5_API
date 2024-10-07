using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TD1BlazorApp.Models;
using TD1BlazorApp.Services;

namespace TD1BlazorApp.ViewModels;
internal sealed partial class ProduitViewModel : ObservableObject
{
    private readonly IService<Produit> _service;

    [ObservableProperty] private ObservableCollection<Produit> _produits = new();

    [ObservableProperty] private bool _isLoading;

    public ProduitViewModel(IService<Produit> service)
    {
        _service = service;
        LoadProduitsCommand = new AsyncRelayCommand(GetDataOnLoadAsync);
    }

    public IAsyncRelayCommand LoadProduitsCommand { get; }

    public async Task GetDataOnLoadAsync()
    {
        IsLoading = true;
        try
        {
            var produitsList = await _service.GetItemsAsync();
            if (produitsList != null)
            {
                Produits = new ObservableCollection<Produit>(produitsList);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
