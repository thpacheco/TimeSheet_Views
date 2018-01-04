using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using Newtonsoft.Json;


namespace TimeSheet.Forms
{
    public partial class FormApontamento : MetroForm
    {
        public FormApontamento()
        {
            InitializeComponent();
            // Here we call Regex.Match.
            String strpattern = @"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"; //Pattern is Ok
            Regex regex = new Regex(strpattern);

            if (regex.Match("09:00").Success)
            {
                MetroMessageBox.Show(null, "Hora Ínvalida", "Hora ínvalida", MessageBoxButtons.RetryCancel);
            }
            //chamando a api pela url
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/timesheet/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/apontamentos").Result;
                if (response.IsSuccessStatusCode)
                {
                    var apontamento = response.Content.ReadAsStringAsync();
                    var dados = JsonConvert.DeserializeObject<List<Apontamento>>(apontamento.Result);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }


        #region Ações Entrada
        private void btnRelogioEntrada_Click(object sender, EventArgs e)
        {
            txtEntrada.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void btnEntrada_Click(object sender, EventArgs e)
        {
            Entrada();
        }

        private void CancelEntrada_Click(object sender, EventArgs e)
        {
            txtEntrada.Visible = true;

            txtEntrada.Text = "";

            lblMarcacaoEntrada.Visible = false;

            checkEntrada.Visible = false;

            CancelEntrada.Visible = false;

            btnEntrada.Visible = true;

        }

        #endregion
        #region Ações Saida Almoço
        private void btnRelogioSaidaAlmoco_Click(object sender, EventArgs e)
        {
            txtSaidaAlmoco.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void btnSaidaAlmoco_Click(object sender, EventArgs e)
        {
            SaidaAlmoco();
        }
        private void CancelSaidaAlmoco_Click(object sender, EventArgs e)
        {
            txtSaidaAlmoco.Visible = true;

            txtSaidaAlmoco.Text = "";

            lblMarcacaoSaidaAlmoco.Visible = false;

            checkSaidaAlmoco.Visible = false;

            CancelSaidaAlmoco.Visible = false;

            btnSaidaAlmoco.Visible = true;
        }
        #endregion
        #region Ações Retorno Almoço
        private void btnRelogioRetornoAlmoco_Click(object sender, EventArgs e)
        {
            txtRetornoAlmoco.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void btnRetornoAlmoco_Click(object sender, EventArgs e)
        {
            RetornoAlmoco();
        }
        private void CancelRetornoAlmoco_Click(object sender, EventArgs e)
        {
            txtRetornoAlmoco.Visible = true;

            txtRetornoAlmoco.Text = "";

            lblMarcacaoRetornoAlmoco.Visible = false;

            checkRetornoAlmoco.Visible = false;

            CancelRetornoAlmoco.Visible = false;

            btnRetornoAlmoco.Visible = true;
        }
        #endregion
        #region Ações Saida
        private void btnRelogioSaida_Click(object sender, EventArgs e)
        {
            txtSaida.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void btnSaida_Click(object sender, EventArgs e)
        {
            Saida();
        }

        private void CancelSaida_Click(object sender, EventArgs e)
        {
            txtSaida.Visible = true;

            txtSaida.Text = "";

            lblMarcacaSaida.Visible = false;

            checkSaida.Visible = false;

            CancelSaida.Visible = false;

            btnSaida.Visible = true;
        }
        #endregion
        #region Metodos Saída

        public void Saida()
        {
            checkSaida.Visible = true;

            CancelSaida.Visible = true;

            btnSaida.Visible = false;

            lblMarcacaSaida.Text = txtEntrada.Text;

            lblMarcacaSaida.Visible = true;

            txtSaida.Visible = false;
        }

        #endregion
        #region Metodos Entrada

        public void Entrada()
        {

            MessageBox.Show("Marcação Registrada!");

            MarcacaoEntradaEfetuada(txtEntrada.Text);


        }
        public void MarcacaoEntradaEfetuada(string entrada)
        {
            checkEntrada.Visible = true;

            CancelEntrada.Visible = true;

            btnEntrada.Visible = false;

            lblMarcacaoEntrada.Text = entrada;

            lblMarcacaoEntrada.Visible = true;

            txtEntrada.Visible = false;
        }
        #endregion
        #region Metodos Saida Almoço
        public void SaidaAlmoco()
        {
            checkSaidaAlmoco.Visible = true;

            CancelSaidaAlmoco.Visible = true;

            btnSaidaAlmoco.Visible = false;

            lblMarcacaoSaidaAlmoco.Text = txtSaidaAlmoco.Text;

            lblMarcacaoSaidaAlmoco.Visible = true;

            txtSaidaAlmoco.Visible = false;
        }
        #endregion
        #region Metodos Retorno Almoço
        public void RetornoAlmoco()
        {
            checkRetornoAlmoco.Visible = true;

            CancelRetornoAlmoco.Visible = true;

            btnRetornoAlmoco.Visible = false;

            lblMarcacaoRetornoAlmoco.Text = txtRetornoAlmoco.Text;

            lblMarcacaoRetornoAlmoco.Visible = true;

            txtRetornoAlmoco.Visible = false;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
    public class Apontamento
    {
        public string Id { get; set; }

        public string IdUsuario { get; set; }

        public string Entrada { get; set; }

        public string SaidaAlmoco { get; set; }

        public string RetornoAlmoco { get; set; }

        public string Saida { get; set; }

        public string DataMarcacao { get; set; }
    }
}
