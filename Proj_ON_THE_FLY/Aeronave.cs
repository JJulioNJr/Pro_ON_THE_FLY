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
    internal class Aeronave {

        public String Inscricao { get; set; }
        public String Capacidade { get; set; }
        
        public DateTime UltimaVenda = DateTime.Now;
        public DateTime DataCadastro = DateTime.Now;
        public char Situacao { get; set; }

        public Conexao banco;

       // String Companhia_Aerea CNPJ;
         public String  CNPJ { get; set; }

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
        public void CadastroAeronaves(SqlConnection conecta) {

            Console.WriteLine("\n*** Cadastro de Aeronave ***");
            
            //pedir o cnpj procurar na tbela companhia e se tiver agrupar na variavel
            this.CNPJ = "15086511000145";
            //Procurar se existe um cnpj
            this.Inscricao = "PR-" + GeraNumero();
            
            #region Capacidade Aeronave
            int cap = 0;
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
                Capacidade = cap.ToString();

            } while (cap < 100 || cap > 999);
            #endregion
            
            do {
                Console.Write("\nInfome a Situação da Aeronave:" +
                              "\nA-Ativo ou I-Inativo\n" +
                              "\nSituação: ");
                Situacao = char.Parse(Console.ReadLine().ToUpper());

            } while (!Situacao.Equals('A') && !Situacao.Equals('I'));

            UltimaVenda = DateTime.Now;
            DataCadastro = DateTime.Now;
            
            Console.WriteLine("\nDeseja Salvar o Cadastrado de Aeronave? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int op = int.Parse(Console.ReadLine());
            
          
            if (op == 1) {
                String sql = $"Insert Into Aeronave(ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE) " +
                             $"Values ('{this.Inscricao}','{this.CNPJ}','{this.DataCadastro}','{this.Situacao}','{this.UltimaVenda}','{this.Capacidade}');";
                banco = new Conexao();
                banco.InserirDado(conecta, sql);
                Console.WriteLine("\nCadastro de Aeronave Salvo com Sucesso!");
            
            } else {
                Console.WriteLine("\nCadastro de Aeronave não Foi Acionado... ");

            }
        }
        #endregion

        #region Localizar Aeronave
        public void LocalizarAeronave(SqlConnection conecta) {

            Console.WriteLine("\n*** Localizar Aeronave Especifico ***");

            Console.WriteLine("\nDeseja Localizar uma Aeronave no Cadastro? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Aeronave Especifico ***");
                Console.Write("\n Digite a ID_ANAC: ");
                this.Inscricao = Console.ReadLine();

                while (this.Inscricao.Length < 6) {
                    Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                    Console.Write("ID_ANAC: ");
                    this.Inscricao = Console.ReadLine();
                }

                //Ver se existe Companhia Aeronave cadastrada, perguntar pro usuario o CNPJ manda pro banco via select e  localizar e se tiver deletar
                this.CNPJ = "15086511000145";

               
                String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
              //  String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE";
                banco=new Conexao();
                Console.Clear();
                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                    Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                    Console.ReadKey();

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

            Console.WriteLine("\n*** Deletar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao =Console.ReadLine().ToUpper();

            while (this.Inscricao.Length < 6) {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }

           

            Console.Clear();
            // String sql = $"Select ID_ANAC,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
            String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}');";
            banco = new Conexao();
            //switch
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                Console.WriteLine("Deseja Deletar Aeronave? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());

                if (op == 1) {

                    //Ver se existe Companhia Aeronave cadastrada, perguntar pro usuario o CNPJ manda pro banco via select e  localizar e se tiver deletar
                  // USei Para testa
                     this.CNPJ = "15086511000145";

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


            Console.WriteLine("\n*** Editar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao = Console.ReadLine().ToUpper();

            while (this.Inscricao.Length < 6) {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }

            //pedir o cnpj 
            //this.CNPJ = "15086511000145";

            Console.Clear();
            String sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}');";
            banco = new Conexao();

            //switch
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3))) {
                Console.WriteLine("\nDeseja Alterar a Informação de algum Campo ? ");
                Console.Write("1- Sim / 2- Não: ");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());


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
                            this.Situacao = char.Parse(Console.ReadLine());
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
                    Console.Write("\n*** Aeronaves Ativas ***\n");
                    this.Situacao = 'A';
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where SITUACAO=('{this.Situacao}');";
                    banco = new Conexao();
                    banco.LocalizarDado(conecta,sql,3);
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("\n*** Aeronaves Ativas ***\n");
                    this.Situacao = 'I';
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where SITUACA)=('{this.Situacao}');";
                    banco = new Conexao();
                    banco.LocalizarDado(conecta, sql,3);
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("\n*** Aeronaves Cadastradas ***\n");
                    sql = $"Select ID_ANAC,CNPJ,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE ";
                    banco = new Conexao();
                    banco.LocalizarDado(conecta, sql,3);
                    break;
            }
            
        }
        #endregion
    }
}
