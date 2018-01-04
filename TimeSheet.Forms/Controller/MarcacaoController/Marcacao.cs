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
        public string IdUsuario { get; set; }
        public string Entrada { get; set; }
        public string SaidaAlmoco { get; set; }
        public string RetornoAlmoco { get; set; }
        public string Saida { get; set; }

        public Marcacao(Apontamento apontamento)
        {
            this.IdUsuario = apontamento.IdUsuario;
            this.Entrada = apontamento.Entrada;
            this.SaidaAlmoco = apontamento.SaidaAlmoco;
            this.RetornoAlmoco = apontamento.RetornoAlmoco;
            this.Saida = apontamento.Saida;
        }

    }
    public class EfetuaMarcacao
    {
        public void RealizaMarcacao(Marcacao marcacao, IMarcacao iMarcacao)
        {
            iMarcacao.EfetuarMarcacao(marcacao);
        }
    }
}
