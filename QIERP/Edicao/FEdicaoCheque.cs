﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QIERP.Classes;
using QIERPDatabase;
using QIERPDatabase.Classes;

namespace QIERP.Edicao
{
    public partial class FEdicaoCheque : QIERP.FEdicao
    {
        public FEdicaoCheque()
        {
            InitializeComponent();
        }

        private Cheque cheque;

        protected override void Gravar()
        {
            if (estado == Estado.Inserir)
            {
                DB.GetInstance().ChequeRepo.Inserir(cheque);
            }
            else if (estado == Estado.Modificar)
            {
                DB.GetInstance().ChequeRepo.Salvar(cheque);
            }

            this.Close();
        }

        protected override void Mapear()
        {
            cheque.Agencia = tbxAgencia.Text;
            cheque.Banco = tbxBanco.Text;
            cheque.Conta = tbxConta.Text;
            cheque.Emissor = tbxEmissor.Text;
            cheque.Numero = tbxNumero.Text;
            cheque.Telefone = tbxTelefone.Text;
            cheque.Vencimento = dtpVencimento.Value;
            if (tbxValor.Text.Contains("R$"))
            {
                cheque.Valor = Convert.ToDecimal(tbxValor.Text.Substring(3));
            }
            else
            {
                cheque.Valor = Convert.ToDecimal(tbxValor.Text);
            }
        }

        private void MapearTela()
        {
            tbxAgencia.Text = cheque.Agencia;
            tbxBanco.Text = cheque.Banco;
            tbxConta.Text = cheque.Conta;
            tbxEmissor.Text = cheque.Emissor;
            tbxNumero.Text = cheque.Numero;
            tbxTelefone.Text = cheque.Telefone;
            dtpVencimento.Value = cheque.Vencimento;
            tbxValor.Text = cheque.Valor.ToString("c");
        }

        private void FEdicaoCheque_Shown(object sender, EventArgs e)
        {
            foreach (Control ctr in Controls)
            {
                if (ctr is TextBox)
                {
                    ((TextBox)ctr).Clear();
                }
            }
            if (estado == Estado.Inserir)
            {
                cheque = new Cheque();
                Objeto = cheque;
            }
            else if (estado == Estado.Modificar)
            {
                cheque = Objeto as Cheque;
                MapearTela();
            }
            tbxBanco.Focus();
        }

        private void FEdicaoCheque_Load(object sender, EventArgs e)
        {
            tbxValor.Validating += tbx_Leave;
        }

    }
}
