﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VERPDatabase;
using VERP.Utils;
using VERP.CRUD;
using VERPDatabase.Classes;

namespace VERP
{

    public partial class FFechaVenda : BaseForm
    {
        public Venda VendaAtual;
        public FormaDePagamento FP = null;
        public FVenda fVenda;

        public FFechaVenda()
        {
            InitializeComponent();
        }

        private void FFechaVenda_Shown(object sender, EventArgs e)
        {
            VendaAtual.Cliente = new Cliente();
            VendaAtual.Vendedor = new Vendedor();
            pesCliente.CRUD = new FCRUDPessoa();
            pesCliente.Campo = "Nome";
            pesCliente.CampoDisplay = "Nome";
            pesCliente.Titulo = "Cliente";
            pesCliente.Repo = DB.GetInstance().PessoaRepo;
            pesCliente.Objeto = VendaAtual.Cliente;
            pesCliente.Filter = "Clientes";
            pesCliente.onLoad();
            pesVendedor.CRUD = new FCRUDPessoa();
            pesVendedor.Campo = "Nome";
            pesVendedor.CampoDisplay = "Nome";
            pesVendedor.Titulo = "Vendedor";
            pesVendedor.Repo = DB.GetInstance().PessoaRepo;
            pesVendedor.Objeto = VendaAtual.Vendedor;
            pesVendedor.onLoad();

            CalculaPagto();
            pagamentoBindingSource.DataSource = VendaAtual.Pagamentos;
            formaDePagamentoBindingSource.DataSource = DB.GetInstance().FPRepo.GetAll();
            if (FP != null)
            {
                cmbForma.SelectedItem = FP;
                cmbCondicao.Focus();
            }
            AtualizaCondicoes();
        }

        public void AtualizaCondicoes()
        {
            try
            {
                FormaDePagamento fp = (FormaDePagamento)cmbForma.SelectedItem;
                if (fp != null)
                {
                    condicaoDePagamentoBindingSource.DataSource = DB.GetInstance().CPRepo.GetAll().Where(c => c.Forma.Id == fp.Id).ToList();
                }
            }
            catch
            {
                return;
            }
        }

        private void CalculaPagto()
        {
            rtbTotal.Clear();
            rtbTotal.AppendText("Total Venda: " + VendaAtual.Total.ToString("C2") + Environment.NewLine, Color.Black);
            rtbTotal.AppendText("Total Pago: " + VendaAtual.TotalPagto.ToString("C2") + Environment.NewLine, Color.Black);
            if (VendaAtual.Troco > 0)
            {
                rtbTotal.AppendText("Troco: " + VendaAtual.Troco.ToString("C2"), Color.Black);
            }
            else if (VendaAtual.Troco < 0)
            {
                rtbTotal.AppendText("Falta: " + (VendaAtual.Troco * -1).ToString("C2"), Color.Red);
            }
        }

        private string VerifyNumeric(string text)
        {
            double value = 0;
            double.TryParse(text, out value);
            return value.ToString("C2"); 
        }

        private void btnFinalizaVenda_Click(object sender, EventArgs e)
        {
            if (VendaAtual.Troco < 0)
            {
                return;
            }

            if (!DB.GetInstance().VendaRepo.Inserir(VendaAtual))
            {
                Mensagem.MostrarMsg(40002, DB.GetInstance().Error);
            }
            else
            {
                fVenda.VendaAtual = null;
                Mensagem.MostrarMsg(10000);
                this.Close();
            }
        }

        private void FFechaVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double valor = 0;
            if (tbxValor.Text.Trim() == "")
            {
                tbxValor.Text = "0";
            }

            if (tbxValor.Text.Contains("R$"))
            {
                valor = Double.Parse(tbxValor.Text.Substring(2));
                tbxValor.Text = VerifyNumeric(tbxValor.Text.Substring(2));
            }
            else
            {
                valor = Double.Parse(tbxValor.Text);
                tbxValor.Text = VerifyNumeric(tbxValor.Text);
            }

            if (valor <= 0)
            {
                Mensagem.MostrarMsg(40003);
                tbxValor.Focus();
                return;
            }

            FormaDePagamento fp = DB.GetInstance().FPRepo.GetById((int)cmbForma.SelectedValue);

            if (fp == null)
            {
                Mensagem.MostrarMsg(40000, "Forma de Pagamento");
                cmbForma.Focus();
                return;
            }

            CondicaoDePagamento cp = DB.GetInstance().CPRepo.GetById((int)cmbCondicao.SelectedValue);

            if (cp == null)
            {
                Mensagem.MostrarMsg(40000, "Condição de Pagamento");
                cmbCondicao.Focus();
                return;
            }

            Pagamento pagto = new Pagamento();
            pagto.Condicao = cp;
            pagto.Forma = fp;
            pagto.Valor = valor;

            VendaAtual.Pagamentos.Add(pagto);
            CalculaPagto();
        }

        private void cmbForma_SelectedValueChanged(object sender, EventArgs e)
        {
            AtualizaCondicoes();
        }

        private void tbxValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !e.KeyChar.Equals(','))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
