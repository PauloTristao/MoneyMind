using MoneyMind.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MoneyMind.DAO
{
    public class CarteiraDAO : PadraoDAO<CarteiraViewModel>
    {
        protected override SqlParameter[] CriaParametros(CarteiraViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("id", model.Id),
                new SqlParameter("descricao", model.Descricao),
                new SqlParameter("id_portifolio", model.Id_Portifolio)
            };

            return parameters;
        }

        protected override CarteiraViewModel MontaModel(DataRow registro)
        {
            CarteiraViewModel portifolio = new CarteiraViewModel();
            portifolio.Id = Convert.ToInt32(registro["id_carteira"]);
            portifolio.Descricao = registro["descricao"].ToString();
            portifolio.Id_Portifolio = Convert.ToInt32(registro["id_portifolio"]);
            return portifolio;
        }

        public List<CarteiraViewModel> ConsultaCarteirasPorPortifolio(int id_portifolio)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_portifolio", id_portifolio),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_CarteiraPorPortifolio", p);
            List<CarteiraViewModel> lista = new List<CarteiraViewModel>();

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }

        protected override void SetTabela()
        {
            Tabela = "Carteira";
        }
    }
}
