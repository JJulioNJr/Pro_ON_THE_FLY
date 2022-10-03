using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class CompanhiaAerea {

        public String Cnpj { get; set; }
        public String RazaoSocial { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime UltimoVoo { get; set; }
        public char Situacao { get; set; }

        public Conexao banco;
        public CompanhiaAerea(String cnpj, String razaoSocial, DateTime dataAbertura, DateTime ultimoVoo, DateTime dataCadastro, char situacao) {
            this.Cnpj = cnpj;
            this.RazaoSocial = razaoSocial;
            this.DataAbertura = dataAbertura;
            this.DataCadastro = dataCadastro;
            this.UltimoVoo = ultimoVoo;
            this.Situacao = situacao;
        }
        public CompanhiaAerea() {

        }
        #region Cadastro de Companhia Aerea
        public void CadastrarCia(SqlConnection conecta) {
         
            this.UltimoVoo = DateTime.Now;
            this.DataCadastro = DateTime.Now;

            Console.Clear();
            Console.WriteLine("*** Cadastro de Companhia Aérea ***\n\n");
            Console.Write("Digite o CNPJ : ");
            this.Cnpj = Console.ReadLine();
            if (ValidarCnpj(this.Cnpj)) {
                Console.Write("Digite a Data de abertura da Companhia: ");
                this.DataAbertura = DateTime.Parse(Console.ReadLine());
                System.TimeSpan tempoAbertura = DateTime.Now.Subtract(this.DataAbertura);
                if (tempoAbertura.TotalDays > 190) {
                    do {
                        Console.Write("Digite a Razão Social (até 50 dígitos) : ");
                        this.RazaoSocial = Console.ReadLine();

                    } while (this.RazaoSocial.Length > 50);


                    Console.Write("\nSituação (A-Ativo / I- Inativo): ");
                    this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                    while ((this.Situacao.CompareTo('A') != 0) && (this.Situacao.CompareTo('I') != 0)) {
                        Console.WriteLine("\nOpção invalida, digite novamente");
                        Console.Write("Situação(A - Ativo / I - Inativo): ");
                        this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                    }

                    String sql = $"Insert Into COMPANHIA_AEREA(CNPJ,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO,RAZAO_SOCIAL) Values('{this.Cnpj}','{this.DataAbertura}','{this.UltimoVoo}'," +
                                 $"'{this.Situacao}','{this.DataCadastro}','{this.RazaoSocial}');";
                    banco = new Conexao();
                    banco.InserirDado(conecta, sql);
                    Console.WriteLine("\nCompanhia Aérea Cadastrada com sucesso!\n\nAperte enter para continuar.");
                    Console.WriteLine("\nPressione uma Tecla Para Continuar...");
                    Console.ReadKey();

                }
                else {
                    Console.WriteLine("Impossível Cadastrar! Tempo de Abertura de Empresa menor que 6 meses!\n\nTecle enter para continuar...");
                    Console.ReadKey();
                }
            }
            else {
                Console.WriteLine("CNPJ inválido! Tente novamente.Aperte enter para continuar.");
                Console.ReadKey();
                CadastrarCia(conecta);
            }
        }
        #endregion

        #region Localizar Companhia
        public void LocalizarCia(SqlConnection conecta) {

            do {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Companhia Aérea: ***\n\n");
                Console.Write("Digite o CNPJ: ");
                this.Cnpj = Console.ReadLine();
            } while (ValidarCnpj(this.Cnpj) == false || this.Cnpj.Length < 14);
            Console.Clear();
            String sql = $"SELECT CNPJ,RAZAO_SOCIAL,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO FROM COMPANHIA_AEREA WHERE CNPJ = '{this.Cnpj}';";
            banco = new Conexao();
            Console.WriteLine("\n***Companhia Aérea Especifica Localizada ***\n\n");
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 4))) {
                Console.WriteLine("Aperte enter para contninuar.");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
            else {
                Console.WriteLine("\nCompanhia Aerea Não Encontrada...");
                Console.WriteLine("\nAperte enter para contninuar.");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
        }
        #endregion

        #region Consultar Todas Companhias
        public void ConsultarCia(SqlConnection conecta) {

            Console.Clear();
            Console.WriteLine("\n*** Todas Companhias Aéreas Cadastradas: ***\n\n");
              
            String sql = $"SELECT CNPJ,RAZAO_SOCIAL,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO FROM COMPANHIA_AEREA";
            banco = new Conexao();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 4))) {
                Console.WriteLine("Aperte enter para contninuar.");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
            else {
                Console.WriteLine("\nCompanhia Aerea Não Encontrada...");
                Console.WriteLine("\nAperte enter para contninuar.");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
        }
        #endregion

        #region Editar Companhia
        public void EditarCia(SqlConnection conecta) {
            int opc = 0;
            String sql = "";

            Console.Clear();
            Console.WriteLine("*** Editar Campos de Companhia Aérea ***\n");
            Console.Write("Localizar Cia Aerea Informe o CNPJ: ");
            this.Cnpj = Console.ReadLine();
            while (ValidarCnpj(this.Cnpj) == false || this.Cnpj.Length < 14) {
                Console.WriteLine("\nCNPJ Invalido, Digite Novamente");
                Console.Write("\nLocalizar Cia Aerea Informe o CNPJ: ");
                this.Cnpj = Console.ReadLine();
            }

            sql = $"SELECT CNPJ,RAZAO_SOCIAL,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO FROM COMPANHIA_AEREA WHERE CNPJ = '{this.Cnpj}';";
            banco = new Conexao();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 4))) {
                Console.WriteLine("Digite a opção que deseja editar:\n\n1-Razão Social\n2-Data de Abertura\n3-Data de Cadastro\n4-Data do Último Vôo\n5-Situação");
                opc = int.Parse(Console.ReadLine());
                switch (opc) {
                    case 1:
                        Console.Clear();
                        Console.Write("Alterar Razão Social:\n");
                        Console.Write("Nova Razão Social: ");
                        this.RazaoSocial = Console.ReadLine();
                        sql = $"UPDATE COMPANHIA_AEREA SET RAZAO_SOCIAL = '{this.RazaoSocial}' WHERE CNPJ = '{this.Cnpj}';";
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Alterar Data de Abertura:\n");
                        Console.Write("Nova Data de Abertura: ");
                        this.DataAbertura = DateTime.Parse(Console.ReadLine());
                        sql = $"UPDATE COMPANHIA_AEREA  SET DATA_ABERTURA = '{this.DataAbertura}' WHERE CNPJ = '{this.Cnpj}';";
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("Alterar Data de Cadastro:\n");
                        Console.Write("Nova data de Cadastro: ");
                        this.DataCadastro = DateTime.Parse(Console.ReadLine());
                        sql = $"UPDATE COMPANHIA_AEREA  SET DATA_CADASTRO = '{this.DataCadastro}' WHERE CNPJ = '{this.Cnpj}';";
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("Alterar Data do Último Vôo:\n");
                        Console.Write("Nova Data do Ultimo voo: ");
                        this.UltimoVoo = DateTime.Parse(Console.ReadLine());
                        sql = $"UPDATE COMPANHIA_AEREA  SET ULTIMO_VOO = '{this.UltimoVoo}' WHERE CNPJ = '{this.Cnpj}';";
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write("\nSituação (A-Ativo / I- Inativo): ");
                        Console.Write("\nNova Situação: ");
                        this.Situacao = char.Parse(Console.ReadLine().ToUpper());

                        while ((this.Situacao.CompareTo('A') != 0) && (this.Situacao.CompareTo('I') != 0)) {
                            Console.WriteLine("\nOpção Invalida, Digite Novamente");
                            Console.Write("Situação(A - Ativo / I - Inativo): ");
                            this.Situacao = char.Parse(Console.ReadLine().ToUpper());
                        }

                        sql = $"UPDATE COMPANHIA_AEREA  SET SITUACAO = '{this.Situacao}' WHERE CNPJ = '{this.Cnpj}';";
                        break;
                    default:
                        Console.WriteLine("Opção não encontrada!");
                        break;
                }
                banco = new Conexao();
                banco.EditarDado(conecta, sql);
                Console.WriteLine("\nAlteração realizada com sucesso!\n\nAperte enter para continuar.");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
            else {
                Console.WriteLine("\nCompanhia Aerea Não Encontrada");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }
        }

        #endregion

        #region Deletar Comapanhia
        public void DeletarCia(SqlConnection conecta) {
            String sql = "";
            Console.Clear();
            Console.WriteLine("\n*** Deletar Companhia Aérea ***");
            Console.Write("\nDigite o CNPJ que deseja excluir: ");
            this.Cnpj = Console.ReadLine();

            while (ValidarCnpj(this.Cnpj) == false || this.Cnpj.Length < 14) {
                Console.Clear();
                Console.WriteLine("\n*** Deletar Companhia Aérea ***");
                Console.WriteLine("\nCNPJ Invalido, Digite Novamente");
                Console.Write("\nLocalizar Cia Aerea Informe o CNPJ: ");
                this.Cnpj = Console.ReadLine();
            }

            sql = $"SELECT CNPJ,RAZAO_SOCIAL,DATA_ABERTURA,ULTIMO_VOO,SITUACAO,DATA_CADASTRO FROM COMPANHIA_AEREA WHERE CNPJ = '{this.Cnpj}';";
            banco = new Conexao();
            Console.WriteLine("\n***Companhia Aérea Especifica Localizada ***\n\n");
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 4))) {

                Console.WriteLine("Deseja Deletar Companhia Aerea? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                if (op == 1) {
                    sql = $"Delete From COMPANHIA_AEREA Where CNPJ=('{this.Cnpj}');";
                    banco = new Conexao();
                    banco.DeletarDado(conecta, sql);
                    Console.WriteLine("\nCompanhia Aerea Deletada com sucesso!\n\nAperte enter para continuar.");
                    Console.ReadKey();
                    MenuCiaAerea(conecta);

                }
                else {
                    Console.WriteLine("\nDeletar Companhia Aerea Não Foi Acionado ...");
                }
            }
            else {
                Console.Clear();
                Console.WriteLine("\nCompanhia Aerea Não Encontrada\n\nAperte enter para continuar");
                Console.ReadKey();
                MenuCiaAerea(conecta);
            }

        }
        #endregion

        #region Menu Companhia
        public void MenuCiaAerea(SqlConnection conexaosql) {
            int op;
            do {
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\n\t   *** Menu Companhias Aereas ***\n\n");
                Console.WriteLine("\nEscolha a opção desejada:\n" +
                                 "\n[1] Voltar ao Menu" +
                                 "\n[2] Cadastrar" +
                                 "\n[3] Localizar" +
                                 "\n[4] Editar" +
                                 "\n[5] Consultar Todas Cias"+
                                 "\n[6] Deletar Cia "+
                                 "\n[7] Cia Bloqueadas");
                Console.Write("\nDigite: ");
                op = int.Parse(Console.ReadLine());
                while (op < 0 && op > 6) {
                    Console.WriteLine("\nOpção inválida escolhida, informe novamente: ");
                    Console.WriteLine("\n\nEscolha a opção desejada:\n" +
                                 "\n[1] Voltar ao Menu" +
                                 "\n[2] Cadastrar" +
                                 "\n[3] Localizar" +
                                 "\n[4] Editar" +
                                 "\n[5] Consultar Todas Cias" +
                                 "\n[6] Deletar Cia"+
                                 "\n[7] Cia Bloqueadas");
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
                        CadastrarCia(conexaosql);
                        break;
                    case 3:
                        LocalizarCia(conexaosql);
                        break;
                    case 4:
                        EditarCia(conexaosql);
                        break;
                    case 5:
                        ConsultarCia(conexaosql);
                        break;
                    case 6:
                        DeletarCia(conexaosql);
                        break;
                    case 7:
                        MenuBloqueadas(conexaosql);
                        break;
                    
                }
            } while (op > 0 && op < 6);
        }
        #endregion

        //___________________________________Bloqueados________________________________________________________//

        #region Menu de Cia Bloqueadas
        private void MenuBloqueadas(SqlConnection conexaosql) {
            int op;

            do {
                Console.Clear();
                Console.WriteLine("\n\t\t*** ON THE FLY ***");
                Console.WriteLine("\n\n\t   *** Companhias Bloqueadas ***\n\n");
                Console.WriteLine("Escolha a opção desejada: \n" +
                                  "\n1- Consultar Companhia Bloqueadas" +
                                  "\n2- Bloquear Companhias" +
                                  "\n3- Desbloquear Companhias" +
                                  "\n0- Sair ");
                Console.Write("\nDigite: ");
                op = int.Parse(Console.ReadLine());

                switch (op) {
                    case 0:
                        MenuCiaAerea(conexaosql);
                        break;
                    case 1:
                        LocalizarBloqueada(conexaosql);
                        break;
                    case 2:
                        CadastrarBloqueadas(conexaosql);
                        break;
                    case 3:
                        DeletarBloqueadas(conexaosql);
                        break;
                }
            } while (op > 0 && op < 4);
        }
        #endregion


        #region CRD Bloqueadas
        public void CadastrarBloqueadas(SqlConnection conexaosql) {
            Console.Clear();
            Console.WriteLine(">>> Cadastro de Companhia Aérea Bloqueada<<<\n\n");
            Console.WriteLine("Digite o CNPJ da Companhia Aérea que deseja bloquear: ");
            this.Cnpj = Console.ReadLine();
            if (ValidarCnpj(this.Cnpj)) {
                string sql = $"INSERT INTO BLOQUEADO(Cnpj) VALUES ('{this.Cnpj}');";
                banco = new Conexao();
                banco.InserirDado(conexaosql, sql);
                Console.WriteLine("CNPJ Cadastrado com sucesso!Aperte enter para continuar.");
                Console.ReadKey();
            }
            else {
                Console.WriteLine("CNPJ inválido! Tente novamente.Aperte enter para continuar.");
                Console.ReadKey();
            }
        }
        public void DeletarBloqueadas(SqlConnection conexaosql) {
            Console.Clear();
            Console.WriteLine("\n*** Deletar Companhia Aérea ***\n\n");
            Console.Write("\nDigite o CNPJ que deseja excluir: ");
            this.Cnpj = Console.ReadLine();

            if (ValidarCnpj(this.Cnpj)) {
                string sql = $"Delete From BLOQUEADO Where Cnpj=('{this.Cnpj}');";
                banco = new Conexao();
                banco.DeletarDado(conexaosql, sql);
                Console.WriteLine("\n\nCNPJ excluido do cadastro de bloqueados!\nAperte enter para continuar...");
                Console.ReadKey();
                MenuCiaAerea(conexaosql);
            }
            else {
                Console.WriteLine("\n\nCNPJ inválido! Tente novamente.\nAperte enter para continuar.");
                Console.ReadKey();
                MenuCiaAerea(conexaosql);
            }
        }
        public void LocalizarBloqueada(SqlConnection conecta) {
            do {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Companhia Aérea Bloqueada: ***\n\n");
                Console.Write("Digite o CNPJ: ");
                this.Cnpj = Console.ReadLine();
            } while (ValidarCnpj(this.Cnpj) == false || this.Cnpj.Length < 14);
            Console.Clear();
            string sql = $"SELECT Cnpj FROM BLOQUEADO WHERE CNPJ = '{this.Cnpj}';";
            banco = new Conexao();
            Console.WriteLine("\nCNPJ Companhias Bloqueadas\n");
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 6))) {
                Console.WriteLine("Aperte enter para sair.");
                Console.ReadKey();
              
            }
            else {
                Console.WriteLine("\nEste CNPJ nao esta bloqueado!! ");
              
            }

        }
        #endregion

        #region validados de CNPJ
        public bool ValidarCnpj(string cnpj) {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;


            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else

                resto = 11 - resto;

            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        #endregion



    }
}
