using System.Collections.Generic;

namespace MoneyMind.Models
{
    public class CarteiraCompletaViewModel
    {
        public CarteiraConsultaViewModel Carteira { get; set; }
        public List<AtiovoCarteiraConsultaViewModel> Ativos { get; set; }
    }
}
