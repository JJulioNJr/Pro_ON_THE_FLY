﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Proj_ON_THE_FLY {
    internal class Conexao {


        string Conectar = "Data Source =localhost; Initial Catalog=ONTHEFLAY;User Id=SA; Password=jj;";
        SqlConnection conn;

        public Conexao() { }

        public string Caminho() {
            return Conectar;
        }

        public void InserirDado(SqlConnection conecta, String sql) {
            try {
                conecta.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();
                conecta.Close();

            } catch (SqlException ex) {

                Console.WriteLine(ex.Message);
            }
        }

        public void DeletarDado(SqlConnection conecta, String sql) {
          
            try {
                conecta.Open();
                SqlCommand cmd = new SqlCommand(sql, conecta);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();
                conecta.Close();

            } catch (SqlException ex) {

                Console.WriteLine(ex.Message);
            }
        }

        public void EditarDado(SqlConnection conecta, String sql) {

            try {

                conecta.Open();
            SqlCommand cmd = new SqlCommand(sql, conecta);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();

                conecta.Close();

            } catch (SqlException ex) {

                Console.WriteLine(ex.Message);
            }
        }

        public String LocalizarDado(SqlConnection conecta, String sql,int op) {

            String recebe = "";

            try {

                //conecta.Open();

                SqlCommand cmd = new SqlCommand(sql, conecta);

                SqlDataReader reader = null;

                switch (op) {
                    //Localizar Passageiro
                    case 1:
                        conecta.Open();
                        using (reader = cmd.ExecuteReader()) {
                            Console.WriteLine("\n\t*** Passageiro Localizado ****\n");
                            while (reader.Read()) {
                                recebe = reader.GetString(0);

                                Console.Write(" {0}", reader.GetString(0));
                                Console.Write(" {0}", reader.GetString(1));
                                Console.Write(" {0}", reader.GetDateTime(2).ToShortDateString());
                                Console.Write(" {0}", reader.GetDateTime(3).ToShortDateString());
                                Console.Write(" {0}", reader.GetString(4));
                                Console.Write(" {0}", reader.GetString(5));
                                Console.Write(" {0}", reader.GetDateTime(6).ToShortDateString());
                                //Console.Write(" {0}", reader.GetString(7));

                                Console.WriteLine("\n");
                            }
                        }
                        conecta.Close();
                        break;
                        //Localizar Passageiro Restrito
                        case 2:
                        conecta.Open();
                        using (reader = cmd.ExecuteReader()) {
                            Console.WriteLine("\n\t*** Passageiro Restrito Localizado ****\n");
                            
                            while (reader.Read()) {
                                recebe = reader.GetString(0);
                                Console.Write("CPF: {0}", reader.GetString(0));

                                Console.WriteLine("\n");
                            }
                        }
                        conecta.Close();
                        break;
                        //Localizar Aeronave
                    case 3:
                        conecta.Open();
                        using (reader = cmd.ExecuteReader()) {
                            Console.WriteLine("\n\t*** Aeronave Localizado ****\n");
                            while (reader.Read()) {
                                recebe = reader.GetString(0);

                                Console.Write(" {0}", reader.GetString(0));
                                Console.Write(" {0}", reader.GetString(1));
                                Console.Write(" {0}", reader.GetDateTime(2).ToShortDateString());
                                Console.Write(" {0}", reader.GetString(3));
                                Console.Write(" {0}", reader.GetDateTime(4).ToShortDateString());
                                Console.Write(" {0}", reader.GetInt32(5));
                               

                                Console.WriteLine("\n");
                            }
                        }
                        conecta.Close();
                        break;
                }

            } catch (SqlException ex) {

                Console.WriteLine(ex.Message);
            }

            return recebe;

        }
        public void ConsultaDado(SqlConnection conecta, String sql) {
            try {

                conecta.Open();

                SqlCommand cmd = new SqlCommand(sql, conecta);
                SqlDataReader reader = null;
                using (reader = cmd.ExecuteReader()) {

                   // Console.WriteLine("\n\t*** Passageiros Cadastrados ****\n");
                    while (reader.Read()) {

                         Console.Write(" {0}", reader.GetString(0));
                         Console.Write(" {0}", reader.GetString(1));
                         Console.Write(" {0}", reader.GetDateTime(2).ToShortDateString());
                         Console.Write(" {0}", reader.GetDateTime(3).ToShortDateString());
                         Console.Write(" {0}", reader.GetString(4));
                         Console.Write(" {0}", reader.GetString(5));
                         Console.Write(" {0}", reader.GetDateTime(6).ToShortDateString());
                        //Console.Write(" {0}", reader.GetString(7));

                        Console.WriteLine("\n");
                    }
                }
                conecta.Close();

            } catch (SqlException ex) {

                Console.WriteLine(ex.Message);
            }
        }

    }
}
