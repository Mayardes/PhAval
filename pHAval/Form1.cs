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
using iTextSharp.text;



//Usando biliotecas para elaboração do PDF
using iTextSharp;//E A BIBLIOTECA ITEXTSHARP E SUAS EXTENÇÕES
using iTextSharp.text.pdf;//ESTENSAO 2 (PDF)
using System.Drawing.Printing;

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
            chtGraficoPizza.Visible = false;

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

               desativaLabelsIniciais();
               dados.zeraTudo();

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
           
            //Abre o arquivo já gerado
            if (datas == null || datas == "")
                MessageBox.Show("Nenhum arquivo carregado!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                //Gera o documento em PDF
                if (GerarRelatorioPDF() == true)
                {

                    //MessageBox.Show("Gerado com sucesso!\n Deseja abrir agora?", "Informações", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    DialogResult resultado = MessageBox.Show("Sucesso!\nDeseja abrir agora?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado.Equals(DialogResult.Yes))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("C:\\PDF_TCC\\Doc.pdf");
                        Process.Start(startInfo);
                    }
                }else
                {
                    MessageBox.Show("Erro","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            desativaLabelsIniciais();
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
                chtGraficoPizza.Visible = false;

                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Text = "Analisando..";
                InitializeMyTimer(1);
                MessageBox.Show("Analisando " + ofd.SafeFileName, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chtGrafico.Visible = true;
                cbEnable3D.Visible = true;

                //Coletaremos dados do text
                leitura();
                analisaMenoreMaiorTemp();

                MessageBox.Show("Foram coletadas: " + dados.retornaUltimaColeta() + " amostas","Total amostras");

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
            MessageBox.Show("1.0.3", "Versão", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dados.listaDePhs[i] >= 800)
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
            chtGrafico.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            chtGrafico.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
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



        //Dados do gráfico de barras para realização da plotagem 
        private void timerGrafico_Tick(object sender, EventArgs e)
        {
            if(chtGrafico.Series[0].Points.Count > 5)
            {
                chtGrafico.Series[0].Points.RemoveAt(0);
                chtGrafico.Series[1].Points.RemoveAt(0);
                chtGrafico.Series[2].Points.RemoveAt(0);
                chtGrafico.Update();
            }
            
            chtGrafico.Series[0].Points.AddXY(x++, dados.recebeAlcalina());
            chtGrafico.Series[1].Points.AddXY(x++, dados.recebeNeutra());
            chtGrafico.Series[2].Points.AddXY(x++, dados.recebeAcida());
           
        }



        //Realiza a impressão para referência
        public void analisaMenoreMaiorTemp()
        {
            dados.listaDeTemperaturas.Sort();
            dados.enviaTempInicial(dados.listaDeTemperaturas[0]);
            dados.enviaTempFinal(dados.listaDeTemperaturas[(dados.retornaUltimaColeta() - 1)]);
        }


        public void graficoFinal()
        {

        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            desativaLabelsIniciais();
            chtGrafico.Visible = true;
            cbEnable3D.Visible = true;
        }

        private void gráficoDePizzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chtGrafico.Visible = false;
            cbEnable3D.Visible = false;
            chtGraficoPizza.Visible = true;
            ativaLabelsIniciais();
            label3.Text = Convert.ToString(dados.retornaUltimaColeta());
            label4.Text = dados.recebeHoraInicio();
            label6.Text = dados.recebeHoraFim();
            label8.Text = Convert.ToString(dados.recebeTempInicial());
            label10.Text = Convert.ToString(dados.recebeTempFinal());



            chtGraficoPizza.Series.Clear();
            chtGraficoPizza.Legends.Clear();

            //Add a new Legend(if needed) and do some formating
            chtGraficoPizza.Legends.Add("MyLegend");
            chtGraficoPizza.Legends[0].Alignment = StringAlignment.Center;
            chtGraficoPizza.Legends[0].Title = "Tabela";
            chtGraficoPizza.Legends[0].BorderColor = Color.Black;

            string seriesname = "Pizza";
            chtGraficoPizza.Series.Add(seriesname);

            chtGraficoPizza.Series[seriesname].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            chtGraficoPizza.Series["Pizza"].Points.AddXY("Alcalina, " + dados.retornaPorcentagens(0) + "%", dados.retornaPorcentagens(0));
            chtGraficoPizza.Series["Pizza"].Points.AddXY("Neutra, " + dados.retornaPorcentagens(1) + "%", dados.retornaPorcentagens(1));
            chtGraficoPizza.Series["Pizza"].Points.AddXY("Ácida, " + dados.retornaPorcentagens(2) + "%", dados.retornaPorcentagens(2));

        }


        public void desativaLabelsIniciais()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
        }
        public void ativaLabelsIniciais()
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        public bool GerarRelatorioPDF()
        {
            //Para ajudar na elaboração do relatório
            //https://www.devmedia.com.br/criando-e-manipulando-arquivos-pdf-com-a-biblioteca-itextsharp-em-c/33392
            //Utilizaremos a biblioteca ItextSharp 


            Document doc = new Document(PageSize.A4);//criando e estipulando o tipo da folha usada
            doc.SetMargins(40, 40, 40, 80);//estibulando o espaçamento das margens que queremos
            doc.AddCreationDate();//adicionando as configuracoes

            //caminho onde sera criado o pdf + nome desejado
            //OBS: o nome sempre deve ser terminado com .pdf
            string caminho = @"C:\PDF_TCC\" + "Doc.pdf";

            //criando o arquivo pdf embranco, passando como parametro a variavel                
            //doc criada acima e a variavel caminho 
            //tambem criada acima.
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
            //Escrevendo no documento PDF
            doc.Open();

            string t = "";



            /**
            * Logo da empresa
            */

            string imageURL = "C:\\PDF_TCC\\logo.jpg";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(140f, 120f);
            //Give space before image
            jpg.SpacingBefore = 1f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);


            /**
             * Cabeçalho do relatório
             * */

            //criando a variavel para paragrafo
            Paragraph titulo = new Paragraph(t, new iTextSharp.text.Font());
            //etipulando o alinhamneto
            titulo.Alignment = Element.ALIGN_CENTER;
            //adicioando texto
            titulo.Add("RELATÓRIO DE ANÁLISE DE COLETA");
            //acidionado paragrafo ao documento
            doc.Add(titulo);

            /**
             * Corpo do relatório
             * */
            DateTime localDate = DateTime.Now;

            Paragraph texto = new Paragraph(t, new iTextSharp.text.Font());
            texto.Alignment = Element.ALIGN_LEFT;
            texto.Add("\n");
            texto.Add("Emissão: " + localDate + "\n");
            texto.Add("Arquivo: " + ofd.SafeFileName);
            texto.Add("\n");
            texto.Add("\nQuantidade de amostras coletadas: " + dados.retornaUltimaColeta() + "\n");
            texto.Add("Média de valores do pH: " + dados.recebeValorMedioPh() + "\n");
            texto.Add("Hora da Coleta: " + dados.recebeHoraInicio() + " até " + dados.recebeHoraFim() + "\n");
            texto.Add("Variação da temperatura: " + dados.recebeTempInicial() +"°C"+ " - " + dados.recebeTempFinal() + "°C"+"\n");
            texto.Add("\n");
            texto.Add("Porcentagens:\n");
            texto.Add("Alcalinidade: " + dados.retornaPorcentagens(0) + "%\n");
            texto.Add("Neutra: " + dados.retornaPorcentagens(1) + "%\n");
            texto.Add("Ácida: " + dados.retornaPorcentagens(2) + "%\n");
            texto.Add("\n");
            texto.Add("Quantidade de amostras individuais:\n");
            texto.Add("Alcalinidade: " + dados.recebeAlcalina() + "\n");
            texto.Add("Neutra: " + dados.recebeNeutra() + "\n");
            texto.Add("Ácida: " + dados.recebeAcida() + "\n");
            doc.Add(texto);


            /**
             * Rodapé do relatório
             */

            Paragraph footer = new Paragraph("Manaus - AM", new iTextSharp.text.Font());
            footer.Alignment = Element.ALIGN_CENTER;

            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.WidthPercentage = 100f;
            footerTbl.TotalWidth = 1000f;
            footerTbl.HorizontalAlignment = 0;
            PdfPCell cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.Colspan = 1;
            cell.PaddingLeft = 0;
            cell.HorizontalAlignment = 0;
            footerTbl.DefaultCell.HorizontalAlignment = 0;
            footerTbl.WidthPercentage = 10;
            footerTbl.AddCell(cell);                                     
            footerTbl.WriteSelectedRows(0, -10, 270, 30, writer.DirectContent);
            doc.Close();

            return true;
        }
    }
}
