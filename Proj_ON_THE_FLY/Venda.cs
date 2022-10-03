using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class Venda {



        public int Id_Venda { get; set; }
        public DateTime Data_venda { get; set; }
        public double Valor_Total { get; set; }
        public String Id_Passagem { get; set; }
        public string Cpf { get; set; }

        public PassagemVoo passagem { get; set; }
        public Voo voo = new Voo();
        public Conexao banco;
        public Passageiro passageiro;

        public Venda() { }

        #region Gera Id
        private int RandomCadastroVenda() {
            Random rand = new Random();
            int[] numero = new int[100];
            int aux = 0;
            for (int k = 0; k < numero.Length; k++) {
                int rnd = 0;
                do {
                    rnd = rand.Next(1000, 9999);
                } while (numero.Contains(rnd));
                numero[k] = rnd;
                aux = numero[k];
            }
            return aux;
        }
        #endregion
       
        #region Passagem Venda
        public void CadastrarVenda(SqlConnection conexaosql) {
            passageiro = new Passageiro();
            banco = new Conexao();
            int assentosOcupados, capacidade;
            double valorPassagem;
            Console.Clear();
            string sql = $"select cpf from Passageiro", parametro, Inscricao, idPassagem;
            Console.WriteLine("\n*** Menu Vendas ***");
            int verificarCpf = banco.VerificarExiste(sql);
            if (verificarCpf != 0) {
                Console.Write("\nInforme seu CPF: ");
                this.Cpf = Console.ReadLine();
                while (passageiro.ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                    Console.WriteLine("\nCpf invalido, digite novamente");
                    Console.Write("CPF: ");
                    this.Cpf = Console.ReadLine();
                }
                sql = $"Select CPF From RESTRITO where CPF='{this.Cpf}';";
                if (string.IsNullOrEmpty(banco.LocalizarDado(conexaosql, sql, 2))) {
                    Console.Clear();
                    Console.WriteLine("\nGostaria de iniciar uma venda de passagens?\n\n[1]Sim / [2]Não");
                    Console.Write("Digite: ");
                    int op = int.Parse(Console.ReadLine());
                    while (op != 1 && op != 2) {
                        Console.WriteLine("\nValor inválido informado, informe novamente: ");
                        op = int.Parse(Console.ReadLine());
                    }
                    switch (op) {

                        case 1:
                            banco = new Conexao();
                            passagem = new PassagemVoo();

                            this.Data_venda = DateTime.Now;
                            Console.WriteLine("\nData da venda: " + this.Data_venda);
                            Console.WriteLine("\nQuantas passagens voce gostaria de adquirir? (Maximo 4 por Venda)");//perguntar quantas passagem!! 
                            Console.Write("Digite: ");
                            int contaPassagem = int.Parse(Console.ReadLine());
                            while (contaPassagem < 1 || contaPassagem > 4) {
                                Console.WriteLine("\nNumero inválido de passagens, insira outro valor: ");
                                Console.Write("Digite: ");
                                contaPassagem = int.Parse(Console.ReadLine());
                            }
                            sql = $"select Id, Situacao from Voo where Situacao = 'A'";
                            int verificarVoo = banco.VerificarExiste(sql);
                            if (verificarVoo != 0) {
                                Console.WriteLine("\nInforme o Id do Voo desejado (Ex = 'V1234', Caso nao saiba o id do Voo, vá ao menu 'Voo' e consulte o ID!): ");
                                Console.Write("Digite: ");
                                string idVoo = Console.ReadLine();
                                sql = $"select Id from Voo where Id = '{idVoo}';";
                                verificarVoo = banco.VerificarExiste(sql);
                                if (verificarVoo != 0) {
                                    #region Coletando Dados da DB
                                    sql = $"select Assentos_Ocupados from Voo where Id = '{idVoo}';";
                                    parametro = "Assentos_Ocupados";
                                    assentosOcupados = Convert.ToInt32(banco.RetornoDados(sql, conexaosql, parametro));

                                    sql = $"select ID_ANAC from Voo where Id = '{idVoo}';";
                                    parametro = "ID_ANAC";
                                    Inscricao = banco.RetornoDados(sql, conexaosql, parametro);

                                    sql = $"select Capacidade from Aeronave where ID_ANAC = '{Inscricao}';";
                                    parametro = "Capacidade";
                                    capacidade = Convert.ToInt32(banco.RetornoDados(sql, conexaosql, parametro));

                                    #endregion
                                    if ((assentosOcupados + contaPassagem) < (capacidade)) {
                                        for (int i = 0; i < contaPassagem; i++) {
                                            this.Id_Venda = RandomCadastroVenda();
                                            Console.WriteLine($"\n\n*** Cadastro de passagem [{i + 1}]***");

                                            passagem.CadastrarPassagem(conexaosql, idVoo);
                                            assentosOcupados = +1;
                                            sql = $"select Valor from Passagens where Id_Voo = '{idVoo}';";
                                            parametro = "Valor";
                                            valorPassagem = Convert.ToDouble(banco.RetornoDados(sql, conexaosql, parametro));
                                            this.Valor_Total = valorPassagem * contaPassagem;
                                            sql = $"select Id from Passagens where Id_Voo = '{idVoo}';";
                                            parametro = "Id";
                                            idPassagem = banco.RetornoDados(sql, conexaosql, parametro);
                                            string sqll = $"insert into PassagemVenda(Id, DataVenda, ValorTotal, IDItemVenda, Passageiro,ValorUnitario, Voo) values ('{this.Id_Venda}', " +
                                            $"'{this.Data_venda}','{this.Valor_Total}','{idPassagem}','{this.Cpf}','{valorPassagem}','{idVoo}');";
                                            banco.InserirDado(conexaosql, sqll);
                                            sqll = $"insert VendaPassageiro(DataVenda, ValorTotal, Cpf) values ('{DateTime.Now}','{this.Valor_Total}','{this.Cpf}');";
                                            banco.InserirDado(conexaosql, sqll);
                                            //AssentosOcupados
                                            string update = $"Update Voo set Assentos_Ocupados = {assentosOcupados + 1} where Id = '{idVoo}'";
                                            banco.EditarDado(conexaosql, update);
                                            //Capacidade do Voo
                                            update = $"Update Aeronave set Capacidade = {capacidade - 1} where ID_ANAC = '{Inscricao}'";
                                            banco.EditarDado(conexaosql, update);
                                            Console.WriteLine("\n*** Cadastro de Venda com Sucesso ***\nPressione uma tecla para prosseguir!");
                                            Console.ReadKey();
                                        }
                                    }
                                    else {
                                        Console.WriteLine("\nAssentos insulficientes, volte ao menu e informe uma quantidade menor!");
                                        Console.Clear();
                                        Program.Menu();
                                    }
                                }
                                else {
                                    Console.WriteLine("\nO voo nao foi encontrado, tente novamente depois!");
                                    Console.Clear();
                                    Program.Menu();

                                }
                                break;
                            }
                            else {
                                Console.WriteLine("\nNao existem Voos Cadastrados, impossível realizar venda!!");
                                Console.Clear();
                                Program.Menu();

                            }
                            break;
                        case 2:
                            MenuVenda(conexaosql);
                            break;

                    }
                }
                else {
                    Console.WriteLine("\nPassageiro Restrito Venda de Passagens Não Autorizadas....\nAperte enter para sair.");
                    Console.ReadKey();
                    Console.Clear();
                    Program.Menu();
                }


            }
            else {
                Console.WriteLine("\nVenda não pode ser finalizada, Não existem passageiros cadastrados! \nAperte enter para sair.");
                Console.ReadKey();
            }

        }
        #endregion

        #region Menu de Vendas
        public void MenuVenda(SqlConnection conecta) {
            int op = 0;
            do {
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\t      *** Menu de Venda ***");
                Console.WriteLine("\nEscolha a opção desejada:\n" +
                                  "\n[1] Voltar ao Menu" +
                                  "\n[2] Cadastrar" +
                                  "\n[3] Localizar" +
                                  "\n[4] Deletar" +
                                  "\n[5] Buscar por Resgistro" +
                                  "\n[0] Sair");
                Console.Write("\nDigite: ");
                op = int.Parse(Console.ReadLine());
                CompanhiaAerea cia = new CompanhiaAerea();
                switch (op) {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:

                        Console.Clear();
                        Program.Menu();
                        break;
                    case 2:
                        CadastrarVenda(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuVenda(conecta);
                        break;

                    case 3:
                        LocalizarVenda(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuVenda(conecta);

                        break;
                    case 4:
                        DeletarVenda(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuVenda(conecta);
                        break;
                    case 5:
                        RegistroPRegistroVenda(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuVenda(conecta);
                        break;
                    default:
                        if (op < 0 || op > 6) {

                            Console.WriteLine("\n\nOpcao Invalida!!");
                            Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                            Console.ReadKey();
                            Console.Clear();
                            MenuVenda(conecta);
                        }
                        break;

                }
            } while (op < 0 || op > 5);
        }
        #endregion

        #region Seleciona Destino Voo
        public string DestinoVoo() {
            List<string> destinoVoo = new List<string>();
            destinoVoo.Add("BSB");
            destinoVoo.Add("CGH");
            destinoVoo.Add("GIG");
            Console.WriteLine("Destinos atualmente disponíves: ");
            Console.WriteLine("BSB - Aeroporto Internacional de Brasilia");
            Console.WriteLine("CGH - Aeroporto Internacional de Congonhas/SP");
            Console.WriteLine("GIG - Aeroporto Internacional do Rio de Janeiro");
            Console.WriteLine("");
            do {
                Console.Write("Informe a sigla do destino de voo: ");
                String destinoEscolhido = Console.ReadLine().ToUpper();
                if (destinoVoo.Contains(destinoEscolhido)) {
                    return destinoEscolhido;
                }
                else {
                    Console.WriteLine("Destino inválido, informe novamente!");
                    Console.WriteLine("");
                }
            } while (true);
        }
        #endregion

        #region Localizar Venda Especifica
        public void LocalizarVenda(SqlConnection conexaosql) {
            Console.Clear();
            banco = new Conexao();
            Console.WriteLine("\n*** Localizar Venda ***");
            Console.WriteLine("Informe o ID da venda (4 dígitos numéricos):");
            int id = int.Parse(Console.ReadLine());
            string query = $"select Id from PassagemVenda where Id = '{id}';";
            int verificar = banco.VerificarExiste(query);
            if (verificar == 0) {
                Console.WriteLine("Passagem nao localizada!!");
               
            }
            else {
                string sql = $"Select Id, DataVenda, ValorTotal,IDItemVenda,Passageiro, ValorUnitario,Voo from PassagemVenda where id = '{id}';";
                Console.Clear();
                banco.LocalizarDado(conexaosql, sql, 7);
               
            }
        }
        #endregion
       
        #region Deletear Venda passagem
        public void DeletarVenda(SqlConnection conexaosql) {
            Console.Clear();
            banco=new Conexao();
            Console.WriteLine("\n*** Deletar Venda ***");
            Console.WriteLine("\nInforme o ID da venda (4 dígitos numéricos):");
            Console.Write("Digite: ");
            int id = int.Parse(Console.ReadLine());
            string query = $"select Id from PassagemVenda where Id = '{id}';";
            int verificar = banco.VerificarExiste(query);
            if (verificar == 0) {
                Console.WriteLine("Passagem nao localizada!!");
                
            }
            else {
                
                string sql = $"Select Id, DataVenda, ValorTotal,IDItemVenda,Passageiro, ValorUnitario,Voo from PassagemVenda where id = '{id}';";
                banco.LocalizarDado(conexaosql, sql, 7);
                string delete = $"Delete from PassagemVenda where Id = '{id}';";
                banco.DeletarDado(conexaosql, delete);
                Console.WriteLine("\nVenda Deletada com sucesso...");
               
            }
        }
        #endregion

        #region Percorrer Registro
        public void RegistroPRegistroVenda(SqlConnection conecta) {
            List<string> venda = new();
            banco =new Conexao();
            conecta.Open();
            string sql = "Select Id, DataVenda, ValorTotal,IDItemVenda,Passageiro, ValorUnitario,Voo from PassagemVenda";
            SqlCommand cmd = new SqlCommand(sql, conecta);
            SqlDataReader reader = null;
            using (reader = cmd.ExecuteReader()) {
                Console.WriteLine("\n\t*** Venda Localizada ***\n");
                while (reader.Read()) {
                    venda.Add(Convert.ToString(reader.GetInt32(0)));
                }
            }
            conecta.Close();
            for (int i = 0; i < venda.Count; i++) {
                string op;
                do {
                    Console.Clear();
                    Console.WriteLine("\n*** Venda ***" +
                        "\nDigite para navegar:" +
                        "\n[1] Próximo Cadasatro" +
                        "\n[2] Cadastro Anterior" +
                        "\n[3] Último cadastro" +
                        "\n[4] Voltar ao Início" +
                        "\n[s] Sair\n");
                    Console.WriteLine($"Cadastro [{i + 1}] de [{venda.Count}]");
                    //Imprimi o primeiro da lista
                    string query = $"Select Id, DataVenda, ValorTotal,IDItemVenda,Passageiro, ValorUnitario,Voo from PassagemVenda where Id = '{venda[i]}';";
                    banco.LocalizarDado(conecta, query, 7);
                    Console.Write("Opção: ");
                    op = Console.ReadLine();
                    if (op != "1" && op != "2" && op != "3" && op != "4" && op != "s") {
                        Console.WriteLine("\nOpção inválida!");
                        Thread.Sleep(2000);
                    }
                    //Sai do método
                    else if (op.Contains("s"))
                        return;
                    //Volta no Cadastro Anterior
                    else if (op.Contains("2"))
                        if (i == 0)
                            i = 0;
                        else
                            i--;
                    //Vai para o fim da lista
                    else if (op.Contains("3"))
                        i = venda.Count - 1;
                    //Volta para o inicio da lista
                    else if (op.Contains("4"))
                        i = 0;
                    //Vai para o próximo da lista
                } while (op != "1");
                if (i == venda.Count - 1)
                    i--;
            }
        }
        #endregion


    }
}
