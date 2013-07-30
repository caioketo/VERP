﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VERP.Classes;
using VERPDatabase;

namespace VERP.Edicao
{
    public partial class FEdicaoCondicaoDePagamento : VERP.FEdicao
    {
        public FEdicaoCondicaoDePagamento()
        {
            InitializeComponent();
            this.Repo = DB.GetInstance().FPRepo;
        }

        private CondicaoDePagamento condicao;

        protected override void Gravar()
        {
            if (estado == Estado.Inserir)
            {
                DB.GetInstance().CondicaoRepo.Inserir(condicao);
            }
            else if (estado == Estado.Modificar)
            {
                DB.GetInstance().CondicaoRepo.Salvar(condicao);
            }

            this.Close();
        }

        protected override void Mapear()
        {
            condicao.Descricao = tbxDescricao.Text;
            condicao.DiasVencimento = Convert.ToInt32(tbxDiasVencimento.Text);
            condicao.Forma = pesFormaDePagamento.Objeto as FormaDePagamento;
        }

        private void MapearTela()
        {
            tbxDescricao.Text = condicao.Descricao;
            tbxDiasVencimento.Text = condicao.DiasVencimento.ToString();
            pesFormaDePagamento.Objeto = condicao.Forma;
            pesFormaDePagamento.Selecionar();
        }

        private void FEdicaoCondicaoDePagamento_Shown(object sender, EventArgs e)
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
                condicao = new CondicaoDePagamento();
                Objeto = condicao;
            }
            else if (estado == Estado.Modificar)
            {
                condicao = Objeto as CondicaoDePagamento;
                MapearTela();
            }
        }
    }
}