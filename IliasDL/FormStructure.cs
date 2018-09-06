//==================================================================
//
//    (c) Copyright by Viperinius
//
//==================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IliasDL
{
    public partial class FormStructure : Form
    {
        CConfig config = new CConfig();

        public FormStructure()
        {
            InitializeComponent();
        }

        private void FormStructure_Load(object sender, EventArgs e)
        {
            if (config.GetStructureTemplate() != "__NO_VAL__")
            {
                txtTemplate.Text = config.GetStructureTemplate();
            }

            if (config.GetUseYearInStructure() == "true")
            {
                chkYear.Checked = true;
            }
            else
            {
                chkYear.Checked = false;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            config.SetStructureTemplate(txtTemplate.Text);
        }

        private void ChkYear_CheckedChanged(object sender, EventArgs e)
        {
            if (chkYear.Checked)
            {
                config.SetUseYearInStructure(true);
            }
            else
            {
                config.SetUseYearInStructure(false);
            }
        }
    }
}
