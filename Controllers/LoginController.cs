using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyMind.DAO;
using MoneyMind.Models;
using Newtonsoft.Json;
using System;

namespace MoneyMind.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        public IActionResult Login(LoginViewModel model)
        {
            ValidaDados(model);
            if (ModelState.IsValid == false)
            {
                return View("Index", model);
            }
            else
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                UsuarioViewModel user = usuarioDAO.ConsultaAcesso(model.Login, model.Senha);

                if (user != null)
                {
                    var json = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("Usuario", json);
                    HttpContext.Session.SetString("UsuarioName", json);
                    HttpContext.Session.SetString("UsuarioFoto", json);
                    HttpContext.Session.SetString("Logado", "true");
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Senha", "Usuário ou senha inválidos!");
                    return View("Index", model);
                }
            }
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        protected void ValidaDados(LoginViewModel model)
        {
            ModelState.Clear();
            if (model.Login == null)
                ModelState.AddModelError("Login", "Preencha esse campo");
            if (model.Senha == null)
                ModelState.AddModelError("Senha", "Preencha esse campo");

        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                UsuarioViewModel model = Activator.CreateInstance<UsuarioViewModel>();
                return View("Form", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Save(UsuarioViewModel model, string Operacao)
        {
            try
            {
                UsuarioDAO usuarioDao = new UsuarioDAO();
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    //PreencheDadosParaView(Operacao, model);
                    return View("Form", model);
                }
                else
                {
                    model.Adm = false;
                    if (Operacao == "I")
                        usuarioDao.Insert(model);
                    else
                        usuarioDao.Update(model);
                    return RedirectToAction("Index","Login");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected void ValidaDados(UsuarioViewModel model, string operacao)
        {
            ModelState.Clear();
            if (model.NomePessoa == null)
                ModelState.AddModelError("NomePessoa", "Preencha esse campo");
            if (model.LoginUsuario == null)
                ModelState.AddModelError("LoginUsuario", "Preencha esse campo");
            if (model.Senha == null)
                ModelState.AddModelError("Senha", "Preencha esse campo");

        }
    }
}
