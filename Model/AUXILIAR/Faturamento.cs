using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.AUXILIAR
{
    public class Faturamento
    {
        public int ano;
        public int mes;
        public String mesNome;
        public double valorTotal;
        public List<String> NomeMeses = new List<string> {
            "Janeiro",
            "Fevereiro",
            "Março",
            "Abril",
            "Maio",
            "Junho",
            "Julho",
            "Agosto",
            "Setembro",
            "Outubro",
            "Novembro",
            "Dezembro"
        };

        public Faturamento(int mes) {
            mesNome = NomeMeses.ElementAt(mes-1);
        }
    }
}
