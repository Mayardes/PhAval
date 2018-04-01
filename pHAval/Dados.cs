using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pHAval
{
    public class Dados
    {
        //Lista dados separadamente
        public List<int> listaIdColeta = new List<int>();
        public List<double> listaDePhs = new List<double>();
        public List<string> listaDeData = new List<string>();
        public List<int> listaDeTemperaturas = new List<int>();

        //Conta a quantidade de amostras separadamente
        private int QtsAlcalina { get; set; }
        private int QtsAcida { get; set; }
        private int QtsNeutra { get; set; }

        //Colta dados para visualização
        private int idUltimaColeta { get; set; }
        private double valorMedioPh { get; set; }
        private string horaInicio { get; set; }
        private string horaFim { get; set; }

        public void enviaUltimaColeta(int valor)
        {
            this.idUltimaColeta = valor;
        }
        public int retornaUltimaColeta()
        {
            return this.idUltimaColeta;
        }

        public void enviaValorPh(double valor)
        {
            this.valorMedioPh += valor;
        }

        public double recebeValorMedioPh()
        {
            double medio = this.valorMedioPh;
            int qtsDeColeta = this.idUltimaColeta;
            double media = (medio/qtsDeColeta);
            return media;
        }

        public void enviaHoraInicio(string hora)
        {
            this.horaInicio = hora;
        }

        public string recebeHoraInicio()
        {
            return this.horaInicio;
        }

        public void enviaHoraFim(string hora)
        {
            this.horaFim = hora;
        }

        public string recebeHoraFim()
        {
            return this.horaFim;
        }



        public void enviaAlcalino(int cont)
        {
            this.QtsAlcalina += cont;
        }

        public void enviaAcida(int cont)
        {
            this.QtsAcida += cont;
        }
        public void enviaNeutra(int cont)
        {
            this.QtsNeutra += cont;
        }


        public int recebeAlcalina()
        {
            return this.QtsAlcalina;
        }

        public int recebeAcida()
        {
            return this.QtsAcida;
        }
        public int recebeNeutra()
        {
            return this.QtsNeutra;
        }



        //Metodo que calcula a porcentagem das amostras
        /**
         * 0 - Alcalina
         * 1 - Neutra
         * 2 - Ácida
         * */
        public double retornaPorcentagens(int tipo)
        {
            if (tipo == 0)
            {
                double porcent = ((this.recebeAlcalina() * 100));
                return Math.Round(porcent/this.retornaUltimaColeta(), 2);
            }else
                if(tipo == 1)
            {
                double porcent = ((this.recebeNeutra() * 100));
                return Math.Round(porcent/ this.retornaUltimaColeta(), 2);
            }
            else
            {
                double porcent = ((this.recebeAcida() * 100));
                return Math.Round(porcent / this.retornaUltimaColeta(), 2);
            }

        }



    }
}
