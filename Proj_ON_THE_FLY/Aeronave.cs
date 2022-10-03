using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal  class Aeronave {

        public String Inscricao { get; set; }
        public String Capacidade { get; set; }
        public String CNPJ { get; set; }

        public DateTime UltimaVenda { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }

        public Conexao banco;

        public CompanhiaAerea cia;
     

        public Aeronave() { }
        public Aeronave(String Inscrição, char Situacao, String Capacidade) {
            
            this.Inscricao = Inscrição;
            this.Capacidade = Capacidade;
            this.Situacao = Situacao;
        }

        #region GeraNumeros
        public String GeraNumero() {
            Random rand = new Random();
            int[] numero = new int[100];
            int aux = 0;
            String convert = "";
            for (int k = 0; k < numero.Length; k++) {
                int rnd = 0;
                do {
                    rnd = rand.Next(100, 999);
                } while (numero.Contains(rnd));
                numero[k] = rnd;
                aux = numero[k];
                convert = aux.ToString();
                break;
            }
            return convert;
        }
        #endregion

        #region Cadastra Aeronave
        public void CadastrarAeronave(SqlConnection conecta) {
            int cap = 0;
            cia = new CompanhiaAerea();
            Console.Clear();
            Console.WriteLine("\n*** Cadastro de Aeronave ***\n");
            Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");
            Console.Write("CNPJ: ");
            this.CNPJ = Console.ReadLine();
           
            while (cia.ValidarCnpj(this.CNPJ) == false || this.CNPJ.Length < 14) {
                Console.WriteLine("\nCNPJ Invalido, Digite Novamente");
                Console.Write("\nLocalizar Cia Aerea Informe o CNPJ: ");
                this.CNPJ = Console.ReadLine();
            }
            
            String sql = $"SELECT CNPJ,RAZAO_SOCIAL,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO FROM COMPANHIA_AEREA WHERE CNPJ = '{this.CNPJ}';";
            banco = new Conexao();
            Console.WriteLine("\n\t*** Companhia Aerea Cadastrada ****\n");
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 4))) {
                
                this.Inscricao = "PR-" + GeraNumero();

                do {
                    Console.Write("\nInforme a Capacidade da Aeronave: ");
                    cap = int.Parse(Console.ReadLine());
                    if (cap < 100 || cap > 999) {
                        Console.Clear();
                        Console.WriteLine("\nCapacidade Informada Inválida...!!!" +
                                          "\nInforme Novamente!!!");

                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    this.Capacidade = cap.ToString();

                } while (cap < 100 || cap > 999);


                do {
                    Console.Write("\nInfome a Situação da Aeronave:" +
                                  "\nA-Ativo ou I-Inativo\n" +
                                  "\nSituação: ");
                    this.Situacao = char.Parse(Console.ReadLine().ToUpper());

                } while (!this.Situacao.Equals('A') && !this.Situacao.Equals('I'));

                this.UltimaVenda = DateTime.Now;
                this.DataCadastro = DateTime.Now;

                Console.WriteLine("\nDeseja Salvar o Cadastrado de Aeronave? ");
                Console.WriteLine("1- Sim / 2-Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());


                if (op == 1) {
                    sql = $"Insert Into Aeronave(ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE) " +
                                $"Values ('{this.Inscricao}','{this.CNPJ}','{this.DataCadastro}','{this.Situacao}','{this.UltimaVenda}','{this.Capacidade}');";
                    banco = new Conexao();
                    banco.InserirDado(conecta, sql);
                    Console.WriteLine("\nCadastro de Aeronave Salvo com Sucesso!");

                }else {
                    Console.WriteLine("\nCadastro de Aeronave Não Foi Acionado... ");

                }
            }else {
                Console.WriteLine("\n Companhia Aerea Não Encontrata....");
            }
        }
        #endregion

        #region Localizar Aeronave
        public void LocalizarAeronave(SqlConnection conecta) {
            cia = new CompanhiaAerea();
            Console.WriteLine("\n*** Localizar Aeronave Especifico ***");

            Console.WriteLine("\nDeseja Localizar uma Aeronave no Cadastro? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Aeronave Especifico ***");
                Console.Write("\n Digite a ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();

                while (this.Inscricao.Length < 6) {
                    Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                    Console.Write("ID_ANAC: ");
                    this.Inscricao = Console.ReadLine().ToUpper();
                }

                Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");
                Console.Write("CNPJ: ");
                this.CNPJ = Console.ReadLine();
                while (cia.ValidarCnpj(this.CNPJ) == false || this.CNPJ.Length < 14) {
                    Console.WriteLine("\nCNPJ Invalido, Digite Novamente");
                    Console.Write("\nLocalizar Cia Aerea Informe o CNPJ: ");
                    this.CNPJ= Console.ReadLine();
                }


                String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
          
                banco=new Conexao();
                Console.Clear();

                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                    Console.WriteLine();
                }
                else {
                    Console.WriteLine("\n\tAeronove não Encontrado!!!");
                }
            }
            else {
                Console.WriteLine("Localização de Aeronave Não foi Acionada...");
            }
        }
        #endregion

        #region Deletar Aeronave
        public void DeletarAeronave(SqlConnection conecta) {
            cia = new CompanhiaAerea();
            Console.Clear();
            Console.WriteLine("\n*** Deletar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao =Console.ReadLine().ToUpper();

            while (this.Inscricao.Length < 6) {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }
            
            Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");
            Console.Write("CNPJ: ");
            this.CNPJ = Console.ReadLine();
            while (cia.ValidarCnpj(this.CNPJ) == false || this.CNPJ.Length < 14) {
                Console.WriteLine("\nCNPJ Invalido, Digite Novamente");
                Console.Write("\nLocalizar Cia Aerea Informe o CNPJ: ");
                this.CNPJ = Console.ReadLine();
            }
            
            Console.Clear();
            String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
            banco = new Conexao();
          
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                Console.WriteLine("Deseja Deletar Aeronave? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());

                if (op == 1) {

                    sql = $"Delete From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
                    banco = new Conexao();
                    banco.DeletarDado(conecta, sql);
                    Console.WriteLine("\nCadastro de Aeronave Deletado com sucesso!");

                }
                else {
                    Console.WriteLine("\nDeletar Aeronave Não Foi Acionado ...");

                }
            }
            else {
                Console.WriteLine("\nAeronave não Encontrada!!!");
            }
        }
        #endregion

        #region EditarAeronave
        public void EditarAeronave(SqlConnection conecta) {
            int opc = 0;

            Console.Clear();
            Console.WriteLine("\n*** Editar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao = Console.ReadLine().ToUpper();

            while (this.Inscricao.Length < 6) {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }
            
            Console.Clear();
            String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}');";
            banco = new Conexao();

          
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                Console.WriteLine("\nDeseja Alterar a Informação de algum Campo ? ");
                Console.Write("1- Sim / 2- Não: ");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("\n*** Editar Campos da Aeronave ***\n");
                if (opc == 1) {
                    Console.WriteLine("\nDigite a Opção que Deseja Editar");
                    Console.WriteLine("1-Data_Cadastro");
                    Console.WriteLine("2-Ultima Venda");
                    Console.WriteLine("3-Situacao");
                    Console.WriteLine("4-Capacidade");
                    Console.Write("\nDigite: ");
                    opc = int.Parse(Console.ReadLine());
                    while (opc < 1 || opc > 4) {
                        Console.WriteLine("\nDigite uma Opcao Valida:");
                        Console.Write("\nDigite: ");
                        opc = int.Parse(Console.ReadLine());

                    }

                    switch (opc) {
                        case 1:
                            Console.Write("\nAlterar o Data de Cadastro: ");
                            this.DataCadastro = DateTime.Parse(Console.ReadLine());
                            sql = $"Update AERONAVE Set DATA_CADASTRO=('{this.DataCadastro}') Where ID_ANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nData de Cadastro Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 2:
                            Console.Write("\nAlterar a  Data da Ultima Venda: ");
                            this.UltimaVenda = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("\nData da Ultima Venda Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            sql = $"Update AERONAVE Set ULTIMA_VENDA=('{this.UltimaVenda}') Where ID_ANAC=('{this.Inscricao}');";
                            break;
                        case 3:
                            Console.Write("\nAlterar Situação: ");
                            Console.WriteLine("\nA-Ativo ou I-Inativo");
                            Console.Write("Situacao: ");
                            this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                            sql = $"Update AERONAVE Set SITUACAO=('{this.Situacao}') Where ID_ANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nSituacao Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 4:
                            Console.Write("\nAlterar a Capacidade: ");
                            this.Capacidade = Console.ReadLine();
                            sql = $"Update AERONAVE Set CAPACIDADE=('{this.Capacidade}') Where ID_ANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nCapacidade Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;

                    }
                   
                    banco = new Conexao();
                    banco.EditarDado(conecta, sql);
                }
            }
            else {
                Console.WriteLine("\n\tAeronove Não Encontrada!!!");
            }
        }
        #endregion

        #region Consulta Aeronave
        public void ConsultarAeronave(SqlConnection conecta) {
            int op = 0;
            String sql = "";
        
            Console.Write("\nDeseja Concultar Situação de Aeronave" +
                             "\n1-Ativo , 2-Inativo , 3-Geral\n" +
                             "\nConsulta: ");
            op = int.Parse(Console.ReadLine());

            switch (op) {

                case 1:
                    Console.Clear();
                    Console.Write("\n\t*** Aeronaves Ativas ***\n");
                    this.Situacao = 'A';
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where SITUACAO=('{this.Situacao}');";
                    banco = new Conexao();
                    if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                        Console.WriteLine();
                    }
                    else {
                        Console.WriteLine("Não Existe Cadastro de Aeronaves Ativas\n");
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("\n\t*** Aeronaves Inativas ***\n");
                    this.Situacao = 'I';
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where SITUACAO=('{this.Situacao}');";
                    banco = new Conexao();
                    if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                        Console.WriteLine();
                    }
                    else {
                        Console.WriteLine("Não Existe Cadastro de Aeronaves Inativas\n");
                    }
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("\n\t*** Aeronaves Cadastradas ***\n");
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE";
                    banco = new Conexao();
                    if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                        Console.WriteLine();
                    }
                    else {
                        Console.WriteLine("Não Existe Aeronaves Cadastradas...\n");
                    }
                    break;
            }
            
        }
        #endregion

        #region Menu Aeronave
        public void MenuAeronave(SqlConnection conecta) {
            do {
              
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\t    *** Menu de Aeronave ***");
                Console.WriteLine("\nEscolha a opção desejada:\n" +
                                 "\n[1] Voltar ao Menu anterior" +
                                 "\n[2] Cadastrar" +
                                 "\n[3] Localizar" +
                                 "\n[4] Editar" +
                                 "\n[5] Deletar" +
                                 "\n[6] Consultar Aeronaves" +
                                 "\n[0] Sair");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                while (op < 0 || op > 7) {
                    Console.WriteLine("\nOpção inválida, informe novamente: ");
                    Console.WriteLine("\nEscolha a opção desejada:\n" +
                                      "\n[1] Voltar ao Menu anterior" +
                                      "\n[2] Cadastrar" +
                                      "\n[3] Localizar" +
                                      "\n[4] Editar" +
                                      "\n[5] Deletar " +
                                      "\n[6] Consultar Aeronaves"+
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
                        CadastrarAeronave(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                    case 3:
                        LocalizarAeronave(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                    case 4:
                        EditarAeronave(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                    case 5:
                        DeletarAeronave(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                    case 6:
                        ConsultarAeronave(conecta);
                        Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                        Console.ReadKey();
                        break;
                }
            } while (true);
        }
        #endregion

    }
}
