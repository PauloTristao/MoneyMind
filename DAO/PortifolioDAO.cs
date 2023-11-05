using MoneyMind.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MoneyMind.DAO
{
    public class PortifolioDAO : PadraoDAO<PortifolioViewModel>
    {
        protected override SqlParameter[] CriaParametros(PortifolioViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("id", model.Id);
            parameters[1] = new SqlParameter("id_usuario", model.Id_Usuario);
            parameters[2] = new SqlParameter("nome", model.Nome);

            return parameters;
        }

        protected override PortifolioViewModel MontaModel(DataRow registro)
        {
            PortifolioViewModel portifolio = new PortifolioViewModel();
            portifolio.Id = Convert.ToInt32(registro["id_portifolio"]);
            portifolio.Id_Usuario = Convert.ToInt32(registro["id_usuario"]);
            portifolio.Nome = registro["nome"].ToString();
            return portifolio;
        }

        public PortifolioViewModel ConsultaPortifolioPorUsuario(int id_usuario)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_usuario", id_usuario),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_PortifolioPorUsuario", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        protected override void SetTabela()
        {
            Tabela = "Portifolio";
        }
    }
}
