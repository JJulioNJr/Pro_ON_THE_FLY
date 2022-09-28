using System;
using System.Data.SqlClient;

namespace Proj_ON_THE_FLY {
    internal class Program {
        static void Main(string[] args) {
            

            Passageiro passageiro = new Passageiro();
            Aeronave aeronave = new Aeronave();
            Conexao cnx = new Conexao();
            SqlConnection conexaosql = new SqlConnection(cnx.Caminho());

            #region Passageiro
            // passageiro.CadastrarPassageiro(conexaosql);
            // passageiro.DeletarPassageiro(conexaosql);
            // passageiro.LocalizarPassageiro(conexaosql);
            //  passageiro.ConsultarPassageiro(conexaosql);
            // passageiro.EditarPassageiro(conexaosql);

            // passageiro.LocalizarRestrito(conexaosql);

            // passageiro.ConsultarRestrito(conexaosql);

            //passageiro.DeletarRestrito(conexaosql);
            #endregion
            //aeronave.CadastroAeronaves(conexaosql);
           // aeronave.LocalizarAeronave(conexaosql);
           //aeronave.EditarAeronave(conexaosql);
          // aeronave.ConsultarAeronave(conexaosql);
          aeronave.DeletarAeronave(conexaosql);



        }
    }
}
