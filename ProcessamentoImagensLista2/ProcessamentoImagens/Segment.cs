using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessamentoImagens
{
    internal class Segment
    {
        public List<Ponto> segment {  get; set; }

        public Segment() {
            segment = new List<Ponto>();
        }
    }
}
