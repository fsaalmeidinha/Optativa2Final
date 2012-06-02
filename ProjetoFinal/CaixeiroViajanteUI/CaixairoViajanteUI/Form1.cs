using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using Utils.Model;
using AGCaixeiroViajantePMX.Model;

namespace CaixairoViajanteUI
{
    public partial class Form1 : Form
    {
        List<Agencia> agencias = null;
        Dictionary<int, string> bancos = new Dictionary<int, string>();
        double latitude = 0;
        double longitude = 0;

        //List<Agencia> agenciasSelecionadas = new List<Agencia>();
        public Form1()
        {
            InitializeComponent();
            agencias = Agencia.PegarTodas();
            ExibirBancos();
        }

        private void ExibirBancos()
        {
            int idbanco = 1;
            int top = pnlBancos.Top - 60;
            int leftTextBox = pnlBancos.Left;
            int leftLabel = pnlBancos.Left + 40;

            foreach (var banco in agencias.Select(ag => ag.Nome).Distinct())
            {
                bancos.Add(idbanco++, banco);
                TextBox txtQtdbanco = new TextBox();
                txtQtdbanco.Name = "txt" + banco.Trim();
                txtQtdbanco.Width = 25;
                txtQtdbanco.Top = top;
                txtQtdbanco.Left = leftTextBox;
                pnlBancos.Controls.Add(txtQtdbanco);

                Label lblNomeBanco = new Label();
                lblNomeBanco.Text = banco;
                lblNomeBanco.Top = top + 3;
                lblNomeBanco.Left = leftLabel;
                lblNomeBanco.Width = 150;
                pnlBancos.Controls.Add(lblNomeBanco);
                top += 30;
            }

        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            string endereco = txtEndereco.Text;
            if (endereco.Length == 8)
            {
                int cep = 0;
                Int32.TryParse(endereco, out cep);
                if (cep != 0)
                {
                    endereco = endereco.Insert(5, "-");
                }
            }

            Coordenadas cord = GoogleGeoCode.GetCoordenadas(endereco);
            latitude = (double)cord.Latitude;
            longitude = (double)cord.Longitude;

            ReconhecimentoZonaRN.ReconhecimentoZonaRN reconhecimentoZonaRN = new ReconhecimentoZonaRN.ReconhecimentoZonaRN();
            string zonaRN = reconhecimentoZonaRN.IdentificarZona((double)cord.Longitude, (double)cord.Latitude);
            lblZona.Text = "O enrereço foi identificado na(o) " + zonaRN;

            foreach (Agencia agencia in agencias)
            {
                agencia.Distancia = Utils.Distancia.CalcularDistanciaKM((double)cord.Latitude, (double)cord.Longitude, agencia.Latitude, agencia.Longitude);
            }

            pnlBancos.Enabled = true;
        }

        private void txtEndereco_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEndereco.Text))
            {
                btnLocalizar.Visible = true;
            }
            else
            {
                btnLocalizar.Visible = false;
            }
        }

        private void btnGerarRota_Click(object sender, EventArgs e)
        {
            try
            {
                txtRota.Text = "Processando...";

                Dictionary<string, int> qtdAgenciasPorBanco = new Dictionary<string, int>();
                foreach (KeyValuePair<int, string> banco in bancos)
                {
                    string txtbanco = ((TextBox)pnlBancos.Controls.Find("txt" + banco.Value.Trim(), true).First()).Text;
                    int qtd = 0;
                    if (Int32.TryParse(txtbanco, out qtd) && qtd > 0)
                    {
                        qtdAgenciasPorBanco.Add(banco.Value, qtd);
                    }
                }
                //qtdAgenciasPorBanco.Add("Banco do Brasil", 1);
                //qtdAgenciasPorBanco.Add("Caixa Economica Federal", 1);

                AGCaixeiroViajantePMX.AlgGenetico algGenetico = new AGCaixeiroViajantePMX.AlgGenetico(qtdAgenciasPorBanco, latitude, longitude);
                List<Rota> rota = algGenetico.RecuperarRotaAgencias();

                StringBuilder roteiro = new StringBuilder("Voce deve:");
                foreach (Rota rotaAtual in rota)
                {
                    roteiro.AppendFormat("\r\n ir a \"{0}\" que fica na latitude: {1} e longitude: {2}, percorrendo {3} KM,\r\n", rotaAtual.Destino, rotaAtual.Latitude, rotaAtual.Longitude, rotaAtual.Distancia);
                }

                txtRota.Text = roteiro.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (txtRota.Text == "Processando...")
                {
                    txtRota.Text = "";
                }
            }
        }

        private void btnGerarRota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                txtRota.Text = "Processando...";
        }

        private void btnGerarRota_MouseDown(object sender, MouseEventArgs e)
        {
            txtRota.Text = "Processando...";
        }

        //private void txtFiltroAgencias_TextChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(txtFiltroAgencias.Text))
        //    {
        //        btnFiltrarAgencias.Visible = true;
        //    }
        //    else
        //    {
        //        btnFiltrarAgencias.Visible = false;
        //    }
        //}

        //private void btnFiltrarAgencias_Click(object sender, EventArgs e)
        //{
        //    List<Agencia> agenciasFiltradas = agencias.Where(ag => ag.Nome.ToUpper().Contains(txtFiltroAgencias.Text.ToUpper()) && agenciasSelecionadas.All(agSelec => agSelec.Id != ag.Id)).ToList();
        //    lbAgencias.ValueMember = "Id";
        //    lbAgencias.DisplayMember = "Descricao";
        //    lbAgencias.DataSource = agenciasFiltradas.Select(ag => new { ag.Id, Descricao = String.Format("{0} - Zona:{1} - Lat: {2} - Long: {3}", ag.Nome, ag.Zona, ag.Latitude, ag.Longitude) }).ToList();
        //}s
    }
}
