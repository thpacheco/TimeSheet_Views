using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using TimeSheet.Forms.Controller.MarcacaoController;
using TimeSheet.Forms.Models;
using TimeSheet.Forms.Service;
using TimeSheet.Forms.Util;

namespace TimeSheet.Forms
{
    public partial class FormApontamento : MetroForm
    {
        public static string IdUsuario = "5a4bcf4f7a0052364c68617f";
        public readonly ApontamentoService ApontamentoService = new ApontamentoService();
        private readonly IMarcacao _marcacaoEntrada = new MarcacaoEntrada();
        private readonly IMarcacao _marcacaoSaidaAlmoco = new MarcacaoSaidaAlmoco();
        private readonly IMarcacao _marcacaoRetornoAlmoco = new MarcacaoRetornoAlmoco();
        private readonly IMarcacao _marcacaoSaida = new MarcacaoSaida();
        private readonly EfetuaMarcacao _realizaMarcacao = new EfetuaMarcacao();
        public Apontamento Apontamento;
        public Marcacao Marcacao;

        public FormApontamento()
        {
            InitializeComponent();
            CarregarInformacaoUsuario();
            VerificaJaMarcadoNoDia();
        }

        private void CarregarInformacaoUsuario()
        {
            lblAnoRef.Text = DateTime.Now.Date.ToString("yyyy");
            lblMesRef.Text = DateTime.Now.Date.ToString("MM");
        }

