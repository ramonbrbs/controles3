using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using ControleApp.Webservice;
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
        private int? _codAnot;
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

        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var arq = new Arquivo();
                arq.Conteudo = arquivo.DataArray;
                arq.ProgNr = _codTarefa.HasValue ? _codTarefa.Value.ToString() : "0";
                arq.CodEmpresa = ((Cliente)PckCliente.SelectedItem)?.Id ?? 0;
                arq.CodRespDig = Session.Usuario.Usw_cod;
                arq.DesArq = TxtTexto.Text;
                arq.PcsArq_AnotNr = _codAnot.HasValue? _codAnot.Value.ToString() : "0";
                arq.DesrOrigArq = arquivo.FileName;
                arq.PcsArq_CodOcorrencia = 0;
                var result = await TarefasWS.EnviarArquivo(arq);
                if (result)
                {

                }
            }
            catch (Exception exception)
            {
                DisplayAlert("Erro", exception.Message, "Ok");
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
    }
}