﻿using Microsoft.Reporting.WinForms;
using QIERPDatabase;
using QIERPDatabase.Classes.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QIERP.Relatorios
{
    public partial class RPOrcamento : Form
    {
        public int OrcamentoId;

        public RPOrcamento()
        {
            InitializeComponent();
        }

        private void RPOrcamento_Load(object sender, EventArgs e)
        {
            BindingList<DTO_Orcamento> bind = new BindingList<DTO_Orcamento>();
            bind.Add(DB.GetInstance().OrcamentoRepo.GetDTOById(OrcamentoId));
            DTO_OrcamentoBindingSource.DataSource = bind;

            reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            this.reportViewer1.RefreshReport();
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("OrcamentoDetail", DB.GetInstance().OrcamentoRepo.GetDTOById(OrcamentoId).Itens));
        }
    }
}
