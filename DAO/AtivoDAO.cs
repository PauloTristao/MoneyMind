using MoneyMind.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MoneyMind.DAO
{
    public class AtivoDAO : PadraoDAO<AtivoViewModel>
    {
        protected override SqlParameter[] CriaParametros(AtivoViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("ticker", model.Ticker),
                new SqlParameter("empresa", model.Empresa),
                new SqlParameter("id_classe_ativo", model.Id_classe_ativo)
            };

            return parameters;
        }

        protected override AtivoViewModel MontaModel(DataRow registro)
        {
            AtivoViewModel ativo = new AtivoViewModel();
            ativo.Id = Convert.ToInt32(registro["id_Ativo"]);
            ativo.Ticker = registro["ticker"].ToString();
            ativo.Empresa = registro["Empresa"].ToString();
            ativo.Id_classe_ativo = Convert.ToInt32(registro["id_classe_ativo"]);
            return ativo;
        }

        public List<AtivoViewModel> ConsultaMovimentacoesPorCarteira(int id_carteira)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_carteira", id_carteira),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_AticoPorCarteira", p);
            List<AtivoViewModel> lista = new List<AtivoViewModel>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }

        public List<AtivoViewModel> ListaAtivo()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("ativo", "todos"),
            };
            DataTable tabela = HelperDAO.ExecutaProcSelect("spListagemAtivo", p);
            List<AtivoViewModel> retorno = new List<AtivoViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }

        protected override void SetTabela()
        {
            Tabela = "Ativo";
            ChaveIdentity = true;
        }
    }
}
