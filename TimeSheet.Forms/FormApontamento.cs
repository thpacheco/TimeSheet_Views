using System;
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
        public static string IdUsuario;
        private readonly IMarcacao _marcacaoAtividades = new MarcacaoAtividades();
        private readonly IMarcacao _marcacaoEntrada = new MarcacaoEntrada();
        private readonly IMarcacao _marcacaoRetornoAlmoco = new MarcacaoRetornoAlmoco();
        private readonly IMarcacao _marcacaoSaida = new MarcacaoSaida();
        private readonly IMarcacao _marcacaoSaidaAlmoco = new MarcacaoSaidaAlmoco();
        private readonly EfetuaMarcacao _realizaMarcacao = new EfetuaMarcacao();
        public readonly ApontamentoService ApontamentoService = new ApontamentoService();
        public Apontamento Apontamento;
        public Marcacao Marcacao;
        public readonly FormLogin _formUsuario = new FormLogin();

        public FormApontamento()
        {
            InitializeComponent();
            CarregarInformacaoUsuario();
            VerificaJaMarcadoNoDia();
        }

        public void ObterIdUsuario(string idUsuario)
        {
            IdUsuario = idUsuario;
        }

        private void CarregarInformacaoUsuario()
        {
            lblAnoRef.Text = DateTime.Now.Date.ToString("yyyy");
            lblMesRef.Text = DateTime.Now.Date.ToString("MM");
        }

        #region Metodos Adicionais

        private string RetornaTotalHorasTrabalhada()
        {
            var entrada = ConverteHoras.ConverterHoraMinutos(txtEntrada.Text);
            var saidaAlmoco = ConverteHoras.ConverterHoraMinutos(txtSaidaAlmoco.Text);

            var totalHorasManha = entrada - saidaAlmoco;

            var retornoAlmoco = ConverteHoras.ConverterHoraMinutos(txtRetornoAlmoco.Text);
            var saida = ConverteHoras.ConverterHoraMinutos(txtSaida.Text);

            var totalHorasTarde = retornoAlmoco - saida;

            return (totalHorasTarde + totalHorasManha).ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void TempoTranscorrido(string entrada, string saidaAlmoco)
        {
            var totalHorasDia = new TimeSpan(9, 0, 0);

            var horaEntrada = Convert.ToInt32(entrada.Substring(0, 2));
            var minutosEntrada = Convert.ToInt32(entrada.Substring(3, 2));
            var horaEntradaFinal = new TimeSpan(horaEntrada, minutosEntrada, 0);

            var horaSaidaAlmoco = Convert.ToInt32(saidaAlmoco.Substring(0, 2));
            var minutosSaidaAlmoco = Convert.ToInt32(saidaAlmoco.Substring(3, 2));
            var horaSaidaAlmocoFinal = new TimeSpan(horaSaidaAlmoco, minutosSaidaAlmoco, 0);

            var totalTempoTranscorrido = horaEntradaFinal - horaSaidaAlmocoFinal;

            var totalTempoRestante = totalHorasDia + totalTempoTranscorrido;

            lblTotalTranscorrido.Text = totalTempoTranscorrido.ToString().Replace("-", "");

            lblTempoRestante.Text = totalTempoRestante.ToString();
        }

        private void CalcularTotalDiferencashoras(string saida)
        {
            var horaSaida = Convert.ToInt32(saida.Substring(0, 2));
            var minutosSaida = Convert.ToInt32(saida.Substring(3, 2));
            var horaSaidaFinal = new TimeSpan(horaSaida, minutosSaida, 0);

            lblHorasDeverAver.Text = (horaSaidaFinal - RetornarHoraMinimaSaida()).ToString();
        }

        #endregion
        private async void VerificaJaMarcadoNoDia()
        {
            progressMarcacao.Visible = true;
            PainelMarcacao.Visible = false;
            var dataMarcacao = DateTime.Now.Date.ToString("dd/MM/yyyy");
            var apontamento = await ApontamentoService.BuscarMarcacaoNoDia(IdUsuario, dataMarcacao);

            if (apontamento != null && apontamento.Entrada != null) CarregarMarcacaoEntrada(apontamento.Entrada);

            if (apontamento != null && apontamento.SaidaAlmoco != null)
            {
                CarregarMarcacaoSaidaAlmoco(apontamento.SaidaAlmoco);
                TempoTranscorrido(apontamento.Entrada, apontamento.SaidaAlmoco);
            }
            if (apontamento != null && apontamento.RetornoAlmoco != null)
            {
                CarregarMarcacaoRetornoAlmoco(apontamento.RetornoAlmoco);
                lblHoraMinimaSaida.Text = RetornarHoraMinimaSaida().ToString();
            }

            if (apontamento != null && apontamento.Saida != null)
            {
                CarregarMarcacaoSaida(apontamento.Saida);
                CalcularTotalDiferencashoras(apontamento.Saida);
            }

            CarregarAtividades(apontamento);
            progressMarcacao.Visible = false;
            PainelMarcacao.Visible = true;
        }

        private void CarregarAtividades(Apontamento apontamento)
        {
            if (apontamento != null)
            {
                txtDescricao.Text = apontamento.DescricaoAtividade;
                txtCodigoAtividade.Text = apontamento.CodigoAtividade;
            }
        }

        private void CalcularHoraMinimaSaida(string retornoAlmoco)
        {
            var entrada = txtEntrada.Text;
            var saidaAlmoco = txtSaidaAlmoco.Text;

            var totalHorasDia = new TimeSpan(9, 0, 0);

            var horaEntradaFinal = ConverteHoras.ConverterHoraMinutos(entrada);

            var horaSaidaAlmocoFinal = ConverteHoras.ConverterHoraMinutos(saidaAlmoco);

            var horaRetornoAlmocoFinal = ConverteHoras.ConverterHoraMinutos(retornoAlmoco);

            var totalTempoTranscorrido = horaEntradaFinal - horaSaidaAlmocoFinal;

            var totalTempoRestante = totalHorasDia + totalTempoTranscorrido;

            var horaMinimaSaida = horaRetornoAlmocoFinal + totalTempoRestante;

            lblHoraMinimaSaida.Text = horaMinimaSaida.ToString();
        }

        private async void SalvarAtividades()
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.DescricaoAtividade = txtDescricao.Text;
            Apontamento.CodigoAtividade = txtCodigoAtividade.Text;

            Marcacao = new Marcacao(Apontamento);

            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoAtividades))
            {
                MetroMessageBox.Show(this, "Atividades Salvas com sucesso.", "Sucesso", MessageBoxButtons.OK);
                LimparCampos();
            }
        }

        #region Metodos Entrada

        private void CarregarMarcacaoEntrada(string entrada)
        {
            checkEntrada.Visible = true;

            CancelEntrada.Visible = true;

            btnEntrada.Visible = false;

            txtEntrada.Text = entrada;

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

            txtSaidaAlmoco.Text = saidaAlmoco;

            lblMarcacaoSaidaAlmoco.Text = saidaAlmoco;

            lblMarcacaoSaidaAlmoco.Visible = true;

            txtSaidaAlmoco.Visible = false;

            btnRelogioSaidaAlmoco.Visible = false;
        }

        #endregion

        private void btnSalvarAtividades_Click(object sender, EventArgs e)
        {
            SalvarAtividades_E_CodigoChamado();
        }

        #region Metodos Saída

        public void Saida()
        {
            if (txtSaida.Text != ":")
                if (Validation.ValidarHoraValida(txtSaida.Text))
                {
                    EfetuaMarcacaoSaida(txtSaida.Text);
                    CalcularTotalDiferencashoras(txtSaida.Text);
                }
        }

        private void SalvarAtividades_E_CodigoChamado()
        {
            if (ValidarAtividades())
                SalvarAtividades();
            else
                MetroMessageBox.Show(this, "Descrição Atividades e código chamado de ser obrigatório.",
                    "Campos Obrigatórios", MessageBoxButtons.OK);
        }



        private void LimparCampos()
        {
            txtDescricao.Text = "";
            txtCodigoAtividade.Text = "";
        }

        private bool ValidarAtividades()
        {
            if (txtDescricao.Text == "") return false;

            if (txtCodigoAtividade.Text == "") return false;

            return true;
        }



        private TimeSpan RetornarHoraMinimaSaida()
        {
            TimeSpan horaMinimaSaida = new TimeSpan();

            if (txtEntrada.Text == "" || txtSaidaAlmoco.Text == "")
            {
                var entrada = txtEntrada.Text;
                var saidaAlmoco = txtSaidaAlmoco.Text;
                var retornoAlmoco = txtRetornoAlmoco.Text;

                var totalHorasDia = new TimeSpan(9, 0, 0);

                var horaEntradaFinal = ConverteHoras.ConverterHoraMinutos(entrada);

                var horaSaidaAlmocoFinal = ConverteHoras.ConverterHoraMinutos(saidaAlmoco);

                var horaRetornoAlmocoFinal = ConverteHoras.ConverterHoraMinutos(retornoAlmoco);

                var totalTempoTranscorrido = horaEntradaFinal - horaSaidaAlmocoFinal;

                var totalTempoRestante = totalHorasDia + totalTempoTranscorrido;

                horaMinimaSaida = horaRetornoAlmocoFinal + totalTempoRestante;
            }
            return horaMinimaSaida;
        }

        private async void EfetuaMarcacaoSaida(string saida)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.Saida = saida;
            Apontamento.HorasTotalDia = RetornaTotalHorasTrabalhada();

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoSaida))
                CarregarMarcacaoSaida(saida);
        }

        private void CarregarMarcacaoSaida(string saida)
        {
            checkSaida.Visible = true;

            CancelSaida.Visible = true;

            btnSaida.Visible = false;

            txtSaida.Text = saida;

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
                if (Validation.ValidarHoraValida(txtRetornoAlmoco.Text))
                {
                    EfetuaMarcacaoRetornoAlmoco(txtRetornoAlmoco.Text);
                    CalcularHoraMinimaSaida(txtRetornoAlmoco.Text);
                }
        }

        private void CarregarMarcacaoRetornoAlmoco(string retornoAlmoco)
        {
            checkRetornoAlmoco.Visible = true;

            CancelRetornoAlmoco.Visible = true;

            btnRetornoAlmoco.Visible = false;

            txtRetornoAlmoco.Text = retornoAlmoco;

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
                CarregarMarcacaoRetornoAlmoco(retornoAlmoco);
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
                if (Validation.ValidarHoraValida(txtEntrada.Text))
                    EfetuaMarcacaoEntrada(txtEntrada.Text);
        }

        private async void EfetuaMarcacaoEntrada(string entrada)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.Entrada = entrada;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoEntrada))
                CarregarMarcacaoEntrada(entrada);
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
                    TempoTranscorrido(txtEntrada.Text, txtSaidaAlmoco.Text);
                }
                MetroMessageBox.Show(this, "Horarío ínvalido", "Formato de hora invalido", MessageBoxButtons.OK);
            }
        }

        private async void EfetuaMarcacaoSaidaAlmoco(string saidaAlmoco)
        {
            Apontamento = new Apontamento();
            Apontamento.IdUsuario = IdUsuario;
            Apontamento.SaidaAlmoco = saidaAlmoco;

            Marcacao = new Marcacao(Apontamento);


            if (await _realizaMarcacao.RealizaMarcacao(Marcacao, _marcacaoSaidaAlmoco))
                CarregarMarcacaoSaidaAlmoco(saidaAlmoco);
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
    }
}