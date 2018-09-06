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
    public partial class FormServer : Form
    {
        private CConfig config = new CConfig();

        private string sClient;
        private string sIliasUrl;

        public FormServer()
        {
            InitializeComponent();

            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            sClient = config.GetClient();
            sIliasUrl = config.GetServer();

            txtUrl.Text = sIliasUrl;
            txtClient.Text = sClient;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            sIliasUrl = config.FormatIliasUrlToWebServiceLink(txtUrl.Text, ref sClient);
            if (sIliasUrl != "")
            {
                txtClient.Text = sClient;
                btnOK.Enabled = true;

                picCheck.Visible = true;
                picWaiting.Visible = false;
                picFail.Visible = false;
            }
            else
            {
                picCheck.Visible = false;
                picWaiting.Visible = false;
                picFail.Visible = true;

                MessageBox.Show("Link konnte nicht aufgelöst werden. Ist der richtige Link angegeben?", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtClient.Text != "" && txtUrl.Text != "")
            {
                config.SetClient(sClient);
                config.SetServer(sIliasUrl);

                config.SetIliasReference(sIliasUrl);
            }
        }
    }
}
