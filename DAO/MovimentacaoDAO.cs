using MoneyMind.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MoneyMind.DAO
{
    public class MovimentacaoDAO : PadraoDAO<MovimentacaoViewModel>
    {
        protected override SqlParameter[] CriaParametros(MovimentacaoViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("id", model.Id),
                new SqlParameter("id_operacao", model.Id_Operacao),
                new SqlParameter("preco", model.Preco),
                new SqlParameter("id_carteira", model.Id_carteira),
                new SqlParameter("quantidade", model.Quantidade),
                new SqlParameter("data_hora_movimentacao", model.DataMovimentacao),
            };        
            return parameters;
        }

        protected override MovimentacaoViewModel MontaModel(DataRow registro)
        {
            MovimentacaoViewModel ativo = new MovimentacaoViewModel();
            ativo.Id = Convert.ToInt32(registro["id_movimentacao"]);
            ativo.Id_Operacao = Convert.ToInt32(registro["id_operacao"]);
            ativo.Id_Ativo = Convert.ToInt32(registro["id_ativo"]);
            ativo.Preco = Convert.ToDouble(registro["preco"]);
            ativo.Id_carteira = Convert.ToInt32(registro["id_carteira"]);
            ativo.Quantidade = Convert.ToInt32(registro["quantidade"]);
            ativo.DataMovimentacao = Convert.ToDateTime(registro["data_hora_movimentacao"]);

            return ativo;
        }

        public List<MovimentacaoViewModel> ConsultaMovimentacoesPorCarteira(int id_carteira)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_carteira", id_carteira),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_MovimentacaoPorCarteira", p);
            List<MovimentacaoViewModel> lista = new List<MovimentacaoViewModel>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }

        protected override void SetTabela()
        {
            Tabela = "Ativo";
        }
    }
}
