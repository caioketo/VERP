﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIERPDatabase.Classes;
using QIERPDatabase.Migrations;
using System.Data.Entity.Infrastructure;

namespace QIERPDatabase
{
    public class QIERPContext : DbContext
    {
        public static string CreateConnectionString()
        {
            return "Server=127.0.0.1\\SQLSERVER;Database=qierp;User Id=sa;Password=vd001989;";
        }
        private string connectionString;
        public QIERPContext() : base() { }
        public QIERPContext(string nameOrConnectionString) : base(nameOrConnectionString) 
        {
            connectionString = nameOrConnectionString;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<FormaDePagamento> FormasDePagamento { get; set; }
        public DbSet<CondicaoDePagamento> CondicoesDePagamento { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<UF> UFs { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<ContaReceber> CRs { get; set; }
        public DbSet<ContaPagar> CPs { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<NotaFiscal> Notas { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<QIERPContext>(new DropCreateDatabaseIfModelChanges<QIERPContext>());
            Database.DefaultConnectionFactory = new SqlConnectionFactory("System.Data.SqlServer");
            base.OnModelCreating(modelBuilder);
        }

        public int GetSequence(string sequence)
        {
            try
            {
                return Database.SqlQuery<int>("select Next value for " + sequence).FirstOrDefault();
            }
            catch
            {
                Database.ExecuteSqlCommand("CREATE SEQUENCE " + sequence + " AS [int] START WITH 1 INCREMENT BY 1 MINVALUE 1");
                return Database.SqlQuery<int>("select Next value for " + sequence).FirstOrDefault();
            }
            
        }

        public void LoadAll()
        {
            Produtos.Load();
            FormasDePagamento.Load();
            CondicoesDePagamento.Load();
            Items.Load();
            Pagamentos.Load();
            Vendas.Load();
            Movimentacoes.Load();
            Pessoas.Load();
            Clientes.Load();
            Telefones.Load();
            Enderecos.Load();
            Cidades.Load();
            UFs.Load();
            Fornecedores.Load();
            Vendedores.Load();
            CRs.Load();
            CPs.Load();
            Cheques.Load();
            Notas.Load();
            Orcamentos.Load();
        }
    }
}
