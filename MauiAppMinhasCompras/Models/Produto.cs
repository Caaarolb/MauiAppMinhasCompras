using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [PrimaryKey, AutoIncrement]
        public string Descricao { get; set; }

        public double Quantidade { get; set; }

        public double Preco { get; set; }

    }
}
