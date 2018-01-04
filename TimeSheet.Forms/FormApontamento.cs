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
using TimeSheet.Forms.Controller.MarcacaoController;
using TimeSheet.Forms.Enum;
using TimeSheet.Forms.Models;
using TimeSheet.Forms.Service;

namespace TimeSheet.Forms
{
    public partial class FormApontamento : MetroForm
    {
        private readonly IMarcacao _marcacaoEntrada = new MarcacaoEntrada();
        private readonly ApontamentoService _marcacaoService = new ApontamentoService();
        private readonly EfetuaMarcacao _realizaMarcacao = new EfetuaMarcacao();
        public Marcacao _marcacao;
        public Apontamento _apontamento;
        public static string _IdUsuario = "5a4bcf4f7a0052364c68617f";

        public FormApontamento()
        {
            InitializeComponent();
            VerificaJaMarcadoNoDia();
        }
        private void VerificaJaMarcadoNoDia()
        {
            Task<Apontamento> apontamento = _marcacaoService.BuscarMarcacaoNoDia(_IdUsuario, DateTime.Now.Date.ToString("dd/MM/yyyy"));

            txtEntrada.Text = apontamento.Result.Entrada;
            Entrada();

            txtSaidaAlmoco.Text = apontamento.Result.SaidaAlmoco;
            SaidaAlmoco();

            txtRetornoAlmoco.Text = apontamento.Result.RetornoAlmoco;
            RetornoAlmoco();

            txtSaida.Text = apontamento.Result.Saida;
            Saida();
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
            if (ValidarHoraValida(txtEntrada.Text))
            {
                MarcacaoEntradaEfetuada(txtEntrada.Text);
            }
        }
        public void MarcacaoEntradaEfetuada(string entrada)
        {

            //_apontamento = new Apontamento
            //{
            //    IdUsuario = "5a4bcf4f7a0052364c68617f",
            //    Entrada = entrada
            //};


            //_marcacao = new Marcacao(_apontamento);

            //_realizaMarcacao.RealizaMarcacao(_marcacao, _marcacaoEntrada);


            checkEntrada.Visible = true;

            CancelEntrada.Visible = true;

            btnEntrada.Visible = false;

            lblMarcacaoEntrada.Text = entrada;

            lblMarcacaoEntrada.Visible = true;

            txtEntrada.Visible = false;

            btnRelogioEntrada.Visible = false;
        }

        private void MarcacaoEntrada()
        {
            throw new NotImplementedException();
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

        #region Metodos de validação
        private bool ValidarHoraValida(string hora)
        {
            String strpattern = @"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"; //Pattern is Ok

            Regex regex = new Regex(strpattern);

            return regex.Match(hora).Success;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }

}
