using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.IModels
{
    public interface IContactInfoModel
    {
        public void UpdateContacts(string username, string argument, string info);
    }
}
