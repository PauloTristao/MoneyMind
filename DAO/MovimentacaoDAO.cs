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
                new SqlParameter("id_ativo", model.Id_Ativo),
                new SqlParameter("preco", model.Preco),
                new SqlParameter("id_carteira", model.Id_carteira),
                new SqlParameter("quantidade", model.Quantidade),
                new SqlParameter("data_hora_movimentacao", model.DataMovimentacao),
            };        
            return parameters;
        }

        protected override MovimentacaoViewModel MontaModel(DataRow registro)
        {
            MovimentacaoViewModel movimentacao = new MovimentacaoViewModel();
            movimentacao.Id = Convert.ToInt32(registro["id_movimentacao"]);
            movimentacao.Id_Operacao = Convert.ToInt32(registro["id_operacao"]);
            movimentacao.Id_Ativo = Convert.ToInt32(registro["id_ativo"]);
            movimentacao.Preco = Convert.ToDouble(registro["preco"]);
            movimentacao.Id_carteira = Convert.ToInt32(registro["id_carteira"]);
            movimentacao.Quantidade = Convert.ToInt32(registro["quantidade"]);
            movimentacao.DataMovimentacao = Convert.ToDateTime(registro["data_hora_movimentacao"]);

            if (registro.Table.Columns.Contains("DescricaoCarteira"))
                movimentacao.DescricaoCarteira = registro["DescricaoCarteira"].ToString();

            return movimentacao;
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

        public List<MovimentacaoViewModel> ConsultaAvancadaMovimentacao(int carteira,
                                                         DateTime dataInicial,
                                                         DateTime dataFinal)
        {
            SqlParameter[] p = {
                new SqlParameter("carteira", carteira),
                new SqlParameter("dataInicial", dataInicial),
                new SqlParameter("dataFinal", dataFinal),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaMovimentacoes", p);
            var lista = new List<MovimentacaoViewModel>();
            foreach (DataRow dr in tabela.Rows)
                lista.Add(MontaModel(dr));

            return lista;
        }

        public List<ItemTabelaGeralViewModel> ListaOperacao()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_tabela_geral", 1),
            };
            DataTable tabela = HelperDAO.ExecutaProcSelect("spListagemOperacao", p);
            List<ItemTabelaGeralViewModel> retorno = new List<ItemTabelaGeralViewModel>();

            ItemTabelaGeralViewModel itemTabelaGeral;
            foreach (DataRow registro in tabela.Rows)
            {
                itemTabelaGeral = new ItemTabelaGeralViewModel();
                itemTabelaGeral.Id = Convert.ToInt32(registro["id_item_tabela_geral"]);
                itemTabelaGeral.Id_tabela_geral = Convert.ToInt32(registro["id_tabela_geral"]);
                itemTabelaGeral.Descricao = registro["descricao"].ToString();

                retorno.Add(itemTabelaGeral);
            }

            return retorno;
        }

        protected override void SetTabela()
        {
            Tabela = "Movimentacao";
            ChaveIdentity = true;
        }
    }
}
