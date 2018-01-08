using System.Threading.Tasks;

namespace TimeSheet.Forms.Controller.MarcacaoController
{
    public interface IMarcacao
    {
        Task<bool> EfetuarMarcacao(Marcacao marcacao);
    }
}
