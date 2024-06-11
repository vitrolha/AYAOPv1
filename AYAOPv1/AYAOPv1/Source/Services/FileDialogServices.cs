using System.Windows.Forms;

namespace AYAOPv1.Source.Services
{
    public static class FileDialogServices
    {
        public static string FilePathSelected()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Atalhos (*.lnk)|*.lnk|Todos (*.*)|*.*";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) return openFileDialog.FileName;
            return "";
        }
    }
}
