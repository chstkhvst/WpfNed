using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;
using WpfNed.Model;

namespace WpfNed.ViewModel
{
    using RealEstateObject = WpfNed.EF.REObject;
    public class SharedVM
    {
        public ObjectTableVM ObjectTableVM { get; set; }
        public UserVM UserVM { get; set; }
        public ContractVM ContractVM { get; set; }

        //TableModel tb = new TableModel();

        public SharedVM()
        {
            ObjectTableVM = new ObjectTableVM();
            UserVM = new UserVM();
            ContractVM = new ContractVM();
        }


    }
}
