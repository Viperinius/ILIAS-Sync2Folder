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
    public partial class FormNames : Form
    {
        private CConfig config = new CConfig();
        private readonly Form1 _form1;

        private BackgroundWorker worker;

        private int iPercentage;
        private bool bCoursesDone = false;
        private bool bCancelled = false;

        List<string> lCourses = new List<string>();
        List<string> lCourseNames = new List<string>();

        public FormNames(Form1 form)
        {
            InitializeComponent();

            _form1 = form;

            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void FormNames_Load(object sender, EventArgs e)
        {
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanges);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);

            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
            else
            {
                if (progBar.Value == progBar.Maximum)
                {
                    progBar.Value = progBar.Minimum;
                }
                worker.RunWorkerAsync(progBar.Value);
            }
        }

        private void FormNames_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker.IsBusy)
            {
                bCancelled = true;
                worker.CancelAsync();
                e.Cancel = true;
                this.Enabled = false;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (listViewCourses.SelectedItems.Count > 0)
            {
                var item = listViewCourses.SelectedItems[0];
                txtCourseIlias.Text = item.SubItems[0].Text;
                txtCourseOwn.Text = item.SubItems[1].Text;
            }
            else
            {
                txtCourseIlias.Text = "Bitte erst eine Zeile auswählen.";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtCourseIlias.Text != "Bitte erst eine Zeile auswählen.")
            {
                string sOriginal = txtCourseIlias.Text;
                string sOwn = txtCourseOwn.Text;

                var item = listViewCourses.FindItemWithText(sOriginal);
                ChangeListViewItem(item.Index, "Eigener Name", sOwn);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            foreach (string sRef in lCourses)
            {
                config.ClearCourseName(sRef);
            }

            foreach (ListViewItem item in listViewCourses.Items)
            {
                if (item.SubItems[1].Text != "")
                {
                    config.SetCourseName(item.SubItems[2].Text, item.SubItems[1].Text);
                }
            }
        }


        private void PrintCourseNames()
        {
            lCourses = _form1.lCourses;
            lCourseNames = _form1.lCourseNames;

            string sOwnName = "";

            for (int i = 0; i < lCourses.Count; i++)
            {
                if (config.GetUseOwnNames() == "false")
                {
                    AddToListView(lCourseNames[i], "", lCourses[i]);
                }
                else if (config.GetUseOwnNames() == "true")
                {
                    sOwnName = config.GetCourseName(lCourses[i]);
                    if (sOwnName != "__NO_VAL__")
                    {
                        AddToListView(lCourseNames[i], sOwnName, lCourses[i]);
                    }
                    else
                    {
                        AddToListView(lCourseNames[i], "", lCourses[i]);
                    }
                }

            }
        }

        private void AddToListView(string sCourse, string sOwnName, string sRefId)
        {
            string[] row = { sCourse, sOwnName, sRefId };
            if (listViewCourses.InvokeRequired)
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewCourses.Invoke(new AddToListViewCallback(AddToListView), new object[] {
                    sCourse,
                    sOwnName,
                    sRefId
                });
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewCourses.Items.Add(listViewItem);
            }

        }
        public delegate void AddToListViewCallback(string sCourse, string sOwnName, string sRefId);

        private void ClearListView()
        {
            if (listViewCourses.InvokeRequired)
            {
                listViewCourses.Invoke(new ClearListViewCallback(ClearListView), new object[] { });
            }
            else
            {
                listViewCourses.Items.Clear();
            }
        }
        public delegate void ClearListViewCallback();

        private void ChangeListViewItem(int iRow, string sCategory, string sNewVal)
        {
            int iColumn = 3;
            switch (sCategory)
            {
                case "Kurs":
                    iColumn = 0;
                    break;
                case "Eigener Name":
                    iColumn = 1;
                    break;
                case "Ref ID":
                    iColumn = 2;
                    break;
                default:
                    Console.WriteLine("====ERROR WHEN CHANGING LIST VIEW ITEM===");
                    break;
            }

            if (listViewCourses.InvokeRequired)
            {
                listViewCourses.Invoke(new ChangeListViewItemCallback(ChangeListViewItem), new object[]
                {
                    iRow,
                    sCategory,
                    sNewVal
                });
            }
            else
            {
                ListViewItem item = listViewCourses.Items[iRow];

                item.SubItems[iColumn].Text = sNewVal;
            }
        }
        public delegate void ChangeListViewItemCallback(int iRow, string sCategory, string sNewVal);

        //-------------------------------------------------------------------background worker

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //clear old list view data
            ClearListView();
            //reset progress
            iPercentage = 0;


            while (!worker.CancellationPending && !bCoursesDone)
            {
                worker.ReportProgress(iPercentage);

                iPercentage = 6;   //random value to jump to (to keep users patient)
                worker.ReportProgress(iPercentage);

                if (!_form1.lCourses.Any())
                {
                    _form1.GetCourses(ref _form1.client, sSessionId: _form1.sSessionId);
                }
                if (worker.CancellationPending) return;

                if (!_form1.lCourseNames.Any())
                {
                    _form1.GetCourseNames(ref _form1.client, _form1.sSessionId);
                }

                if (worker.CancellationPending) return;           
                
                PrintCourseNames();
                bCoursesDone = true;

                iPercentage = 100;
                worker.ReportProgress(iPercentage);
            }
            bCoursesDone = false;
            e.Result = iPercentage;
        }

        private void Worker_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {
            progBar.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.WriteLine(e.Result.ToString());
            if (bCancelled) Application.Exit();
        }
    }
}
