using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var app = (App)Application.Current; // ? Obtendo a instância da aplicação corretamente

        if (app?.Db != null)
        {
            List<Produto> tmp = await app.Db.GetAll();
            lista.Clear();
            tmp.ForEach(i => lista.Add(i));
        }
        else
        {
            await DisplayAlert("Erro", "Banco de dados não foi inicializado.", "Ok");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void text_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        var app = (App)Application.Current;

        if (app?.Db != null)
        {
            string q = e.NewTextValue;
            lista.Clear();
            List<Produto> tmp = await app.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
    }

    // ? Método para remover um produto da lista e do banco de dados
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var produto = (Produto)menuItem.BindingContext;

        bool confirm = await DisplayAlert("Remover", $"Deseja remover {produto.Descricao}?", "Sim", "Não");
        if (confirm)
        {
            var app = (App)Application.Current;

            if (app?.Db != null)
            {
                await app.Db.Delete(produto.Id);
                lista.Remove(produto);
            }
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var app = (App)Application.Current;

        if (app?.Db != null)
        {
            string q = e.NewTextValue;
            lista.Clear();
            List<Produto> tmp = await app.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
    }

}

