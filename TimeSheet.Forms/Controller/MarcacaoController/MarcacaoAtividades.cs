﻿using System;
using System.Threading.Tasks;
using TimeSheet.Forms.Models;
using TimeSheet.Forms.Service;

namespace TimeSheet.Forms.Controller.MarcacaoController
{
    public class MarcacaoAtividades : IMarcacao
    {
        private readonly ApontamentoService _apontamentoService = new ApontamentoService();

        public async Task<bool> EfetuarMarcacao(Marcacao marcacao)
        {
            string dataMarcacao = DateTime.Now.Date.ToString("dd/MM/yyyy");

            var apontamento = await _apontamentoService.BuscarMarcacaoNoDia(marcacao.IdUsuario, dataMarcacao);

            if (apontamento == null)
            {
                apontamento = new Apontamento
                {
                    IdUsuario = marcacao.IdUsuario,
                    DataMarcacao = dataMarcacao,
                    DescricaoAtividade = marcacao.DescricaoAtividade,
                    CodigoAtividade = marcacao.CodigoAtividade

                };
            }
            apontamento.DescricaoAtividade = marcacao.DescricaoAtividade;
            apontamento.CodigoAtividade = marcacao.CodigoAtividade;

            return await _apontamentoService.SalvarMarcacao(apontamento);
        }
    }
}
