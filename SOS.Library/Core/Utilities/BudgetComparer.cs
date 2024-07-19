using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Core
{
    public class BudgetComparer : IComparer<IBudget>
    {
        public int Compare(IBudget iBudget1, IBudget iBudget2)
        {
            if (iBudget1.BudgetDate == null)
            {
                if (iBudget2.BudgetDate == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (iBudget2.BudgetDate == null)
                {
                    return -1;
                }
                else
                {
                    return iBudget1.BudgetDate.Value.CompareTo(iBudget2.BudgetDate.Value);
                }
            }

        }
    }
}
