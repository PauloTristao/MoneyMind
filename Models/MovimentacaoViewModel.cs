using System;

namespace MoneyMind.Models
{
    public class MovimentacaoViewModel : PadraoViewModel
    {
        public int Id_carteira { get; set; }
        public int Id_Ativo { get; set; }
        public int Id_Operacao { get; set; }
        public int Quantidade { get; set; }
        public double Preco { get; set;}
        public DateTime DataMovimentacao { get; set;}
        public string DescricaoCarteira { get; set;}
        public string Ticker { get; set;}
    }
}
