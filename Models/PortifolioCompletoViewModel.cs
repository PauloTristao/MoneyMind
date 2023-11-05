using System.Collections.Generic;

namespace MoneyMind.Models
{
    public class PortifolioCompletoViewModel
    {
        public PortifolioConsultaViewModel Portifolio { get; set; }
        public List<CarteiraPortifolioViewModel> Carteiras { get; set; }
    }
}
