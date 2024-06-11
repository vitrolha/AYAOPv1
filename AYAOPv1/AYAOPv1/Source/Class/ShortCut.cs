using AYAOPv1.Source.Abstract;
using AYAOPv1.Source.Interfaces;
using System;
using System.Drawing;
using Shell32;
using System.IO;
using AYAOPv1.Source.Services;

namespace AYAOPv1.Source.Class
{
    public class ShortCut : FileBase, IShortCut
    {
        private Icon icon;
        private string pathToExe = string.Empty;
        public ShortCut(string name, string path) : base(name, path)
        {
        }

        public Icon GetIconFromPath()
        {
            return IconServices.GetIconFromPath(GetPath);
        }

        public string GetPathToExeFromShortCut()
        {
            return IconServices.GetExePathFromIcon(GetPath);
        }

    }
}
