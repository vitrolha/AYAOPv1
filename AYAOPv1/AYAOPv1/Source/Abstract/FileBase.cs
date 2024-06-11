using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAOPv1.Source.Abstract
{
    public abstract class FileBase
    {
        private string name = string.Empty;
        private string path = string.Empty;
        protected FileBase(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string GetPath {  get { return path; } }
        public string GetName { get { return name; } }
    }
}
