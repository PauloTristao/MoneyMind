using Microsoft.AspNetCore.Mvc;
using MoneyMind.DAO;
using MoneyMind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace MoneyMind.Controllers
{
    public class CarteiraController : PadraoController<CarteiraViewModel>
    {
        public CarteiraController()
        {
            DAO = new CarteiraDAO();
        }

        public override IActionResult Index()
        {
            try
            {
                return RedirectToAction("CarregaCarteira");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult CarregaCarteira(int id)
        {
            try
            {
                CarteiraCompletaViewModel carteiraRetorno = new CarteiraCompletaViewModel();
                if (id > 0)
                {
                    var model = DAO.Consulta(id);
                    var ativoDao = new AtivoDAO();

                    MovimentacaoDAO movimentacaoDAO = new MovimentacaoDAO();
                    List<MovimentacaoViewModel> movimentacoes = movimentacaoDAO.ConsultaMovimentacoesPorCarteira(model.Id);

                    carteiraRetorno.Carteira = new CarteiraConsultaViewModel
                    {
                        IdCarteira = id,
                        IdPortifolio = model.Id_Portifolio,
                        NomeCarteira = model.Descricao
                    };

                    carteiraRetorno.Ativos = new List<AtiovoCarteiraConsultaViewModel>();
                    AtiovoCarteiraConsultaViewModel at;

                    foreach (var movimentacao in movimentacoes)
                    {
                        at = new AtiovoCarteiraConsultaViewModel();
                        if (!carteiraRetorno.Ativos.Any(x => x.IdAtivo == movimentacao.Id_Ativo))
                        {
                            var ativo = ativoDao.Consulta(movimentacao.Id_Ativo);
                            at.Ticker = ativo.Ticker;
                            at.IdAtivo = movimentacao.Id_Ativo;
                            if (movimentacao.Id_Operacao == 1)
                            {
                                at.Quantidade = movimentacao.Quantidade;
                                at.PrecoMedio = movimentacao.Preco;
                                at.Total = at.Quantidade * at.PrecoMedio;
                            }
                            carteiraRetorno.Ativos.Add(at);
                        }
                        else
                        {
                            at = carteiraRetorno.Ativos.Where(x => x.IdAtivo == movimentacao.Id_Ativo).FirstOrDefault();
                            at.IdAtivo = movimentacao.Id_Ativo;
                            if (movimentacao.Id_Operacao == 1)
                            {
                                at.Quantidade += movimentacao.Quantidade;
                                at.Total += movimentacao.Quantidade * movimentacao.Preco;
                                at.PrecoMedio = at.Total / at.Quantidade;
                            }
                            else
                            {
                                at.Quantidade -= movimentacao.Quantidade;
                                at.Total -= movimentacao.Quantidade * movimentacao.Preco;
                                at.PrecoMedio = at.Total / at.Quantidade;
                            }
                        }
                        
                    }

                    carteiraRetorno.Carteira.Quantidade = carteiraRetorno.Ativos.Sum(x => x.Quantidade);
                    carteiraRetorno.Carteira.Total = carteiraRetorno.Ativos.Sum(x => x.Total);
                    return View("Index", carteiraRetorno);
                }
                else
                    return View("Index", carteiraRetorno);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

    }
}