        private async void VerificaJaMarcadoNoDia()
        {
            var dataMarcacao = DateTime.Now.Date.ToString("dd/MM/yyyy");
            Apontamento apontamento = await ApontamentoService.BuscarMarcacaoNoDia(IdUsuario, dataMarcacao);

            if (apontamento != null && apontamento.Entrada != null) CarregarMarcacaoEntrada(apontamento.Entrada);

            if (apontamento != null && apontamento.SaidaAlmoco != null)
            {
                CarregarMarcacaoSaidaAlmoco(apontamento.SaidaAlmoco);
                TempoTranscorrido(apontamento.Entrada, apontamento.SaidaAlmoco, apontamento.RetornoAlmoco);
            }
            if (apontamento != null && apontamento.RetornoAlmoco != null)
            {
                CarregarMarcacaoRetornoAlmoco(apontamento.RetornoAlmoco);
            }

            if (apontamento != null && apontamento.Saida != null) CarregarMarcacaoSaida(apontamento.Saida);

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        #region Metodos Saída

        public void Saida()
        {
            if (txtSaida.Text != ":")
            {
                if (Validation.ValidarHoraValida(txtSaida.Text))
                {
                    EfetuaMarcacaoSaida(txtSaida.Text);
                }
            }
        }
        private async void EfetuaMarcacaoSaida(string saida)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.Saida = saida;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoSaida))
            {
                CarregarMarcacaoSaida(saida);
            }
        }
        private void CarregarMarcacaoSaida(string saida)
        {
            checkSaida.Visible = true;

            CancelSaida.Visible = true;

            btnSaida.Visible = false;

            lblMarcacaSaida.Text = saida;

            lblMarcacaSaida.Visible = true;

            txtSaida.Visible = false;

            btnRelogioSaida.Visible = false;
        }

        #endregion

        #region Metodos Retorno Almoço

        public void RetornoAlmoco()
        {
            if (txtRetornoAlmoco.Text != ":")
            {
                if (Validation.ValidarHoraValida(txtRetornoAlmoco.Text))
                {
                    EfetuaMarcacaoRetornoAlmoco(txtRetornoAlmoco.Text);
                }
            }
        }

        private void CarregarMarcacaoRetornoAlmoco(string retornoAlmoco)
        {
            checkRetornoAlmoco.Visible = true;

            CancelRetornoAlmoco.Visible = true;

            btnRetornoAlmoco.Visible = false;

            lblMarcacaoRetornoAlmoco.Text = retornoAlmoco;

            lblMarcacaoRetornoAlmoco.Visible = true;

            txtRetornoAlmoco.Visible = false;

            btnRelogioRetornoAlmoco.Visible = false;
        }

        private async void EfetuaMarcacaoRetornoAlmoco(string retornoAlmoco)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.RetornoAlmoco = retornoAlmoco;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoRetornoAlmoco))
            {
                CarregarMarcacaoRetornoAlmoco(retornoAlmoco);
            }
        }
        #endregion


        #region Ações Entrada

        private void btnRelogioEntrada_Click(object sender, EventArgs e)
        {
            txtEntrada.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            if (txtEntrada.Text != ":")
            {
                if (Validation.ValidarHoraValida(txtEntrada.Text))
                {
                    EfetuaMarcacaoEntrada(txtEntrada.Text);
                }
            }
        }

        private async void EfetuaMarcacaoEntrada(string entrada)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.Entrada = entrada;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoEntrada))
            {
                CarregarMarcacaoEntrada(entrada);
            }
        }

        private void CancelEntrada_Click(object sender, EventArgs e)
        {
            txtEntrada.Visible = true;

            txtEntrada.Text = "";

            lblMarcacaoEntrada.Visible = false;

            checkEntrada.Visible = false;

            CancelEntrada.Visible = false;

            btnEntrada.Visible = true;

            btnRelogioEntrada.Visible = true;
        }

        #endregion

        #region Ações Saida Almoço

        private void btnRelogioSaidaAlmoco_Click(object sender, EventArgs e)
        {
            txtSaidaAlmoco.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnSaidaAlmoco_Click(object sender, EventArgs e)
        {
            if (txtSaidaAlmoco.Text != ":")
            {
                if (Validation.ValidarHoraValida(txtSaidaAlmoco.Text))
                {
                    EfetuaMarcacaoSaidaAlmoco(txtSaidaAlmoco.Text);
                    TempoTranscorrido(txtEntrada.Text, txtSaidaAlmoco.Text, "");
                }
                MetroMessageBox.Show(null, "Horarío ínvalido", "Formato de hora invalido", MessageBoxButtons.OK);
            }
        }
        private async void EfetuaMarcacaoSaidaAlmoco(string saidaAlmoco)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.SaidaAlmoco = saidaAlmoco;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoSaidaAlmoco))
            {
                CarregarMarcacaoSaidaAlmoco(saidaAlmoco);
            }
        }

        private void CancelSaidaAlmoco_Click(object sender, EventArgs e)
        {
            txtSaidaAlmoco.Visible = true;

            txtSaidaAlmoco.Text = "";

            lblMarcacaoSaidaAlmoco.Visible = false;

            checkSaidaAlmoco.Visible = false;

            CancelSaidaAlmoco.Visible = false;

            btnSaidaAlmoco.Visible = true;

            btnRelogioSaidaAlmoco.Visible = true;
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

            btnRelogioRetornoAlmoco.Visible = true;
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

            btnRelogioSaida.Visible = true;
        }

        #endregion

        #region Metodos Entrada

        private void CarregarMarcacaoEntrada(string entrada)
        {
            checkEntrada.Visible = true;

            CancelEntrada.Visible = true;

            btnEntrada.Visible = false;

            lblMarcacaoEntrada.Text = entrada;

            lblMarcacaoEntrada.Visible = true;

            txtEntrada.Visible = false;

            btnRelogioEntrada.Visible = false;
        }

        #endregion

        #region Metodos Saida Almoço


        private void CarregarMarcacaoSaidaAlmoco(string saidaAlmoco)
        {
            checkSaidaAlmoco.Visible = true;

            CancelSaidaAlmoco.Visible = true;

            btnSaidaAlmoco.Visible = false;

            lblMarcacaoSaidaAlmoco.Text = saidaAlmoco;

            lblMarcacaoSaidaAlmoco.Visible = true;

            txtSaidaAlmoco.Visible = false;

            btnRelogioSaidaAlmoco.Visible = false;
        }

        #endregion

        private void TempoTranscorrido(string entrada, string saidaAlmoco, string retornoAlmoco)
        {
            TimeSpan totalHorasDia = new TimeSpan(9, 0, 0);

            int horaEntrada = Convert.ToInt32(entrada.Substring(0, 2));
            int minutosEntrada = Convert.ToInt32(entrada.Substring(3, 2));

            TimeSpan horaEntradaFinal = new TimeSpan(horaEntrada, minutosEntrada, 0);

            int horaSaidaAlmoco = Convert.ToInt32(saidaAlmoco.Substring(0, 2));
            int minutosSaidaAlmoco = Convert.ToInt32(saidaAlmoco.Substring(3, 2));


            int horaRetornoAlmoco = Convert.ToInt32(retornoAlmoco.Substring(0, 2));
            int minutosRetornoAlmoco = Convert.ToInt32(retornoAlmoco.Substring(3, 2));

            TimeSpan horaRetornoAlmocoFinal = new TimeSpan(horaRetornoAlmoco, minutosRetornoAlmoco, 0);


            TimeSpan horaSaidaAlmocoFinal = new TimeSpan(horaSaidaAlmoco, minutosSaidaAlmoco, 0);

            TimeSpan totalTempoTranscorrido = horaEntradaFinal - horaSaidaAlmocoFinal;

            TimeSpan totalTempoRestante = (totalHorasDia + totalTempoTranscorrido);

            lblTotalTranscorrido.Text = totalTempoTranscorrido.ToString().Replace("-", "");

            lblTempoRestante.Text = totalTempoRestante.ToString();


            TimeSpan totalHoraMinimaSaida = (horaRetornoAlmocoFinal + totalTempoRestante);

            lblHoraMinimaSaida.Text = totalHoraMinimaSaida.ToString();

        }
    }
}