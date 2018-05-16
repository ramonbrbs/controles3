using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleApp.Model
{
    public class Arquivo
    {
        public int CodEmpresa { get; set; }
        public string Tipo { get; set; } // usar AG
        public string ProgNr { get; set; } // ID da tarefa
        public int CodRespDig { get; set; }//Codigo do usuario logado
        public string NomeArquivo { get; set; } //O nome gerado no pela api(código da tarefa + digito sequencial)
        public string DesArq { get; set; } //descrição do arquivo
        public int SeqArq { get; set; } //sequencia do arquivo obtida
        public string DesrOrigArq { get; set; } //nome original do arquivo
        public string EndPasta { get; set; } //endereço da pasta onde foi salvo
        public int PcsArq_DtDigit { get; set; } //data atual
        public string PcsArq_AnotNr { get; set; } // Quando o arquivo for enviado do formulario anotacao devera armazenar o id da anotacao
        public int PcsArq_CodOcorrencia { get; set; } //id de outro formulario de ocorrencia
        public string TipoVinculo { get; set; } //nao preencher
        public string NrVinculo { get; set; }//nao preencher
        public int Fase { get; set; }//1-pendente 2=enviado
        public int EmNuvem { get; set; }//0
        public int EmServidor { get; set; }//1
        public int BaseOrigemLcto { get; set; }//1- se for agneda
        public byte[] Conteudo { get; set; }
    }
}
