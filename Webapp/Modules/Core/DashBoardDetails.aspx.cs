using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using System.Drawing;

namespace SOS.Web
{
    public partial class DashBoardDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Width = 150;
            Label1.Text = "4";
            Label1.BackColor = Color.Red;

            Label2.Width = 50;
            Label2.Text = "3";
            Label2.BackColor = Color.Orange;

            Label3.Width = 300;
            Label3.Text = "6";
            Label3.BackColor = Color.Green;


            Label4.Width = 250;
            Label4.Text = "7";
            Label4.BackColor = Color.Red;

            Label5.Width = 150;
            Label5.Text = "3";
            Label5.BackColor = Color.Orange;

            Label6.Width = 100;
            Label6.Text = "1";
            Label6.BackColor = Color.Green;


            Label7.Width = 0;
            Label7.Text = "";
            Label7.BackColor = Color.Red;

            Label8.Width = 150;
            Label8.Text = "3";
            Label8.BackColor = Color.Orange;

            Label9.Width = 350;
            Label9.Text = "5";
            Label9.BackColor = Color.Green;
        }
    }
}