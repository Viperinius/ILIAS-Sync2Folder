using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace IliasDL
{
    public partial class FormFeedback : Form
    {
        public FormFeedback()
        {
            InitializeComponent();
        }

        private void FormFeedback_Load(object sender, EventArgs e)
        {
            LinkLabel.Link link = new LinkLabel.Link
            {
                LinkData = @"https://discord.gg/y5z7djw"
            };
            linklbDiscord.Links.Add(link);
        }

        private void LinklbDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinklbMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"mailto:viperinius@gmx.de?subject=ILIAS%20Sync2Folder%20Feedback");
        }
    }
}
