using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{
    public class Rule
    {
        int numberOfRule; // decimal
        public int intBin; // binary



        public Rule(int n)
        {
            this.numberOfRule = n;
            int rule = numberOfRule;
            String strBin = Convert.ToString(rule, 2);
            intBin = Convert.ToInt32(strBin);



            

        }
    }
}
