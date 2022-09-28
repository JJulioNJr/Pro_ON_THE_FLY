using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class Passageiro {

        public String Nome { get; set; }
        public String Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public DateTime UltimaCompra { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }

        public Conexao banco;

        public Passageiro() { }

        public Passageiro(string nome, string cpf, DateTime dataNascimento, char sexo, DateTime ultimaCompra, DateTime dataCadastro, char situacao) {
            this.Nome = nome;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.UltimaCompra = ultimaCompra;
            this.DataCadastro = dataCadastro;
            this.Situacao = situacao;
        }


        #region Cadastro de Passageiro
        public void CadastrarPassageiro(SqlConnection conecta) {

            Console.WriteLine("\n*** CADASTRO DE PASSAGEIRO ***");
            Console.Write("\nNome: ");
            this.Nome = Console.ReadLine();
            //validação de tamanho
            while (this.Nome.Length > 50) {
                Console.WriteLine("\nDigite um Nome de até 50 digitos!");
                Console.Write("Nome: ");
                this.Nome = Console.ReadLine();
            }
            //validação do tamanho e condição de cpf valido
            Console.Write("\nCPF: ");
            this.Cpf = Console.ReadLine();
            while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                Console.WriteLine("\nCpf invalido, digite novamente");
                Console.Write("CPF: ");
                this.Cpf = Console.ReadLine();
            }

            Console.Write("\nData de Nascimento: ");
            this.DataNascimento = DateTime.Parse(Console.ReadLine());

            Console.Write("\nSexo (M/F/N): ");
            this.Sexo = char.Parse(Console.ReadLine().ToUpper());
            while ((this.Sexo.CompareTo('M') != 0) && (this.Sexo.CompareTo('F') != 0) && (this.Sexo.CompareTo('N') != 0)) {
                Console.WriteLine("\nOpção invalida, digite novamente");
                Console.Write("Sexo (M/F/N): ");
                this.Sexo = char.Parse(Console.ReadLine().ToUpper());
            }

            this.UltimaCompra = DateTime.Now;

            this.DataCadastro = DateTime.Now;

            Console.Write("\nSituação (A-Ativo / I- Inativo): ");
            this.Situacao = char.Parse(Console.ReadLine().ToUpper());
            while ((this.Situacao.CompareTo('A') != 0) && (this.Situacao.CompareTo('I') != 0)) {
                Console.WriteLine("\nOpção invalida, digite novamente");
                Console.Write("Situação(A - Ativo / I - Inativo): ");
                this.Situacao = char.Parse(Console.ReadLine().ToUpper());
            }

            Console.WriteLine("\nDeseja Salvar CPF em Restrito? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int op = int.Parse(Console.ReadLine());

            if (op == 1) {
                string sql = $"Insert into RESTRITO (CPF) Values ('{this.Cpf}');";
                banco = new Conexao();
                banco.InserirDado(conecta, sql);
                Console.WriteLine("\nCadastro de Passageiro Restrito salvo com Sucesso!");

            }

            else {
                Console.WriteLine("\nCadastro de Restrito não foi Acionado... ");

            }

            Console.WriteLine("\nDeseja efetuar a gravação? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                string sql = $"Insert into PASSAGEIRO (CPF, NOME, DATA_NASCIMENTO, DATA_CADASTRO,SEXO,SITUACAO,ULTIMA_COMPRA) Values ('{this.Cpf}' , " +
                     $"'{this.Nome}', '{this.DataNascimento}', '{this.DataCadastro}', '{this.Sexo}', '{this.Situacao}', '{this.UltimaCompra}');";
                banco = new Conexao();
                banco.InserirDado(conecta, sql);
                Console.WriteLine("\nCadastro de Passageiro  Salvo com sucesso!");

            }

            else {
                Console.WriteLine("\nCadastro de Passageiro não foi Acionado...");

            }
        }
        #endregion

        #region Deletar Cadastro de Passageiro
        public void DeletarPassageiro(SqlConnection conecta) {
            Console.WriteLine("\n*** Deletar Passageiro ***");
            Console.Write("\nDigite o CPF: ");
            this.Cpf = Console.ReadLine();
            
            while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                Console.WriteLine("Cpf invalido, digite novamente");
                Console.Write("CPF: ");
                this.Cpf = Console.ReadLine();
            }

            String sql = $"Select CPF,NOME,DATA_NASCIMENTO,DATA_CADASTRO,SEXO,SITUACAO,ULTIMA_COMPRA From PASSAGEIRO Where CPF=('{this.Cpf}');";
            banco = new Conexao();

            Console.Clear();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql,1))) {
                Console.WriteLine("Deseja Deletar Passageiro? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());


                if (op == 1) {

                    sql = $"Delete From PASSAGEIRO Where CPF=('{this.Cpf}');";
                    banco = new Conexao();
                    banco.DeletarDado(conecta, sql);
                    Console.WriteLine("\nCadastro de Passageiro Deletado com sucesso!");

                }
                else {
                    Console.WriteLine("\nDeletar Passageiro Não Foi Acionado ...");

                }
            }
            else {
                Console.WriteLine("\nPassageiro não Encontrado!!!");
            }
        }
        #endregion

        #region Localizar Pasageiro
        public void LocalizarPassageiro(SqlConnection conecta) {
            Console.WriteLine("\n*** Localizar Passageiro Especifico ***");

            Console.WriteLine("\nDeseja Localizar um Passageiro no Cadastro? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Passageiro Especifico ***");
                Console.Write("\nDigite o cpf: ");
                this.Cpf = Console.ReadLine();

                while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                    Console.WriteLine("\nCpf invalido, digite novamente");
                    Console.Write("CPF: ");
                    this.Cpf = Console.ReadLine();
                }

                Console.Clear();
                String sql = $"Select CPF,NOME,DATA_NASCIMENTO,DATA_CADASTRO,SEXO,SITUACAO,ULTIMA_COMPRA From PASSAGEIRO Where CPF=('{this.Cpf}');";
                banco = new Conexao();
                //banco.LocalizarDado(conecta, sql);
                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql,1))) {
                    Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                    Console.ReadKey();
                
                }else{
                    Console.WriteLine("\n\tPassageiro não Encontrado!!!");
                }


            }
            else {
                Console.WriteLine("Localização de Passageiro Não foi Acionada...");

            }

        }
        #endregion

        #region Consultar Passageiro
        public void ConsultarPassageiro(SqlConnection conecta) {

            Console.WriteLine("\nDeseja Consultar Todos Passageiros Cadastrados? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                String sql = $"Select CPF,NOME,DATA_NASCIMENTO,DATA_CADASTRO,SEXO,SITUACAO,ULTIMA_COMPRA From PASSAGEIRO";
                banco = new Conexao();
                Console.WriteLine("\n\t*** Passageiros Cadastrados ****\n");
                banco.ConsultaDado(conecta, sql);

                Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                Console.ReadKey();
            }
            else {
                Console.WriteLine("Consulta de Passageiros Não foi Acionada...");

            }

        }
        #endregion

        #region Editar Passageiro
        public void EditarPassageiro(SqlConnection conecta) {
            int opc = 0;
            String sql = "";

            Console.WriteLine("\n*** Editar Informações do Passageiro ***");
            Console.Write("\nDigite o cpf: ");
            this.Cpf = Console.ReadLine();

            while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                Console.WriteLine("\nCpf invalido, Digite Novamente");
                Console.Write("CPF: ");
                this.Cpf = Console.ReadLine();
            }

            sql = $"Select CPF,NOME,DATA_NASCIMENTO,DATA_CADASTRO,SEXO,SITUACAO,ULTIMA_COMPRA From PASSAGEIRO Where CPF=('{this.Cpf}');";
            banco = new Conexao();
            //banco.LocalizarDado(conecta,sql);
            Console.Clear();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 1))) {
                Console.WriteLine("Deseja Alterar a Informação de algum Campo ? ");
                Console.Write("1- Sim / 2- Não: ");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());

                if (opc == 1) {
                    Console.WriteLine("\nDigite a Opção que Deseja Editar");
                    Console.WriteLine("1-Nome");
                    Console.WriteLine("2-Data de nascimento");
                    Console.WriteLine("3-Sexo (M/F/N)");
                    Console.WriteLine("4-Ultima compra");
                    Console.WriteLine("5-Data cadastro");
                    Console.WriteLine("6-Situação");
                    Console.Write("\nDigite: ");
                    opc = int.Parse(Console.ReadLine());
                    while (opc < 1 || opc > 6) {
                        Console.WriteLine("\nDigite uma Opcao Valida:");
                        Console.Write("\nDigite: ");
                        opc = int.Parse(Console.ReadLine());

                    }

                    switch (opc) {
                        case 1:
                            Console.Write("\nAlterar o Nome: ");
                            this.Nome = Console.ReadLine();
                            sql = $"Update PASSAGEIRO Set NOME=('{this.Nome}') Where CPF=('{this.Cpf}');";
                            Console.WriteLine("\nNome Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 2:
                            Console.Write("\nAlterar a data de Nascimento: ");
                            this.DataNascimento = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("\nData de Nascimento Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            sql = $"Update PASSAGEIRO Set DATA_NASCIMENTO=('{this.DataNascimento}') Where CPF=('{this.Cpf}');";
                            break;
                        case 3:
                            Console.Write("\nAlterar o Sexo (M/F/N): ");
                            this.Sexo = char.Parse(Console.ReadLine());
                            sql = $"Update PASSAGEIRO Set SEXO=('{this.Sexo}') Where CPF=('{this.Cpf}');";
                            Console.WriteLine("\nSexo Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 4:
                            Console.Write("\nAlterar a Ultima Compra: ");
                            this.UltimaCompra = DateTime.Parse(Console.ReadLine());
                            sql = $"Update PASSAGEIRO Set ULTIMA_COMPRA=('{this.UltimaCompra}') Where CPF=('{this.Cpf}');";
                            Console.WriteLine("\nUltima Compra Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 5:
                            Console.Write("\nAlterar a Data de Cadastro: ");
                            this.DataCadastro = DateTime.Parse(Console.ReadLine());
                            sql = $"Update PASSAGEIRO Set DATA_CADASTRO=('{this.DataCadastro}') Where CPF=('{this.Cpf}');";
                            Console.WriteLine("\nData de Cadastro Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 6:
                            Console.WriteLine("\nInforme a Situação: ");
                            this.Situacao = char.Parse(Console.ReadLine());
                            sql = $"Update PASSAGEIRO Set SITUACAO=('{this.Situacao}') Where CPF=('{this.Cpf}');";
                            Console.WriteLine("\nSituacao Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                    }
                    banco = new Conexao();
                    banco.EditarDado(conecta, sql);
                }

                else {
                    Console.WriteLine("\nEditar Cadastrado Não Foi Acionado!");
                }
            }
            else {
                Console.WriteLine("\nPassageiro Não Encontrado");
            }
        }
        #endregion

        //----------------------------------------Restritos--------------------------------------------------------//

        #region Deletar Passageiro Restrito
        public void DeletarRestrito(SqlConnection conecta) {

            Console.WriteLine("\n*** Deletar Passageiro RESTRITO ***");
            Console.Write("\nDigite o CPF: ");
            this.Cpf = Console.ReadLine();
            while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                Console.WriteLine("\nCpf invalido, digite novamente");
                Console.Write("CPF: ");
                this.Cpf = Console.ReadLine();
            }

            String sql = $"Select CPF From RESTRITO Where CPF=('{this.Cpf}');";
            banco = new Conexao();

            Console.Clear();

            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 2))) {
                Console.WriteLine("\nDeseja Deletar Passageiro? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());


                if (op == 1) {
                    sql = $"Delete From RESTRITO Where CPF=('{this.Cpf}');";
                    banco = new Conexao();
                    banco.DeletarDado(conecta, sql);
                    Console.WriteLine("\nPassageiro Restrito Deletado com sucesso!");


                }
                else {
                    Console.WriteLine("\nDeletar Passageiro Não Foi Acionado ...");

                }
            }
            else {
                Console.WriteLine("\nPassageiro não Encontrado!!!");
            }

        }
        #endregion


        #region Localizar Passageiro Restrito
        public void LocalizarRestrito(SqlConnection conecta) {
          

            Console.WriteLine("\nDeseja Localizar um Passageiro Restrito? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Passageiro RESTRITO Especifico ***");
                Console.Write("\nDigite o cpf: ");
                this.Cpf = Console.ReadLine();

                while (ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11) {
                    Console.WriteLine("\nCpf invalido, digite novamente");
                    Console.Write("CPF: ");
                    this.Cpf = Console.ReadLine();
                }
                String sql = $"Select CPF From RESTRITO Where CPF=('{this.Cpf}');";
                banco = new Conexao();
                Console.Clear();
                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 2))) {
                    Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                    Console.ReadKey();

                }
                else {
                    Console.WriteLine("\n\tPassageiro não Encontrado!!!");
                }

               

            }
            else {
                Console.WriteLine("\nLocalização de Passageiro Não foi Acionada...");

            }

        }
        #endregion

        #region Consultar Restrito
        public void ConsultarRestrito(SqlConnection conecta) {

            Console.WriteLine("\nDeseja Consultar Todos Passageiros RESTRITOS Cadastrados? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());

            if (opc == 1) {
                Console.Clear();
                String sql = $"Select CPF From RESTRITO";
                banco = new Conexao();
                banco.LocalizarDado(conecta, sql, 2);

                Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                Console.ReadKey();
            }
            else {
                Console.WriteLine("\nConsulta de Passageiros Não foi Acionada...");

            }

        }
        #endregion

        public bool ValidarCpf(string cpf) {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

    }
}
