using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IliasDL
{
    public partial class FormCourses : Form
    {
        private CConfig config = new CConfig();
        private readonly Form1 _form1;

        private BackgroundWorker worker;

        private int iPercentage;
        private bool bCoursesDone = false;
        private bool bCancelled = false;

        List<string> lCourses = new List<string>();
        List<string> lCourseNames = new List<string>();

        public FormCourses(Form1 form)
        {
            InitializeComponent();

            _form1 = form;

            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void FormCourses_Load(object sender, EventArgs e)
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

        private void FormCourses_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker.IsBusy)
            {
                bCancelled = true;
                worker.CancelAsync();
                e.Cancel = true;
                this.Enabled = false;
            }
        }

        private void PrintCourses()
        {
            lCourses = _form1.lCourses;
            lCourseNames = _form1.lCourseNames;

            string sRef = "";
            string sOwnName = "";

            for (int i = 0; i < lCourses.Count; i++)
            {
                if (config.GetUseOwnNames() == "true")
                {
                    sOwnName = config.GetCourseName(lCourses[i]);

                    if (config.GetSyncAll() == "true")
                    {
                        if (sOwnName != "__NO_VAL__")
                        {
                            AddToListView(true, lCourseNames[i], sOwnName, lCourses[i]);
                        }
                        else
                        {
                            AddToListView(true, lCourseNames[i], "", lCourses[i]);
                        }
                    }
                    else if (config.GetSyncAll() == "false")
                    {
                        sRef = config.GetCourse(lCourses[i]);
                        if (sRef != "__NO_VAL__")
                        {
                            if (sOwnName != "__NO_VAL__")
                            {
                                AddToListView(true, lCourseNames[i], sOwnName, lCourses[i]);
                            }
                            else
                            {
                                AddToListView(true, lCourseNames[i], "", lCourses[i]);
                            }
                        }
                        else
                        {
                            if (sOwnName != "__NO_VAL__")
                            {
                                AddToListView(false, lCourseNames[i], sOwnName, lCourses[i]);
                            }
                            else
                            {
                                AddToListView(false, lCourseNames[i], "", lCourses[i]);
                            }
                        }
                    }
                }
                else if (config.GetUseOwnNames() == "false")
                {
                    if (config.GetSyncAll() == "true")
                    {
                        AddToListView(true, lCourseNames[i], "", lCourses[i]);
                    }
                    else if (config.GetSyncAll() == "false")
                    {
                        sRef = config.GetCourse(lCourses[i]);
                        if (sRef != "__NO_VAL__")
                        {
                            AddToListView(true, lCourseNames[i], "", lCourses[i]);
                        }
                        else
                        {
                            AddToListView(false, lCourseNames[i], "", lCourses[i]);
                        }
                    }
                }
                
            }
        }

        private void AddToListView(bool bStatus, string sCourse, string sOwnName, string sRefId)
        {
            string[] row = { "", sCourse, sOwnName, sRefId };
            if (listViewCourses.InvokeRequired)
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewCourses.Invoke(new AddToListViewCallback(AddToListView), new object[] {
                    bStatus,
                    sCourse,
                    sOwnName,
                    sRefId
                });
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem(row);
                listViewCourses.Items.Add(listViewItem);
                if (bStatus)
                {
                    var items = listViewCourses.Items;
                    listViewCourses.Items[items.Count - 1].Checked = true;
                }
            }

        }
        public delegate void AddToListViewCallback(bool bStatus, string sCourse, string sOwnName, string sRefId);

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
            int iColumn = 0;
            switch (sCategory)
            {
                case "Kurs":
                    iColumn = 1;
                    break;
                case "Eigener Name":
                    iColumn = 2;
                    break;
                case "Ref ID":
                    iColumn = 3;
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

        

        

        private void CheckCheckBox()
        {
            if (chkSyncAll.InvokeRequired)
            {
                chkSyncAll.Invoke(new Action(CheckCheckBox));
            }
            else
            {
                chkSyncAll.Checked = true;
            }
        }

        private void ChkSyncAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSyncAll.Checked)
            {
                config.SetSyncAll(true);
            }
            else
            {
                config.SetSyncAll(false);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            foreach (string sRef in lCourses)
            {
                config.ClearCoursesSettings(sRef);
            }
            if (config.GetSyncAll() == "false")
            {
                int i = 0;
                foreach (ListViewItem item in listViewCourses.CheckedItems)
                {
                    config.SetCourse(item.SubItems[3].Text);
                    i++;
                }
            }
        }

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

                if (config.GetSyncAll() == "true")
                {
                    CheckCheckBox();
                }
                if (_form1.bLoggedIn)
                {
                    iPercentage = 17;   //random value to jump to (to keep users patient)
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

                }
                else
                {
                    MessageBox.Show(@"Please log in first","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    worker.CancelAsync();
                    return;
                }
                PrintCourses();
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
