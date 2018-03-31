using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pHAval
{
    public class Dados
    {
        private List<int> listaIdColeta = new List<int>();
        private List<double> listaDePhs = new List<double>();
        private List<DateTime> listaDeData = new List<DateTime>();
        private List<int> listaDeTemperaturas = new List<int>();

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

        public void enviaListaIDsColeta(int valor)
        {
            this.listaIdColeta.Add(valor);
        }

        public List<int> recebeListaDeIDs()
        {
            return this.listaIdColeta;
        }


        
    }
}
