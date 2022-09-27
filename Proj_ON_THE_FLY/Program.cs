using System;
using System.Data.SqlClient;

namespace Proj_ON_THE_FLY {
    internal class Program {
        static void Main(string[] args) {
            

            Passageiro passageiro = new Passageiro();
            Conexao cnx = new Conexao();
            SqlConnection conexaosql = new SqlConnection(cnx.Caminho());
            //passageiro.CadastrarPassageiro(conexaosql);
             passageiro.DeletarPassageiro(conexaosql);
             //passageiro.LocalizarPassageiro(conexaosql);
            // passageiro.ConsultarPassageiro(conexaosql);
            //passageiro.EditarPassageiro(conexaosql);
        }
    }
}
