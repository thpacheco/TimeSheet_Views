using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Forms.Models;
using TimeSheet.Forms.Service;

namespace TimeSheet.Forms.Controller.MarcacaoController
{
    public class MarcacaoEntrada : IMarcacao
    {
        private readonly ApontamentoService _apontamentoService = new ApontamentoService();

        public async void EfetuarMarcacao(Marcacao marcacao)
        {
            Apontamento apontamento = new Apontamento();

            apontamento.IdUsuario = marcacao.IdUsuario;
            apontamento.Entrada = marcacao.Entrada;
            apontamento.DataMarcacao = DateTime.Now.Date.ToString("dd/MM/yyyy");
            await _apontamentoService.CadatrarMarcacao(apontamento);
        }
    }
}
