using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAOPv1.Source.Interfaces
{
    public interface IShortCut
    {
        Icon GetIconFromPath();
        string GetPathToExeFromShortCut();
    }
}
