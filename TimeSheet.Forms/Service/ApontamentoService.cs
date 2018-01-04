using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimeSheet.Forms.Models;
using Extension;

namespace TimeSheet.Forms.Service
{
    public class ApontamentoService
    {
        public readonly HttpClient client = new HttpClient();
        public ApontamentoService()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:4942/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Uri> CadatrarMarcacao(Apontamento apontamento)
        {

            try
            {
                var serializedApontamento = JsonConvert.SerializeObject(apontamento);

                var content = new StringContent(serializedApontamento, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(client.BaseAddress + "api/cadastrar", content);

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao salvar marcação", ex);
            }
        }

        public async Task<List<Apontamento>> ListarApontamentos()
        {
            List<Apontamento> apontamento = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "api/apontamentos");
            if (response.IsSuccessStatusCode)
            {
                var apontamenroJsonString = await response.Content.ReadAsStringAsync();
                apontamento = JsonConvert.DeserializeObject<Apontamento[]>(apontamenroJsonString).ToList();
            }
            return apontamento;
        }

        public async Task<Apontamento> BuscarMarcacaoNoDia(string idUsuario, string dataMarcacao)
        {
            Apontamento apontamento = null;
            HttpResponseMessage response = await client
                .GetAsync(client.BaseAddress + "api/apontamento/consultar-apontamento/" + idUsuario + "/" + dataMarcacao.FormatarDataEnvio());
            if (response.IsSuccessStatusCode)
            {
                var apontamenroJsonString = await response.Content.ReadAsStringAsync();
                apontamento = JsonConvert.DeserializeObject<Apontamento>(apontamenroJsonString);
            }
            return apontamento;
        }
    }
}


