using Microsoft.AspNetCore.Mvc;
using MoneyMind.DAO;
using MoneyMind.Models;

namespace MoneyMind.Controllers
{
    public class AtivoController : PadraoController<AtivoViewModel>
    {
        public AtivoController()
        {
            DAO = new AtivoDAO();
        }
    }
}
