﻿using System;
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

        public async Task<bool> EfetuarMarcacao(Marcacao marcacao)
        {
            string dataMarcacao = DateTime.Now.Date.ToString("dd/MM/yyyy");

            var apontamento = await _apontamentoService.BuscarMarcacaoNoDia(marcacao.IdUsuario, dataMarcacao);

            if (apontamento == null)
            {
                apontamento = new Apontamento
                {
                    IdUsuario = marcacao.IdUsuario,
                    Entrada = marcacao.Entrada,
                    DataMarcacao = dataMarcacao
                };
            }

            apontamento.Entrada = marcacao.Entrada;

            return await _apontamentoService.SalvarMarcacao(apontamento);
        }
    }
}
