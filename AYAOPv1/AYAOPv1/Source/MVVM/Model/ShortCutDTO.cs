using AYAOPv1.Source.Services;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace AYAOPv1.Source.MVVM.Model
{
    public class ShortCutDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Path {  get; set; } = string.Empty;
        public string PathToExe {  get; set; } = string.Empty;
        public string BackgroundImagePath {  get; set; } = string.Empty;
        public byte[] IconBytes { get; set; }
    }
}
