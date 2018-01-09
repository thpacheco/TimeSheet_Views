using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Forms.Models;

namespace TimeSheet.Forms.Controller.MarcacaoController
{
    public class Marcacao
    {
        public string Id { get; set; }
        public string IdUsuario { get; set; }
        public string Entrada { get; set; }
        public string SaidaAlmoco { get; set; }
        public string RetornoAlmoco { get; set; }
        public string Saida { get; set; }
        public string DataMarcacao { get; set; }
        public string HorasTotalDia { get; set; }
        public string DescricaoAtividade { get; set; }
        public string CodigoAtividade { get; set; }

        public Marcacao(Apontamento apontamento)
        {
            this.Id = apontamento.Id;
            this.IdUsuario = apontamento.IdUsuario;
            this.Entrada = apontamento.Entrada;
            this.SaidaAlmoco = apontamento.SaidaAlmoco;
            this.RetornoAlmoco = apontamento.RetornoAlmoco;
            this.Saida = apontamento.Saida;
            this.DataMarcacao = apontamento.DataMarcacao;
            this.HorasTotalDia = apontamento.HorasTotalDia;
            this.DescricaoAtividade = apontamento.DescricaoAtividade;
            this.CodigoAtividade = apontamento.CodigoAtividade;
        }

    }
    public class EfetuaMarcacao
    {
        public async Task<bool> RealizaMarcacao(Marcacao marcacao, IMarcacao iMarcacao)
        {
            return await iMarcacao.EfetuarMarcacao(marcacao);
        }
    }
}
