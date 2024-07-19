using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOS.Web
{
    public partial class GridPageSizeControl : UserControl
    {
#region Private members;
        private GridView gridView = null;
#endregion

#region public Members
        public event EventHandler NumRecordsSelected;
#endregion

#region Public properties
        public GridView GridView
        {
            set
            {
                gridView = value;

                if (gridView.AllowPaging)
                {
                    switch (gridView.PageSize)
                    {
                        case 10: lnkPage10_Click(null, null); break;
                        case 25: lnkPage25_Click(null, null); break;
                        case 50: lnkPage50_Click(null, null); break;
                        default: lnkPageAll_Click(null, null); break;
                    }
                }
                else
                {
                    lnkPageAll_Click(null, null);
                }
            }
        }

        public Int32 PageSize
        {
            get
            {
                if (gridView != null)
                {
                    return gridView.PageSize;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (gridView != null)
                {
                    gridView.AllowPaging = true;

                    if (gridView.PageSize != value)
                        gridView.PageIndex = 0;

                    gridView.PageSize = value;

                    if (NumRecordsSelected != null)
                        NumRecordsSelected(this, new EventArgs());
                }
            }
        }
#endregion

#region Events
        protected void lnkPage10_Click(object sender, EventArgs e)
        {
            PageSize = 10;

            lblPage10.Visible = true;
            lblPage25.Visible = false;
            lblPage50.Visible = false;
            lblPageAll.Visible = false;

            lnkPage10.Visible = false;
            lnkPage25.Visible = true;
            lnkPage50.Visible = true;
            lnkPageAll.Visible = true;
        }

        protected void lnkPage25_Click(object sender, EventArgs e)
        {
            PageSize = 25;

            lblPage10.Visible = false;
            lblPage25.Visible = true;
            lblPage50.Visible = false;
            lblPageAll.Visible = false;

            lnkPage10.Visible = true;
            lnkPage25.Visible = false;
            lnkPage50.Visible = true;
            lnkPageAll.Visible = true;
        }

        protected void lnkPage50_Click(object sender, EventArgs e)
        {
            PageSize = 50;

            lblPage10.Visible = false;
            lblPage25.Visible = false;
            lblPage50.Visible = true;
            lblPageAll.Visible = false;

            lnkPage10.Visible = true;
            lnkPage25.Visible = true;
            lnkPage50.Visible = false;
            lnkPageAll.Visible = true;
        }

        protected void lnkPageAll_Click(object sender, EventArgs e)
        {
            if (gridView != null)
            {
                gridView.AllowPaging = false;
                gridView.PageIndex = 0;

                if (NumRecordsSelected != null)
                    NumRecordsSelected(this, new EventArgs());
            }

            lblPage10.Visible = false;
            lblPage25.Visible = false;
            lblPage50.Visible = false;
            lblPageAll.Visible = true;

            lnkPage10.Visible = true;
            lnkPage25.Visible = true;
            lnkPage50.Visible = true;
            lnkPageAll.Visible = false;
        }
#endregion

    }
}
