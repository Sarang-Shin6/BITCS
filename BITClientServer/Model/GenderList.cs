using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer.Model
{
    static class GenderList
    {
        public static ObservableCollection<char> GenderCollection = new ObservableCollection<char>();

        static GenderList()
        {
            GenderCollection.Add('M');
            GenderCollection.Add('F');
        }
    }
}
