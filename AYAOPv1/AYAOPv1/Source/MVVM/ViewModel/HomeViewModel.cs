using AYAOPv1.Source.Class;
using AYAOPv1.Source.Components;
using AYAOPv1.Source.Interfaces;
using AYAOPv1.Source.MVVM.Model;
using AYAOPv1.Source.Services;
using AYAOPv1.Source.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AYAOPv1.Source.MVVM.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IShortCutRepository shortCutRepository;

        private List<ShortCutWithBitmapSourceDTO> shortCutProcessManagerList = new List<ShortCutWithBitmapSourceDTO>();

        //Commands
        public RelayCommand AddShortCutCommand { get; }
        public RelayCommand GetShortCutCommand { get; }
        public RelayCommand ChangeBgImageCommand { get; }
        public RelayCommand RunShortCutCommand { get; }
        public RelayCommand KillShortCutCommand { get; }

        //Propeties
        private IEnumerable<ShortCutWithBitmapSourceDTO> shortCutWithBmpSDTOList { get; set; }
        public IEnumerable<ShortCutWithBitmapSourceDTO> ShortCutWithBmpSDTOList
        {
            get { return shortCutWithBmpSDTOList; }
            set { shortCutWithBmpSDTOList = value; OnPropertyChanged(); }
        }

        private ShortCutWithBitmapSourceDTO shortCutWthBmpSouceDTO { get; set; }
        public ShortCutWithBitmapSourceDTO ShortCutWthBmpSouceDTO
        {
            get { return shortCutWthBmpSouceDTO; }
            set { shortCutWthBmpSouceDTO = value; OnPropertyChanged(); }
        }

        public HomeViewModel(IShortCutRepository shortCutRepository)
        {
            this.shortCutRepository = shortCutRepository;

            //Commands
            AddShortCutCommand = new RelayCommand(x => AddShortCut(), x => true);
            GetShortCutCommand = new RelayCommand(x => GetShortCut((string)x), x => true);
            ChangeBgImageCommand = new RelayCommand(x => ChangeBgImage((string)x), x => true);
            RunShortCutCommand = new RelayCommand(x => RunShortCut((string)x), x => true);
            KillShortCutCommand = new RelayCommand(x => KillShortCut((string)x), x => true);

            OnLoad();
        }

        public void OnLoad()
        {
            ShortCutWithBmpSDTOList = ShortCutWithBitmapSourceDTO.GetShortCutWithBitmapSourceDTOList(shortCutRepository.GetAll());
        }

        public void AddShortCut()
        {
            try
            {
                string shortCutPath = FileDialogServices.FilePathSelected();
                if (!string.IsNullOrEmpty(shortCutPath))
                {
                    ShortCut shortCut = new ShortCut(name: Path.GetFileNameWithoutExtension(shortCutPath), path: shortCutPath);

                    shortCutRepository.Save(new ShortCutDTO
                    {
                        Name = shortCut.GetName,
                        Path = shortCut.GetPath,
                        PathToExe = shortCut.GetPathToExeFromShortCut(),
                        IconBytes = IconServices.IconToBytes(shortCut.GetIconFromPath())
                    });

                    //MessageBox.Show($"Name: {shortCut.GetName} \nPath: {shortCut.GetPath} \nPathToExe: {shortCut.GetPathToExeFromShortCut()} \nIconBytes {string.Join("", IconServices.IconToBytes(shortCut.GetIconFromPath()))}");

                    ShortCutWithBmpSDTOList = ShortCutWithBitmapSourceDTO.GetShortCutWithBitmapSourceDTOList(shortCutRepository.GetAll());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetShortCut(string name)
        {
            try
            {
                ShortCutWthBmpSouceDTO = ShortCutWithBitmapSourceDTO.GetShortCutWithBitmapSourceDTO(shortCutRepository.Get(name));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChangeBgImage(string name)
        {
            try
            {
                string bgImagePath = "";
                PopupWindow popup = new PopupWindow();
                if (popup.ShowDialog() == true)
                {
                    bgImagePath = popup.Url;
                    shortCutRepository.ChangeBgImage(name, bgImagePath);

                    ShortCutWthBmpSouceDTO = ShortCutWithBitmapSourceDTO.GetShortCutWithBitmapSourceDTO(shortCutRepository.Get(name));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RunShortCut(string name)
        {
            try
            {
                ShortCutWthBmpSouceDTO = ShortCutWithBitmapSourceDTO.GetShortCutWithBitmapSourceDTO(shortCutRepository.Get(name));

                var startInfo = new ProcessStartInfo
                {
                    FileName = ShortCutWthBmpSouceDTO.PathToExe,
                    Verb = "runas",
                    WorkingDirectory = Path.GetDirectoryName(ShortCutWthBmpSouceDTO.PathToExe)
                };
                ShortCutWthBmpSouceDTO.ShortCutProcess = Process.Start(startInfo);

                //MessageBox.Show(ShortCutWthBmpSouceDTO.ShortCutProcess.Id.ToString());

                shortCutProcessManagerList.Add(ShortCutWthBmpSouceDTO);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void KillShortCut(string name)
        {
            try
            {
                var item = shortCutProcessManagerList.Find(x => x.Name == name);
                if (item != null)
                {
                    if (item.ShortCutProcess != null && !item.ShortCutProcess.HasExited)
                    {
                        //MessageBox.Show(item.ShortCutProcess.ProcessName + " " + item.ShortCutProcess.Id);
                        item.ShortCutProcess.Kill();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
