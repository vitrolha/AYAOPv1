using System.IO;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;
using Shell32;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media;

namespace AYAOPv1.Source.Services
{
    public static class IconServices
    {
        public static byte[] IconToBytes(Icon icon)
        {
            ImageConverter converter = new ImageConverter();
            Image imgIcon = icon.ToBitmap();
            byte[] arr = (byte[])converter.ConvertTo(imgIcon, typeof(byte[]));
            return arr;
        }

        public static Icon BytesToIcon(byte[] value)
        {
            using (var ms = new MemoryStream(value))
            {
                Bitmap btmp = new Bitmap(Image.FromStream(ms));
                return Icon.FromHandle(btmp.GetHicon());
            }
        }

        public static BitmapSource IconToBitmapSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static BitmapSource BytesToBitmapSource(byte[] iconBytes)
        {
            return Imaging.CreateBitmapSourceFromHIcon(BytesToIcon(iconBytes).Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static Icon GetIconFromPath(string shortcutPath)
        {
            return Icon.FromHandle(GetFileImageFromPath(shortcutPath, IconSizeEnum.LargeIcon48).GetHicon());
        }

        public static BitmapSource GetBitmapSourceFromImage(string imgPath)
        {
            try
            {
                Uri uri;
                if(Uri.TryCreate(imgPath, UriKind.Absolute, out uri))
                {
                    var btmp = new BitmapImage();
                    btmp.BeginInit();
                    btmp.UriSource = new Uri(imgPath);
                    btmp.EndInit();
                    return btmp;
                }
                else return null;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static string GetExePathFromIcon(string shortcutPath)
        {
            string iconExePath = "";

            if (File.Exists(shortcutPath))
            {
                Shell shell = new Shell();
                Folder folder = shell.NameSpace(Path.GetDirectoryName(shortcutPath));
                FolderItem folderItem = folder.ParseName(Path.GetFileName(shortcutPath));

                if (folderItem != null)
                {
                    ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;
                    iconExePath = link.Path;
                }
            }
            return iconExePath;
        }

        //To get bigger icons
        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IImageList
        {
            [PreserveSig]
            int Add(
                IntPtr hbmImage,
                IntPtr hbmMask,
                ref int pi);

            [PreserveSig]
            int ReplaceIcon(
                int i,
                IntPtr hicon,
                ref int pi);

            [PreserveSig]
            int SetOverlayImage(
                int iImage,
                int iOverlay);

            [PreserveSig]
            int Replace(
                int i,
                IntPtr hbmImage,
                IntPtr hbmMask);

            [PreserveSig]
            int AddMasked(
                IntPtr hbmImage,
                int crMask,
                ref int pi);

            [PreserveSig]
            int Draw(
                ref IMAGELISTDRAWPARAMS pimldp);

            [PreserveSig]
            int Remove(
                int i);

            [PreserveSig]
            int GetIcon(
                int i,
                int flags,
                ref IntPtr picon);
        };
        private struct IMAGELISTDRAWPARAMS
        {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap;
            public int yBitmap;
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 254)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szTypeName;
        }

        private const int SHGFI_SMALLICON = 0x1;
        private const int SHGFI_LARGEICON = 0x0;
        private const int SHIL_JUMBO = 0x4;
        private const int SHIL_EXTRALARGE = 0x2;
        private const int WM_CLOSE = 0x0010;

        public enum IconSizeEnum
        {
            SmallIcon16 = SHGFI_SMALLICON,
            MediumIcon32 = SHGFI_LARGEICON,
            LargeIcon48 = SHIL_EXTRALARGE,
            ExtraLargeIcon = SHIL_JUMBO
        }

        [DllImport("user32")]
        private static extern
            IntPtr SendMessage(
            IntPtr handle,
            int Msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("shell32.dll")]
        private static extern int SHGetImageList(
            int iImageList,
            ref Guid riid,
            out IImageList ppv);

        [DllImport("Shell32.dll")]
        private static extern int SHGetFileInfo(
            string pszPath,
            int dwFileAttributes,
            ref SHFILEINFO psfi,
            int cbFileInfo,
            uint uFlags);

        [DllImport("user32")]
        private static extern int DestroyIcon(
            IntPtr hIcon);

        private static Bitmap GetFileImageFromPath(
    string filepath, IconSizeEnum iconsize)
        {
            IntPtr hIcon = IntPtr.Zero;
            if (System.IO.Directory.Exists(filepath))
                hIcon = GetIconHandleFromFolderPath(filepath, iconsize);
            else
                if (System.IO.File.Exists(filepath))
                hIcon = GetIconHandleFromFilePath(filepath, iconsize);
            return GetBitmapFromIconHandle(hIcon);
        }

        private static IntPtr GetIconHandleFromFilePath(string filepath, IconSizeEnum iconsize)
        {
            var shinfo = new SHFILEINFO();
            const uint SHGFI_SYSICONINDEX = 0x4000;
            const int FILE_ATTRIBUTE_NORMAL = 0x80;
            uint flags = SHGFI_SYSICONINDEX;
            return GetIconHandleFromFilePathWithFlags(filepath, iconsize, ref shinfo, FILE_ATTRIBUTE_NORMAL, flags);
        }

        private static IntPtr GetIconHandleFromFolderPath(string folderpath, IconSizeEnum iconsize)
        {
            var shinfo = new SHFILEINFO();

            const uint SHGFI_ICON = 0x000000100;
            const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
            return GetIconHandleFromFilePathWithFlags(folderpath, iconsize, ref shinfo, FILE_ATTRIBUTE_DIRECTORY, flags);
        }

        private static System.Drawing.Bitmap GetBitmapFromIconHandle(IntPtr hIcon)
        {
            if (hIcon == IntPtr.Zero) return null;
            var myIcon = System.Drawing.Icon.FromHandle(hIcon);
            var bitmap = myIcon.ToBitmap();
            myIcon.Dispose();
            DestroyIcon(hIcon);
            SendMessage(hIcon, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            return bitmap;
        }

        private static IntPtr GetIconHandleFromFilePathWithFlags(
            string filepath, IconSizeEnum iconsize,
            ref SHFILEINFO shinfo, int fileAttributeFlag, uint flags)
        {
            const int ILD_TRANSPARENT = 1;
            var retval = SHGetFileInfo(filepath, fileAttributeFlag, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (retval == 0) throw (new System.IO.FileNotFoundException());
            var iconIndex = shinfo.iIcon;
            var iImageListGuid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            IImageList iml;
            var hres = SHGetImageList((int)iconsize, ref iImageListGuid, out iml);
            var hIcon = IntPtr.Zero;
            hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
            return hIcon;
        }
    }
}
