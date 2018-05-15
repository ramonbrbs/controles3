using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnvioArquivo : ContentPage
    {
        private int? _codTarefa;
        public EnvioArquivo(int? codTarefa = null)
        {
            if (codTarefa.HasValue)
            {
                _codTarefa = codTarefa;
                StckTarefa.IsVisible = true;
            }
            InitializeComponent();
        }

        private FileData arquivo;
        private async void Selecionar_Clicked(object sender, EventArgs e)
        {
            FileData fileData = await CrossFilePicker.Current.PickFile();
            if (fileData != null)
            {
                LblNomeArquivo.Text = fileData.FileName;
                arquivo = fileData;
            }
        }
    }
}