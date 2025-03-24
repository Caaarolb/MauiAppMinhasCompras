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
        try
        {
            var app = (App)Application.Current;
            if (app?.Db != null)
            {
                lista.Clear();
                List<Produto> tmp = await app.Db.GetAll();
                tmp.ForEach(i => lista.Add(i));
            }
            else
            {
                await DisplayAlert("Erro", "Banco de dados não foi inicializado.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
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
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            if (p != null)
            {
                Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}