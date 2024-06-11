using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;

namespace AYAOPv1.Source.Components
{
    /// <summary>
    /// Lógica interna para PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public string Url {  get; set; }

        public PopupWindow()
        {
            InitializeComponent();
        }

        private async void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(InputUrlText.Text.Trim()))
                {
                    string url = InputUrlText.Text.Trim();
                    if (IsValidUrl(url))
                    {
                        if (await IsImageUrl(url))
                        {
                            Url = InputUrlText.Text;
                            DialogResult = true;
                            Close();
                        }
                        else MessageBox.Show("Esta Url não leva à uma imagem.");
                    }
                    else if (IsValidFilePath(url))
                    {
                        if (IsImageFile(url))
                        {
                            Url = InputUrlText.Text;
                            DialogResult = true;
                            Close();
                        }
                        else MessageBox.Show("Esta Url não leva à uma imagem.");
                    }
                    else MessageBox.Show("Link não é nem uma URL ou caminho de arquivo válido.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async Task<bool> IsImageUrl(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentType = response.Content.Headers.ContentType.MediaType;
                        return contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private bool IsValidFilePath(string filePath)
        {
            return Path.IsPathRooted(filePath) && File.Exists(filePath);
        }

        private bool IsImageFile(string filePath)
        {
            try
            {
                var image = System.Drawing.Image.FromFile(filePath);
                return true;
            }
            catch (OutOfMemoryException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }
}
