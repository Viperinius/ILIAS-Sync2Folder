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
using System.IO;

namespace IliasDL
{
    public partial class FormSettingsImport : Form
    {
        private string sPath;

        public FormSettingsImport()
        {
            InitializeComponent();

            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            sPath = "";

            picCheck.Hide();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (dialogImportPath.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dialogImportPath.SelectedPath;
                sPath = txtPath.Text;
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            picCheck.Hide();

            string sSettingsOrigin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string sSettingsPath = new Uri(Path.GetDirectoryName(sSettingsOrigin)).LocalPath;

            string sSeparator = AddPathSeparator(sPath);
            if (sSeparator != "")
            {
                sPath = sPath + sSeparator;
                sSettingsPath = sSettingsPath + sSeparator;
            }
            else
            {
                MessageBox.Show(@"Fehler im Dateipfad", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(sSettingsPath + "IliasSync2Folder.exe.config") && File.Exists(sPath + "IliasSync2Folder.exe.config"))
            {
                File.Copy(sPath + "IliasSync2Folder.exe.config", sSettingsPath + "IliasSync2Folder.exe.config", overwrite: true);

                picCheck.Show();
            }
            else
            {
                MessageBox.Show(@"Fehler im Dateipfad. Existiert die zu ladende Datei?", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private string AddPathSeparator(string sPath)
        {
            if (sPath.Contains(Path.DirectorySeparatorChar))
            {
                return Path.DirectorySeparatorChar.ToString();
            }
            else if (sPath.Contains(Path.AltDirectorySeparatorChar))
            {
                return Path.AltDirectorySeparatorChar.ToString();
            }
            return "";
        }
    }
}
