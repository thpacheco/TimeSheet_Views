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
    public class UsuarioService
    {
        public HttpClient client = new HttpClient();
        public UsuarioService()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost/timesheet/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public async Task<bool> SalvarMarcacao(Apontamento apontamento)
        //{

        //    try
        //    {
        //        HttpResponseMessage result;

        //        var serializedApontamento = JsonConvert.SerializeObject(apontamento);

        //        var content = new StringContent(serializedApontamento, Encoding.UTF8, "application/json");

        //        if (apontamento.Id == null)
        //        {
        //            result = await client.PostAsync(client.BaseAddress + "api/cadastrar", content);
        //        }
        //        else
        //        {
        //            result = await client.PostAsync(client.BaseAddress + "api/Atualizar", content);
        //        }

        //        return result.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Erro ao salvar marcação", ex);
        //    }
        //}

        //public async Task<bool> AtualizarMarcacao(Apontamento apontamento)
        //{

        //    try
        //    {
        //        var serializedApontamento = JsonConvert.SerializeObject(apontamento);

        //        var content = new StringContent(serializedApontamento, Encoding.UTF8, "application/json");
        //        var result = await client.PostAsync(client.BaseAddress + "api/atualizar", content);

        //        return result.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Erro ao salvar marcação", ex);
        //    }
        //}

        //public async Task<List<Apontamento>> ListarApontamentos()
        //{
        //    List<Apontamento> apontamento = null;
        //    HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "api/apontamentos");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var apontamenroJsonString = await response.Content.ReadAsStringAsync();
        //        apontamento = JsonConvert.DeserializeObject<Apontamento[]>(apontamenroJsonString).ToList();
        //    }
        //    return apontamento;
        //}

        public async Task<Usuario> AutenticarUsuario(string login, string senha)
        {
            Usuario usuario = null;
            HttpResponseMessage response = await client
                .GetAsync(client.BaseAddress + "api/usuario/" + login + "/" + senha);
            if (!response.IsSuccessStatusCode) return null;
            var usuarioJsonString = await response.Content.ReadAsStringAsync();
            usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJsonString);
            return usuario;
        }
    }
}


