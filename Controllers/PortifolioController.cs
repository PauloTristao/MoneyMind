using Microsoft.AspNetCore.Mvc;
using MoneyMind.DAO;
using MoneyMind.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyMind.Controllers
{
    public class PortifolioController : PadraoController<PortifolioViewModel>
    {
        public override IActionResult Index()
        {
            try
            {
                PortifolioViewModel portifolio = new PortifolioViewModel();
                List<CarteiraViewModel> carteiras = new List<CarteiraViewModel>();
                PortifolioDAO port = new PortifolioDAO();
                CarteiraDAO cart = new CarteiraDAO();
                MovimentacaoDAO mov = new MovimentacaoDAO();

                UsuarioViewModel usuario = HttpContext.Session.GetObject<UsuarioViewModel>("Usuario");
                portifolio = port.ConsultaPortifolioPorUsuario(usuario.Id);
                carteiras = cart.ConsultaCarteirasPorPortifolio(portifolio.Id);

                PortifolioCompletoViewModel pc = new PortifolioCompletoViewModel();
                pc.Portifolio = new PortifolioConsultaViewModel()
                {
                    IdPortifolio = portifolio.Id,
                    NomePortifolio = portifolio.Nome,
                    IdUsuario = portifolio.IdUsuario
                };


                pc.Carteiras = new List<CarteiraPortifolioViewModel>();
                CarteiraPortifolioViewModel carteiraPortifolio;
                foreach (var carteira in carteiras)
                {
                    carteiraPortifolio = new CarteiraPortifolioViewModel();
                    carteiraPortifolio.CarteiraNome = carteira.Descricao;

                    List<MovimentacaoViewModel> movimentacao = new List<MovimentacaoViewModel>();
                    movimentacao = mov.ConsultaMovimentacoesPorCarteira(carteira.Id);

                    carteiraPortifolio.Quantidade = movimentacao.Sum(x => x.Quantidade);
                    carteiraPortifolio.Total = movimentacao.Sum(x => x.Preco) * carteiraPortifolio.Quantidade;

                    pc.Carteiras.Add(carteiraPortifolio);
                }
                pc.Portifolio.Quantidade = pc.Carteiras.Sum(x => x.Quantidade);
                pc.Portifolio.Total = pc.Carteiras.Sum(x => x.Total) * pc.Portifolio.Quantidade;

                return View("Index", pc);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

       
    }
}
