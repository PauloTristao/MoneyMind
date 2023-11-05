using System.Data.SqlClient;

namespace MoneyMind.DAO
{
    public class ConexaoBD
    {
        /// <summary>
        /// Método Estático que retorna um conexao aberta com o BD
        /// </summary>
        /// <returns>Conexão aberta</returns>
        public static SqlConnection GetConexao()
        {
            string strCon = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=MoneyMind;Trusted_Connection=True;";    
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
 