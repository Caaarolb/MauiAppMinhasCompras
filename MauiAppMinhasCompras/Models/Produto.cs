﻿using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
                _descricao = value;
            }
        }

        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total => Quantidade * Preco;
    }
}
