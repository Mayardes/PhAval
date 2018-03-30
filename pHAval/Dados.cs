using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pHAval
{
    public class Dados
    {
        private int idColeta { get; set; }
        public string ValorPH { get; set; }
        public string Email { get; set; }



        public void somatorioIdColeta(int valor)
        {
            this.idColeta += valor;
        }

        public int totalIdColeta()
        {
            return this.idColeta;
        }
    }
}
