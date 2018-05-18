using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using ControleApp.Webservice;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
//using Plugin.Media;
//using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnvioArquivo : ContentPage
    {
        private int? _codTarefa;
        private int? _codAnot;
        private byte[] arquivoBytes;
        private Arquivo arquivo = new Arquivo();
        public EnvioArquivo(int? codTarefa = null, int? codAnot = null)
        {
            if (codTarefa.HasValue)
            {
                _codTarefa = codTarefa;
                StckTarefa.IsVisible = true;
            }

            _codAnot = codAnot;
            InitializeComponent();
            
        }

        protected override async void OnAppearing()
        {
            PckCliente.ItemsSource = await ClienteRN.GetClientes();
            base.OnAppearing();
        }

        //private FileData arquivo;
        private async void Selecionar_Clicked(object sender, EventArgs e)
        {
            var resultado = await DisplayActionSheet("Como deseja enviar?", "Cancelar", null, "Câmera", "Arquivos");
            if (resultado == "Arquivos")
            {
                SelecionarArquivo();
            }
            else
            {
                SelecionarCamera();
            }
        }

        private async void SelecionarArquivo()
        {
            FileData fileData = await CrossFilePicker.Current.PickFile();
            if (fileData != null)
            {
                LblNomeArquivo.Text = fileData.FileName;
                arquivo.Conteudo = fileData.DataArray;
                arquivo.DesrOrigArq = fileData.FileName;
                arquivoBytes = fileData.DataArray;
            }
        }

        

        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var arq = arquivo;
                arq.ProgNr = _codTarefa.HasValue ? _codTarefa.Value.ToString() : "0";
                arq.CodEmpresa = ((Cliente)PckCliente.SelectedItem)?.Id ?? 0;
                arq.CodRespDig = Session.Usuario.Usw_cod;
                arq.DesArq = TxtTexto.Text;
                arq.PcsArq_AnotNr = _codAnot.HasValue? _codAnot.Value.ToString() : "0";
                arq.PcsArq_CodOcorrencia = 0;
                var result = await TarefasWS.EnviarArquivo(arq);
                if (result)
                {
                    await DisplayAlert("Sucesso", "Arquivo enviado.", "Ok");
                    Session.Navigation.Navigation.RemovePage(this);
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Erro", exception.Message, "Ok");
            }
            
        }

        private void PckCliente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelCliente.Text = PckCliente.Items[PckCliente.SelectedIndex];
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            PckCliente.Focus();
        }

        private async void SelecionarCamera()
        {
            try
            {
                var armazenamento = new StoreCameraMediaOptions()
                {
                    Name = "foto.jpg",
                };
                var foto = await CrossMedia.Current.TakePhotoAsync(armazenamento);
                MemoryStream ms = new MemoryStream();

                if (foto != null)
                {
                    var str = foto.GetStream();
                    var fotoSource = ImageSource.FromStream(() =>
                    {
                        var stream = foto.GetStream();
                        foto.Dispose();
                        str = stream;
                        return stream;
                    });
                    str.CopyTo(ms);
                    arquivo.Conteudo = ms.ToArray();
                    arquivo.DesrOrigArq = "foto.jpg";

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("a", ex.Message, "ok");
                throw ex;
                
            }
            
        }
    }
}