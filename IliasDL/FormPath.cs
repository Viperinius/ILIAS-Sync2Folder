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
    public partial class FormPath : Form
    {
        private CConfig config = new CConfig();

        public FormPath()
        {
            InitializeComponent();


            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (dialogSavePath.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dialogSavePath.SelectedPath;
                config.SetPath(txtPath.Text);
            }
        }

        private void FormPath_Load(object sender, EventArgs e)
        {
            txtPath.Text = config.GetPath();
        }
    }
}