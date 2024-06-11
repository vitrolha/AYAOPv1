using AYAOPv1.Source.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AYAOPv1.Source.MVVM.Model
{
    public class ShortCutWithBitmapSourceDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string PathToExe { get; set; } = string.Empty;
        public BitmapSource BackgroundImageBitmapSource { get; set; } = null;
        public BitmapSource IconBitmapSource { get; set; } = null;
        public Process ShortCutProcess { get; set; }

        public static IEnumerable<ShortCutWithBitmapSourceDTO> GetShortCutWithBitmapSourceDTOList(IEnumerable<ShortCutDTO> shortCutDTOs)
        {
            List<ShortCutWithBitmapSourceDTO> list = new List<ShortCutWithBitmapSourceDTO>();
            foreach(var shortCut in shortCutDTOs)
            {
                list.Add(new ShortCutWithBitmapSourceDTO
                {
                    Name = shortCut.Name,
                    Path = shortCut.Path,
                    PathToExe = shortCut.PathToExe,
                    BackgroundImageBitmapSource = IconServices.GetBitmapSourceFromImage(shortCut.BackgroundImagePath),
                    IconBitmapSource = IconServices.BytesToBitmapSource(shortCut.IconBytes)
                });
            }
            return list;
        }
        public static ShortCutWithBitmapSourceDTO GetShortCutWithBitmapSourceDTO(ShortCutDTO shortCutDTO)
        {
            return new ShortCutWithBitmapSourceDTO
            {
                Name = shortCutDTO.Name,
                Path = shortCutDTO.Path,
                PathToExe = shortCutDTO.PathToExe,
                BackgroundImageBitmapSource =  IconServices.GetBitmapSourceFromImage(shortCutDTO.BackgroundImagePath),
                IconBitmapSource = IconServices.BytesToBitmapSource(shortCutDTO.IconBytes)
            };
        }
    }
}
