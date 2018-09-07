using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für SyncPage.xaml
    /// </summary>
    public partial class SyncPage : UserControl
    {
        public SyncPage()
        {
            InitializeComponent();
            listViewSync.Items.Add(new ListViewItem { Content = "Test" });
        }
    }
}
