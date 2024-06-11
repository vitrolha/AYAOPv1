using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAOPv1.Source.Interfaces
{
    public interface IJsonFile
    {
        void Write(string json);
        string Read();
    }
}
