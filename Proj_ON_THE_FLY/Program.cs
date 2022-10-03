using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;

namespace Proj_ON_THE_FLY {
    internal class Program {

        public static void Menu() {

            Passageiro passageiro = new Passageiro();
            Aeronave aeronave = new Aeronave();
            Conexao cnx = new Conexao();
            CompanhiaAerea cia = new CompanhiaAerea();
            Voo voo = new Voo();
            PassagemVoo pvoo = new PassagemVoo();
            Venda venda = new Venda();

            SqlConnection conexaosql = new SqlConnection(cnx.Caminho());

            Console.Clear();
            Console.WriteLine("\n\t\t*** ON THE FLY ***");

            Console.WriteLine("\nEscolha a opção desejada:\n" +
                              "\n[1] Passagem" +
                              "\n[2] Passageiro" +
                              "\n[3] Cia.Aérea" +
                              "\n[4] Vôos" +
                              "\n[5] Aeronave" +
                              "\n[6] Venda " +
                              "\n[0] Sair");
            Console.Write("\nDigite: ");
            int op = int.Parse(Console.ReadLine());
            while (op < 0 || op > 6) {
                Console.WriteLine("\nOpção Inválida inserida, informe novamente outro valor: ");
                op = int.Parse(Console.ReadLine());
            }
            switch (op) {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    pvoo.MenuPassagem(conexaosql);
                    break;
                case 2:

                    passageiro.MenuPassageiro(conexaosql);
                    break;
                case 3:
                    cia.MenuCiaAerea(conexaosql);
                    break;
                case 4:
                    voo.MenuVoo(conexaosql);
                    break;
                case 5:
                    aeronave.MenuAeronave(conexaosql);
                    break;
                case 6:
                    venda.MenuVenda(conexaosql);
                    break;

            }
        }

        public static void Main(string[] args) {

            Menu();
        }
    }
}
