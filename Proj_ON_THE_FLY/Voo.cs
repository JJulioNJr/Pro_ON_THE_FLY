using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class Voo {
        public String Id { get; set; }
        public String ID_ANAC { get; set; }
        public string Destino { get; set; }
        public int AssentosOcupados { get; set; }
        public DateTime DataVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public Conexao banco;
    
        public Voo() { }

        public Voo(string destino,int asOpc, DateTime dataVoo, DateTime dataCadastro, char situacaoVoo) {
           
            this.Id = "V" + RandomCadastroVoo();
            this.AssentosOcupados = asOpc;
            this.Destino = destino;
            this.DataVoo = dataVoo;
            this.DataCadastro = dataCadastro;
            this.Situacao = situacaoVoo;
        }
        
        #region Gera Numero
        private String RandomCadastroVoo() {
            Random rand = new Random();
            int[] numero = new int[100];
            int aux = 0;
            String convert = "";
            for (int k = 0; k < numero.Length; k++) {
                int rnd = 0;
                do {
                    rnd = rand.Next(1000, 9999);
                } while (numero.Contains(rnd));
                numero[k] = rnd;
                aux = numero[k];
                convert = aux.ToString();
            }
            return convert;
        }
        #endregion

        #region Cadastrar Voo
        public void CadastrarVoo(SqlConnection conecta) {
            //Todas Aeronaves Cadastradas
            string sql = "Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE from AERONAVE;";
            banco = new Conexao();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {

                Console.WriteLine("\n*** Cadastro de Voo ***");

                Console.WriteLine("\nInsira o ID_ANAC do aeronave: ");
                Console.Write("ID_ANAC: ");
                this.ID_ANAC = Console.ReadLine();
                sql = $"select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE from AERONAVE where ID_ANAC = ('{this.ID_ANAC}');";
                Console.Clear();
                //Aeronave Especifica
                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {

                    this.Id = "V" + RandomCadastroVoo();
                    this.Destino = DestinoVoo();

                    Console.WriteLine("\nInforme a data e hora do Voo: (dd/MM/yyyy hh:mm) ");
                    this.DataVoo = DateTime.Parse(Console.ReadLine());
                    if (this.DataVoo <= DateTime.Now) {
                        Console.WriteLine("\nEssa Data é Inválida, informe novamente: ");
                        this.DataVoo = DateTime.Parse(Console.ReadLine());
                    }
                    this.DataCadastro = DateTime.Now;
                    Console.WriteLine("\nData de Cadastro Definida Como: " + this.DataCadastro);
                    Console.WriteLine("\nInforme a Situacao do Voo: \n[A] Ativo \n[C] Cancelado");
                    Console.Write("\nSituacao: ");
                    this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                    while (!this.Situacao.Equals('A') && !this.Situacao.Equals('C')) {
                        Console.WriteLine("O Valor Informado é Inválido, por favor Informe Novamente!\n[A] Ativo \n[C] Cancelado");
                        this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                    }


                    sql = $"Insert into VOO (ID, ID_ANAC,ASSENTOS_OCUPADOS, DATA_CADASTRO,SITUACAO,DATA_VOO,DESTINO) Values ('{this.Id}' , " +
                      $"'{this.ID_ANAC}','{this.AssentosOcupados}' ,'{this.DataCadastro}', '{this.Situacao}', '{this.DataVoo}', '{this.Destino}');";
                    banco.InserirDado(conecta, sql);
                    Console.WriteLine("\nInscrição de Voo realizada com sucesso!");
               
                }else {
                    Console.Clear();
                    Console.WriteLine("\nAeronave não Encontrada...");
                   
                }
            }
        }
        #endregion

        #region Destinos Voo
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

        #region Editar Voo
        public void AtualizarVoo(SqlConnection conecta) {
            Console.Clear();
            Console.WriteLine("\n*** Atualizar dados Voo ***");
            Console.WriteLine("\nInsira o ID do Voo que deseja alterar (Exemplo = 'V1234'): ");
            Console.Write("ID: ");
            this.Id = Console.ReadLine();
            string sql = $"Select ID,ID_ANAC,ASSENTOS_OCUPADOS,DATA_CADASTRO,SITUACAO,DATA_VOO,DESTINO from Voo where Id = '{this.Id}';";
            banco = new Conexao();
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 5))) {
                Console.WriteLine("\nEscolha o que voce deseja atualizar: " +
                                  "\n[1] Destino " +
                                  "\n[2] Aeronave " +
                                  "\n[3] Data de Voo " +
                                  "\n[4] Situação do Voo");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                while (op < 1 || op > 5) {
                    Console.WriteLine("\nOpcao Invalida, informe um valor válido: ");
                    op = int.Parse(Console.ReadLine());
                }
                switch (op) {
                    case 1:
                        Console.WriteLine("\nQual o novo destino que deseja Informar?: ");
                        this.Destino = DestinoVoo();
                        sql = $"UPDATE Voo set Destino = '{this.Destino}' where Id = '{this.Id}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        Console.WriteLine("\nAlteração Efetuada com Sucesso!");
                        break;
                    case 2:

                        sql = $"select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE from AERONAVE ;";
                        banco = new Conexao();
                        if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                            Console.WriteLine("Qual a nova Aeronave que deseja informar?: ");
                            this.ID_ANAC = Console.ReadLine();
                            sql = $"select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE from AERONAVE where ID_ANAC = ('{this.ID_ANAC}');";
                            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                                sql = $"UPDATE Voo set ID_ANAC = '{this.ID_ANAC}' where Id = '{this.Id}' and Id_ANAC='{this.ID_ANAC}';";
                               // banco = new Conexao();
                                banco.EditarDado(conecta, sql);
                                Console.WriteLine("\nAlteração efetuada com sucesso!");
                            }
                            else {
                                Console.WriteLine("\nAeronave não Localizada...");
                            }

                        }
                        else {
                            Console.WriteLine("\nNão Exite Aeronaves Cadastradas...");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Qual a nova data de voo que deseja informar?: ");
                        this.DataVoo = DateTime.Parse(Console.ReadLine());
                        if (this.DataVoo <= DateTime.Now) {
                            Console.WriteLine("Essa data é inválida, informe novamente: ");
                            this.DataVoo = DateTime.Parse(Console.ReadLine());
                        }
                        sql = $"UPDATE Voo set Data_Voo = '{this.DataVoo}' where Id = '{this.Id}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        Console.WriteLine("\nAlteração Efetuada com Sucesso!");
                        break;
                    case 4:
                        Console.WriteLine("\nQual a nova situacão do voo que deseja informar?: \n[A] Ativo \n[C] Cancelado ");
                        Console.Write("\nSituacao: ");
                        this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                        while (!this.Situacao.Equals('A') && !this.Situacao.Equals('C')) {
                            Console.WriteLine("\nO valor informado é inválido, por favor informe novamente!\n[A] Ativo \n[C] Cancelado");
                            Console.Write("\nSituacao: ");
                            this.Situacao = char.Parse(Console.ReadLine().ToUpper());

                        }
                        sql = $"UPDATE Voo set Situacao = '{this.Situacao}' where Id = '{this.Id}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        break;
                    default:
                        Console.WriteLine("\nOpção Invalida!!!");
                        break;
                }
            }
            else {
                Console.WriteLine("\nValor de ID de voo não encontrado!!");
              
            }
        }
        #endregion

        #region Localizar Voo
        public void LocalizarVoo(SqlConnection conecta) {
            Console.WriteLine("\n*** Localizar Voo ***");
            Console.WriteLine("\nInsira o ID do Voo que deseja Localizar (Exemplo = 'V1234'): ");
            Console.Write("Digite: ");
            this.Id = Console.ReadLine();
            String sql = $"Select ID, ID_ANAC,ASSENTOS_OCUPADOS, DATA_CADASTRO,SITUACAO,DATA_VOO,DESTINO From Voo where Id=('{this.Id}'); ";
            Console.Clear();
            banco = new Conexao();
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 5))) {
                 Console.WriteLine();
            }
            else {
                Console.WriteLine("\nO voo nao foi encontrado!");
              
            }
        }
        #endregion

        #region Registro por Registro
        public void RegistroPorRegistro(SqlConnection conecta) {
             List<string> voo = new();
             conecta.Open();
             string sql = "Select ID, ID_ANAC,ASSENTOS_OCUPADOS, DATA_CADASTRO,SITUACAO,DATA_VOO,DESTINO From Voo";
             SqlCommand cmd = new SqlCommand(sql, conecta);
             SqlDataReader reader = null;
             using (reader = cmd.ExecuteReader()) {
                 Console.WriteLine("\n\t*** Voo Localizado ***\n");
                 while (reader.Read()) {
                     if (reader.GetString(3).Equals("A")) {
                         voo.Add(reader.GetString(0));
                     }
                 }
             }
             conecta.Close();
             for (int i = 0; i < voo.Count; i++) {
                 string op;
                 do {
                     Console.Clear();
                     Console.WriteLine("\n*** Voos ***\nDigite para navegar:\n[1] Próximo Cadasatro\n[2] Cadastro Anterior" +
                         "\n[3] Último cadastro\n[4] Voltar ao Início\n[s] Sair\n");
                     Console.WriteLine($"Cadastro [{i + 1}] de [{voo.Count}]");
                     //Imprimi o primeiro da lista
                     string query = $"Select ID, ID_ANAC,ASSENTOS_OCUPADOS, DATA_CADASTRO,SITUACAO,DATA_VOO,DESTINO From Voo where Id=('{voo[i]}'); ";
                    //"Select Id, InscricaoAeronave, DataVoo, DataCadastro, Destino, Situacao from Voo where Id = '" + voo[i] + "';";
                     banco = new Conexao();
                     banco.LocalizarDado(conecta, query, 5);
                     Console.Write("Opção: ");
                     op = Console.ReadLine();
                     if (op != "1" && op != "2" && op != "3" && op != "4" && op != "s") {
                         Console.WriteLine("Opção inválida!");
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
                         i = voo.Count - 1;
                     //Volta para o inicio da lista
                     else if (op.Contains("4"))
                         i = 0;
                     //Vai para o próximo da lista
                 } while (op != "1");
                 if (i == voo.Count - 1)
                     i--;
             }
        }
        #endregion

        #region Menu Voo
        public void MenuVoo(SqlConnection conecta) {
            do {
              
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\t     ***** Menu de Voo *****");
                Console.WriteLine("\nEscolha a opção desejada:\n" +
                                  "\n[1] Voltar ao Menu anterior" +
                                  "\n[2] Cadastrar" +
                                  "\n[3] Localizar" +
                                  "\n[4] Editar" +
                                  "\n[5] Imprimir por registro" +
                                  "\n[0] Sair");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                while (op < 0 || op > 5) {
                    Console.WriteLine("\nOpção inválida, informe novamente: ");
                    Console.WriteLine("\nEscolha a opção desejada:\n" +
                                      "\n[1] Voltar ao Menu anterior" +
                                      "\n[2] Cadastrar" +
                                      "\n[3] Localizar" +
                                      "\n[4] Editar" +
                                      "\n[5] Imprimir por registro" +
                                      "\n[0] Sair");
                    Console.Write("\nDigite: ");
                    op = int.Parse(Console.ReadLine());
                }
                switch (op) {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.Clear();
                        Program.Menu();
                        break;
                    case 2:
                        CadastrarVoo(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                  
                        break;
                    case 3:
                        LocalizarVoo(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                       
                        break;
                    case 4:
                        AtualizarVoo(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                    case 5:
                        RegistroPorRegistro(conecta);
                      
                        break;
                    default:
                        break;
                }
            } while (true);
        }
        #endregion

    }
}
