using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pHAval
{
    public partial class Form1 : Form
    {

        private Timer time = new Timer();
        string datas = null;
        OpenFileDialog ofd = new OpenFileDialog();
        List<Dados> lista = new List<Dados>();
        Dados dados = new Dados();
        string[] array;

        int x = 2000;
        public Form1()
        {
            InitializeComponent();
            chtGrafico.Visible = false;
            cbEnable3D.Visible = false;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void avaliaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Title = "Localizar arquivo";
            ofd.Filter = "txt files (*.txt)|*.txt";

            datas = null;

            if (DialogResult.OK == ofd.ShowDialog(this))
            {

                if (analiseDosDadosColetados() == true)
                {
                    toolStripStatusLabel1.Text = "Analisado: " + ofd.FileName;
                    toolStripProgressBar1.Visible = false;
                    //MessageBox.Show("" + datas);
                }
            }
        }
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            PrintDialog pd = new PrintDialog();
            if (datas == null || datas == "") 
                MessageBox.Show("Nenhum arquivo carregado!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            else 
            if(DialogResult.OK == pd.ShowDialog(this)){
                //Aqui deverá selecionar o arquivo a ser impresso!
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Para começar, abra um arquivo!";
            toolStripProgressBar1.Visible = false;
        }

        private void InitializeMyTimer(int tempo)
        {
            // Set the interval for the timer.
            time.Interval = 1;
            // Connect the Tick event of the timer to its event handler.
            time.Tick += new EventHandler(IncreaseProgressBar);
            // Start the timer.
            time.Start();
        }


        private void IncreaseProgressBar(object sender, EventArgs e)
        {
            // Increment the value of the ProgressBar a value of one each time.
            toolStripProgressBar1.Increment(1);
            // Display the textual value of the ProgressBar in the StatusBar control's first panel.
            toolStripProgressBar1.Text = toolStripProgressBar1.Value.ToString() + "% Completed";
            // Determine if we have completed by comparing the value of the Value property to the Maximum value.
            if (toolStripProgressBar1.Value == toolStripProgressBar1.Maximum)
            // Stop the timer.
            time.Stop();
    
        }


        public bool analiseDosDadosColetados()
        {
            toolStripStatusLabel1.Text = "";
            datas = File.ReadAllText(ofd.FileName);

            if (datas != "")
            {
                chtGrafico.Visible = false;
                cbEnable3D.Visible = false;

                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Text = "Analisando..";
                InitializeMyTimer(1);
                MessageBox.Show("Analisando " + ofd.SafeFileName, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chtGrafico.Visible = true;
                cbEnable3D.Visible = true;

                //Coletaremos dados do text
                leitura();

                MessageBox.Show("Foram coletadas: " + dados.retornaUltimaColeta() + " amostas","Total amostras");
                MessageBox.Show("Das " + dados.recebeHoraInicio() + " às " + dados.recebeHoraFim(),"Horário");

                impressaoLista();

                return true;
            }
            else
            {
                MessageBox.Show("no data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel1.Text = "Arquivo inválido";
                return false;
            }

            
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sobreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.0.1", "Versão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void leitura()
        {

            //link para ajudar do projeto:https://pablobatistacardoso.wordpress.com/2012/12/15/ler-aquivo-txt-e-armazenar-em-um-list-c/
            //link para o gráfico fixo final: https://www.youtube.com/watch?v=gqo2TGpCOlA

            string filedata = ofd.FileName;
            //Lê todos os dados dentro do arquivo filedata
            array = File.ReadAllLines(@filedata);

           
            for (int i = 0; i < array.Length; i++)
            {
                
                //Uso o método Split e quebro cada linha
                //em um novo array auxiliar, ou seja, cada
                //conteúdo do arquivo txt separado por '|' será
                //um nova linha neste array auxiliar. Assim sei que
                //cada índice representa uma propriedade
                string[] auxiliar = array[i].Split('|');

                //Aqui recupero os itens, atribuindo
                //os mesmo as propriedade da classe
                //Cliente correspondentes, ou seja,
                //o índice zero será corresponde ao Id
                //o um ao nome e o dois ao e-mail
                
                dados.enviaUltimaColeta(Convert.ToInt32(auxiliar[0]));
                dados.enviaValorPh(Convert.ToDouble(auxiliar[1]));
                dados.enviaHoraFim(auxiliar[2]);
               

                //Envia valores para as listas separadamente
                dados.listaIdColeta.Add(Convert.ToInt32(auxiliar[0]));
                dados.listaDePhs.Add(Convert.ToDouble(auxiliar[1]));
                dados.listaDeData.Add(auxiliar[2]);
                dados.listaDeTemperaturas.Add(Convert.ToInt32(auxiliar[3]));


                //Contagem das amostas individualmente
                if(dados.listaDePhs[i] >= 800)
                {
                    dados.enviaAlcalino(1);
                }else
                    if(dados.listaDePhs[i] >=700 && dados.listaDePhs[i] < 800)
                {
                    dados.enviaNeutra(1);
                }else
                    if(dados.listaDePhs[i] < 700)
                {
                    dados.enviaAcida(1);
                }

                //Envia o inicio da hora da coleta
                dados.enviaHoraInicio(dados.listaDeData[0]);

                lista.Add(dados);
            }
        }

        public void grafico()
        {
            //Não exibirá legendas
            chtGrafico.Legends.Clear();

            chtGrafico.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;


        }

        private void chtGrafico_Click(object sender, EventArgs e)
        {

        }

        private void cbEnable3D_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnable3D.Checked)
                chtGrafico.ChartAreas[0].Area3DStyle.Enable3D = true;
            else
                chtGrafico.ChartAreas[0].Area3DStyle.Enable3D = false;
        }

        private void timerGrafico_Tick(object sender, EventArgs e)
        {
            if(chtGrafico.Series[0].Points.Count > 5)
            {
                chtGrafico.Series[0].Points.RemoveAt(0);
                chtGrafico.Update();
            }
          chtGrafico.Series[0].Points.AddXY(x++, dados.recebeValorMedioPh());
        }



        //Realiza a impressão para referência
        public void impressaoLista()
        {
            for (int i = 0; i < array.Length; i++)
            {
                string[] auxiliar = array[i].Split('|');
                MessageBox.Show("ID: " + dados.listaIdColeta[i] + " Ph: " + dados.listaDePhs[i] + " Hora: " + dados.listaDeData[i] + " Temp: " + dados.listaDeTemperaturas[i] + "°");
            }

            MessageBox.Show("Quantidade Alcalinas: " + dados.recebeAlcalina() + " Quantidade Acidas: " + dados.recebeAcida() + " Quantidade Neutras: " + dados.recebeNeutra());
        }


        public void graficoFinal()
        {

        }



    }
}
