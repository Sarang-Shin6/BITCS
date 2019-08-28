using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer.Model
{
    static class StateList
    {
        // List to Fill State Comboboxes

        public static ObservableCollection<string> StateCollection = new ObservableCollection<string>();

        static StateList()
        {
            StateCollection.Add("ACT");
            StateCollection.Add("NSW");
            StateCollection.Add("NT");
            StateCollection.Add("QLD");
            StateCollection.Add("SA");
            StateCollection.Add("TAS");
            StateCollection.Add("WA");
            StateCollection.Add("VIC");
        }
    }
}
