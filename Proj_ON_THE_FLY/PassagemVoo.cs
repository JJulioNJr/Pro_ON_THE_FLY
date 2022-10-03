using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class PassagemVoo {

        public String IdPassagem { get; set; }
        public Voo voo { get; set; } //idVoo
        public DateTime DataUltimaOperacao { get; set; }
        public float Valor { get; set; }
        public string SituacaoPassagem { get; set; }

        public Conexao banco;

        public PassagemVoo() { }

        public PassagemVoo(string id, string idvoo, Aeronave idnave, DateTime dataUltimaOperacao, float valor, string situacao) {

            this.voo.Id = idvoo;
            this.DataUltimaOperacao = dataUltimaOperacao;
            this.Valor = valor;
            this.SituacaoPassagem = situacao;
        }

        #region Gerar Id
        public String GeradorDeID() {
            Random rand = new Random();
            int[] numero = new int[100];
            string convert = "";
            int aux = 0;
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

        #region Cadastro de Passagens
        public void CadastrarPassagem(SqlConnection conecta, string idVoo) {
            Voo voo = new Voo();
            banco = new Conexao();
            string sql = "Select Id from Voo;";
            int verificar = banco.VerificarExiste(sql);
            if (verificar != 0) {

                this.IdPassagem = "PA" + GeradorDeID();
                Console.WriteLine("Id da passagem definido como: " + this.IdPassagem);
                voo.Id = idVoo;
                Console.WriteLine("Id de voo: " + voo.Id);
                this.DataUltimaOperacao = DateTime.Now;
                string parametro = "Destino";
                sql = $"select Destino from Voo where Id = '{idVoo}';";

                string destino = banco.RetornoDados(sql, conecta, parametro);
                voo.Destino = destino;
                if (voo.Destino == "BSB") {
                    this.Valor = 1500;
                }
                else if (voo.Destino == "CGH") {
                    this.Valor = 2500;
                }
                else if (voo.Destino == "GIG") {
                    this.Valor = 3000;
                }
                Console.WriteLine("valor da passagem: " + this.Valor);
                Console.WriteLine("Informe a situação da passagem (P - Paga | R - Reservada): ");
                this.SituacaoPassagem = Console.ReadLine().ToUpper();
                while (!this.SituacaoPassagem.Equals("P") && !this.SituacaoPassagem.Equals("R")) {
                    Console.WriteLine("Valor informado inválido, informe novamente:");
                    this.SituacaoPassagem = Console.ReadLine().ToUpper();
                }
                Console.WriteLine("\nSituação da passagem: " + this.SituacaoPassagem);
                string query = $"\nInsert Into Passagens(ID, ID_VOO, DATA_ULTIMA_OPERACAO, VALOR, SITUACAO) values " +
                    $"('{this.IdPassagem}','{voo.Id}','{this.DataUltimaOperacao}','{this.Valor}','{this.SituacaoPassagem}');";
                banco.InserirDado(conecta, query);
            }
            else {
                Console.Clear();
                Console.WriteLine("\nImpossível Cadastrar Passagem, pois nao temos nenhuma Voo Cadastrado");
            }

        }
        #endregion

        #region Editar Campos da Passagens
        public void EditarPassagem(SqlConnection conecta) {
            int opc = 0;
            
            Console.Clear();
            Console.WriteLine("\n*** Atualizar Passagem ***");
            Console.WriteLine("\nInforme o ID da passagem (PA0000 – Dois dígitos PA, seguidos de 4 dígitos numéricos:");
            Console.Write("Digite: ");
            this.IdPassagem = Console.ReadLine();
            string query = $"Select ID, ID_VOO, DATA_ULTIMA_OPERACAO, VALOR, SITUACAO from Passagens where  id = '{this.IdPassagem} ';";
            banco = new Conexao();
            Console.Clear();
            if (string.IsNullOrEmpty(banco.LocalizarDado(conecta, query, 8))) {
                Console.WriteLine("\nPassagem nao localizada!!");
            }
            else {
               
                Console.WriteLine("\n*** Alterar Passagem ***");
                Console.WriteLine("\nDigite a Opção que Deseja Editar");
                Console.WriteLine("\n[1] Ultima Data Alterada");
                Console.WriteLine("\n[2] Situacao");
                Console.WriteLine("\n[3] Valor");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());
                while (opc < 1 || opc > 3) {
                    Console.WriteLine("\nDigite uma Opcao Valida:");
                    Console.Write("\nDigite: ");
                    opc = int.Parse(Console.ReadLine());
                }
                switch (opc) {
                    case 1:
                       
                        Console.WriteLine("*** Alterar Passagem ***");
                        Console.WriteLine("\nData alterada com sucesso! ");
                        this.DataUltimaOperacao = DateTime.Now;
                        string sql = $"Update Passagens set Data_Ultima_Operacao = '{this.DataUltimaOperacao}' where Id = '{this.IdPassagem}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        Console.WriteLine("\nCampo Alterado Com Sucesso!!!\n");
                        break;
                    case 2:
                       
                        Console.WriteLine("\n*** Alterar Passagem ***");
                        Console.WriteLine("\nInforme a nova situacao (L – Livre, R – Reservada ou P – Paga): ");
                        this.SituacaoPassagem = Console.ReadLine().ToUpper();
                        while (this.SituacaoPassagem != "L" && this.SituacaoPassagem != "R" && this.SituacaoPassagem != "P") {
                            Console.WriteLine("\nSituacao de passagem inválida informada, digite o novamente: ");
                            this.SituacaoPassagem = Console.ReadLine();
                        }
                        sql = $"Update Passagens set Situacao = '{this.SituacaoPassagem}' where Id = '{this.IdPassagem}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        Console.WriteLine("\nCampo Alterado Com Sucesso!!!\n");
                        break;
                    case 3:
                       
                        Console.WriteLine("\n*** Alterar Passagem ***");
                        Console.WriteLine("\nInforme o Novo Valor da Passagem: ");
                        this.Valor = float.Parse(Console.ReadLine());
                        while (this.Valor <999 || this.Valor > 9999) {
                            Console.WriteLine("Valor Inválido Informado, Tente Novamente: ");
                            this.Valor = float.Parse(Console.ReadLine());
                        }
                        sql = $"update Passagens set Valor = '{this.Valor}' where Id = '{this.IdPassagem}';";
                        banco = new Conexao();
                        banco.EditarDado(conecta, sql);
                        Console.WriteLine("\nCampo Alterado Com Sucesso!!!\n");
                        break;
                    default:
                        Console.WriteLine("\nOpcao Invalida");
                        break;
                }
            }
        }
        #endregion

        #region Localizar Passagem Especifica
        public void LocalizarPassagem(SqlConnection conecta) {
            Console.Clear();
            banco = new Conexao();
            Console.WriteLine("\n*** Localizar Passagem ***");
            Console.WriteLine("\nInforme o ID da passagem (PA0000 – Dois dígitos PA, seguidos de 4 dígitos numéricos:");
            Console.Write("Digite: ");
            this.IdPassagem = Console.ReadLine();
            string query = $"Select Id, Id_Voo From Passagens where id = '{this.IdPassagem}';";
            int verificar = banco.VerificarExiste(query);
            if (verificar == 0) {
                Console.WriteLine("\nPassagem nao localizada!!");

            }else {
                Console.Clear();
                string sql = $"Select ID, ID_VOO, DATA_ULTIMA_OPERACAO, VALOR, SITUACAO from Passagens where id = '{this.IdPassagem}';";
                banco = new Conexao();
                banco.LocalizarDado(conecta, sql,8);
            }
        }
        #endregion

        #region Menu de passagem
        public void MenuPassagem(SqlConnection conecta) {
            do {
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\t     *** Menu de Passagem ***");
                Console.WriteLine("\nEscolha a opção desejada:\n" +
                                  "\n[1] Voltar ao Menu anterior" +
                                  "\n[2] Localizar" +
                                  "\n[3] Editar" +
                                  "\n[4] Imprimir por registro" +
                                  "\n[0] Sair");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                while (op < 0 || op > 5) {
                    Console.WriteLine("\nOpção inválida, informe novamente: ");
                    Console.WriteLine("\nEscolha a opção desejada:\n" +
                                      "\n[1] Voltar ao Menu anterior" +
                                      "\n[2] Localizar" +
                                      "\n[3] Editar" +
                                      "\n[4] Imprimir por registro" +
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
                        LocalizarPassagem(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuPassagem(conecta);
                        break;
                    case 3:
                        EditarPassagem(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuPassagem(conecta);
                        break;
                    case 4:
                        RegistroPorRegistro(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        MenuPassagem(conecta);
                        break;

                }
            } while (true);
        }
        #endregion

        #region Rigistro por Registro de Consulta
        public void RegistroPorRegistro(SqlConnection conecta) {
            List<string> Passagem = new();
            banco = new Conexao();
            conecta.Open();
            string sql = "Select ID, ID_VOO, DATA_ULTIMA_OPERACAO, VALOR, SITUACAO from Passagens";
            SqlCommand cmd = new SqlCommand(sql, conecta);
            SqlDataReader reader = null;
            using (reader = cmd.ExecuteReader()) {
                Console.WriteLine("\n\t*** Passagem Localizada ***\n");
                while (reader.Read()) {
                    Passagem.Add(reader.GetString(0));
                }
            }
            conecta.Close();
            for (int i = 0; i < Passagem.Count; i++) {
                string op;
                do {
                    Console.Clear();
                    Console.WriteLine("*** Passagem ***" +
                                     "\nDigite para navegar:" +
                                     "\n[1] Próximo Cadasatro" +
                                     "\n[2] Cadastro Anterior" +
                                     "\n[3] Último cadastro" +
                                     "\n[4] Voltar ao Início" +
                                     "\n[s] Sair\n");
                    Console.WriteLine($"Cadastro [{i + 1}] de [{Passagem.Count}]");
                    //Imprimi o primeiro da lista
                    string query = $"Select ID, ID_VOO, DATA_ULTIMA_OPERACAO, VALOR, SITUACAO from Passagens where id = '{Passagem[i]}';";
                    banco.LocalizarDado(conecta, query, 8);
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
                        i = Passagem.Count - 1;
                    //Volta para o inicio da lista
                    else if (op.Contains("4"))
                        i = 0;
                    //Vai para o próximo da lista
                } while (op != "1");
                if (i == Passagem.Count - 1)
                    i--;
            }
        }
        #endregion 
    }
}
