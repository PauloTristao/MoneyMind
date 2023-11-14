using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyMind.DAO;
using MoneyMind.Models;
using System;
using System.Collections.Generic;

namespace MoneyMind.Controllers
{
    public class MovimentacaoController : PadraoController<MovimentacaoViewModel>
    {
        public MovimentacaoController()
        {
            DAO = new MovimentacaoDAO();
        }

        public IActionResult CadastroMovimentacao(int idCarteira)
        {
            try
            {
                MovimentacaoViewModel model = new MovimentacaoViewModel();
                PreparaComboAtivos();
                PreparaComboOperacao();
                ViewBag.Ativo.Insert(0, new SelectListItem("Selecione...", "0"));
                ViewBag.Operacao.Insert(0, new SelectListItem("Selecione...", "0"));
                ViewBag.Carteira = idCarteira;
                return View("Form", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public override IActionResult Save(MovimentacaoViewModel model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                        DAO.Insert(model);
                    else
                        DAO.Update(model);
                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void PreparaComboAtivos()
        {
            AtivoDAO dao = new AtivoDAO();
            var lista = dao.ListaAtivo();

            List<SelectListItem> listaRetorno = new List<SelectListItem>();
            foreach (var ativo in lista)
            {
                listaRetorno.Add(new SelectListItem(ativo.Ticker, ativo.Id.ToString()));
            }

            ViewBag.Ativo = listaRetorno;
        }


        private void PreparaComboOperacao()
        {
            MovimentacaoDAO dao = new MovimentacaoDAO();
            var lista = dao.ListaOperacao();

            List<SelectListItem> listaRetorno = new List<SelectListItem>();
            foreach (var operacao in lista)
            {
                listaRetorno.Add(new SelectListItem(operacao.Descricao, operacao.Id.ToString()));
            }

            ViewBag.Operacao = listaRetorno;
        }

    }
}
