using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using TimeSheet.Forms.Models;
using TimeSheet.Forms.Service;

namespace TimeSheet.Forms
{
    public partial class FormLogin : MetroForm
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, System.EventArgs e)
        {
            Autenticar();
        }

        private async void Autenticar()
        {
            if (ValidarCampos())
            {
                if (await BuscarUsuario())
                {
                    FormApontamento formApontamento = new FormApontamento();
                    formApontamento.ShowDialog(this);
                    Close();
                }
                else
                {
                    MetroMessageBox.Show(this, "Usuário ou senha inválido", "Erro ao atenticar", MessageBoxButtons.OK);
                }
            }
        }

        private async Task<bool> BuscarUsuario()
        {
            FormApontamento formApontamento = new FormApontamento();

            string login = txtLogin.Text;
            string senha = txtSenha.Text;

            Usuario usuario = await _usuarioService.AutenticarUsuario(login, senha);

            if (usuario != null) formApontamento.ObterIdUsuario(usuario.Id);

            return usuario != null;
        }

        private bool ValidarCampos()
        {
            if (txtLogin.Text == "") return false;

            if (txtSenha.Text == "") return false;

            return true;
        }

        private void txtSenha_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Autenticar();
            }
        }
    }
}
