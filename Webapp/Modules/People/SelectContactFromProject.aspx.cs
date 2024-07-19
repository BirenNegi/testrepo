using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;


namespace SOS.Web
{
    public partial class SelectContactFromProject : System.Web.UI.Page
    {

        protected String GvPeopleSortExpression
        {
            get { return (String)ViewState["GvPeopleSortExpression"]; }
            set { ViewState["GvPeopleSortExpression"] = value; }
        }

        protected SortDirection GvPeopleSortDirection
        {
            get { return (SortDirection)ViewState["GvPeopleSortDirection"]; }
            set { ViewState["GvPeopleSortDirection"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GvPeopleSortDirection = SortDirection.Ascending;


           

        }

        protected void gvPeople_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvPeople.PageIndex = 0;

            if (GvPeopleSortExpression == e.SortExpression)
                GvPeopleSortDirection = Utils.ChangeSortDirection(GvPeopleSortDirection);
            else
            {
                GvPeopleSortExpression = e.SortExpression;
                GvPeopleSortDirection = SortDirection.Ascending;
            }

            e.SortDirection = GvPeopleSortDirection;
        }

        protected void gvPeople_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
           

            Utils.SortedGridSetOrderImage(gvPeople, e, GvPeopleSortExpression, GvPeopleSortDirection);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            PeopleController peopleController = PeopleController.GetInstance();
            TransmittalInfo transmittalInfo;
            ContactInfo contactInfo;
            Boolean flag = false;
            string peopleId, subcontractorId, TransmittalId;
            TransmittalId = Request.QueryString["TransmittalId"].ToString();
            transmittalInfo = projectsController.GetDeepTransmittal(Int32.Parse(TransmittalId));
            foreach (GridViewRow gvRow in gvPeople.Rows)
            {
                CheckBox chk=(CheckBox)(gvRow.Cells[0].Controls[1]);
                if (chk != null && chk.Checked)
                {
                    int i = gvRow.RowIndex;
                    peopleId = gvPeople.DataKeys[i].Value.ToString();
                    subcontractorId= gvRow.Cells[7].Text;

                    contactInfo = (ContactInfo)peopleController.GetPersonById(Int32.Parse(peopleId));
                    if (transmittalInfo.Contacts == null || transmittalInfo.Contacts.Find(delegate (ContactInfo contactInfoInList) { return contactInfoInList.Equals(contactInfo); }) == null)
                    {
                        projectsController.AddTransmittalContact(transmittalInfo, contactInfo);
                        flag = true;
                    }  
                }

            }

            if (flag)
            {
                string close = @"<script type='text/javascript'>

                  if (window.opener != null && !window.opener.closed) 
                   {
                       window.opener.location.reload();
                   }
                      window.close();             
                 </script>";
                base.Response.Write(close);

            }



        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPeople.DataBind();
        }
    }
}