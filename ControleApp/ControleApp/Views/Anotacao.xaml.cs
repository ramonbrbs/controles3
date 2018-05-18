using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Anotacao : ContentPage
    {
        private Tarefas tarefa;
        private Tarefas tarefaPesq;
        private TarefasAnot tarefaAnot;
        //private string usw_usu;
        private List<Tarefas> tarefas = new List<Tarefas>();

        public Anotacao(Tarefas t, TarefasAnot An)
        {
            InitializeComponent();
            tarefa = t;
            tarefaAnot = An;
            //usw_usu = usw_usuario;
            NavigationPage.SetHasBackButton(this, false);

        }
        private void Menu_clicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new Menu());
        }

        protected override void OnAppearing()
        {

            if (tarefa != null)
            {
                TxtDataFim.Date = tarefa.DATA_PROGR;
                if (tarefa.HISTORICO.Length > 45)
                {
                    TxtTarefaDesc.Text = tarefa.HISTORICO.Substring(0, 45) + " ...";

                }
                else
                {
                    TxtTarefaDesc.Text = tarefa.HISTORICO;

                }
                List<String> acoes = new List<string>();
                acoes.Add("      Incluir ");
                acoes.Add("Ler / Aceitar ");
                acoes.Add("       Baixar ");
                PckAcao.ItemsSource = acoes;
                PckAcao.SelectedIndex = 0;
            }

            if (tarefaAnot != null)
            {
                TxtTexto.Text = tarefaAnot.Anot_histor;
            }
        }

        private async void Enviar(object sender, EventArgs e)
        {
            try
            {
                StckSucesso.IsVisible = false;
                var t = new Tarefas();
                //t.CLIENTE = (PckCliente.SelectedIndex == -1) ? Convert.ToInt32(TxtCliente.Text) : ((Cliente)PckCliente.SelectedItem).Id;
                t.DATA_PROGR = TxtDataFim.Date;
                t.SOLICITANTE = Session.Usuario.Usw_cod;
                t.RESPOSAVEL = tarefa.RESPOSAVEL;
                t.HISTORICO = Session.Usuario.Usw_Nome.Substring(0, Session.Usuario.Usw_Nome.IndexOf(" ") + 2) + ": " + TxtTexto.Text;
                t.CodPro = tarefa.CodPro;
                t.Pgr_Fase = 100;
                string retorno = "";
                if (PckAcao.SelectedIndex == 0)
                {
                    retorno = await TarefasRN.IncluirAnotacao(t);
                    if (String.IsNullOrEmpty(retorno))
                    {
                        StckSucesso.IsVisible = true;
                        TxtSucesso.Text = "Nova Anotação Incuida. Email Enviado.";
                        //clsEnviarEmail mail = new clsEnviarEmail();
                        //var usuario = await UsuarioRN.GetUsuario(usw_usu, "123");
                        //string body = "Nova Anotação Incluida Nr." + t.CodPro + " em: " + DateTime.Now.ToString() + "\n\n" + t.HISTORICO;
                        //mail.EnviarEmail(usuario.First().Usw_EmailSenha, "Nova Anotação de Tarefa Recebida", body, string.Empty);
                        
                    }
                }
                if (PckAcao.SelectedIndex == 1)
                {
                    //retorno = await TarefasRN.IncluirAnotacao(t);
                    if (String.IsNullOrEmpty(retorno))
                    {
                        StckSucesso.IsVisible = true;
                        TxtSucesso.Text = "Anotação Lida.";
                    }
                }
                if (PckAcao.SelectedIndex == 2)
                {
                    //retorno = await TarefasRN.IncluirAnotacao(t);
                    if (String.IsNullOrEmpty(retorno))
                    {
                        StckSucesso.IsVisible = true;
                        TxtSucesso.Text = "Anotação foi Baixada.";
                    }
                }

            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public class ListaVM
        {
            public string Anot_DataAnot { get; set; }
            public string Anot_histor { get; set; }

        }

        private async void Lista_clicked(object sender, EventArgs e)
        {

            tarefas = await TarefasRN.GetTarefas(Session.UltOpMenu1, Session.Usuario.Usw_cod.ToString());
            tarefaPesq = tarefas.Where(t => t.CodPro == tarefaAnot.Anot_CodProgr).First();

            await Session.Navigation.Navigation.PushAsync(new NovaTarefa(tarefaPesq));
        }

        private void Eu_Clicked(object sender, EventArgs e)
        {
            //PckPara.SelectedItem = usuarios.Where(s => s.Usw_cod == Session.Usuario.Usw_cod).FirstOrDefault();
        }

        //private void PickerLabel_OnTapped(object sender, EventArgs e)
        //{
        //    PckPara.Focus();
        //}
        private void textTap(object sender, EventArgs e)
        {
            TxtTexto.Focus();
            if (TxtTexto.Text == "Descrição")
            {
                TxtTexto.Text = "";
            }
        }
        private void Enviar_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Erro", "Por favor preencha os campos necessários.", "OK");
        }
        private void PickerLabelAcao_OnTapped(object sender, EventArgs e)
        {
            PckAcao.Focus();
        }
        private void PickerListAcao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelAcao.Text = PckAcao.Items[PckAcao.SelectedIndex];
        }

        private void BtnApagar_OnClicked(object sender, EventArgs e)
        {

            TxtTexto.Text = "";
            StckSucesso.IsVisible = false;
        }
        //private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PickerLabel.Text = PckPara.Items[PckPara.SelectedIndex];
        //}
        private void BtnAnexar_OnClicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new EnvioArquivo());
        }
    }

}