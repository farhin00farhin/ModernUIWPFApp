using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Change namespace to your project namespace
namespace Phase_2
{
    class Contractor
    {
        private string firstName, lastName;
        private int IRD, noChildren;
        private bool isMarried;
        private int noDependents;

        public Contractor(string _firstName, string _lastName, int _IRD, bool _isMarried, int _noChildren)
        {
            firstName = _firstName;
            lastName = _lastName;
            IRD = _IRD;
            isMarried = _isMarried;
            noChildren = _noChildren;

            //caclulating the number of dependents
            if (isMarried)
                noDependents = noChildren + 1;
            else
                noDependents = noChildren;
        }

        public void GetContractorInfo(out string _firstName, out string _lastName, out int _IRD, out bool _isMarried, out int _noChildren)
        {
            _firstName = firstName;
            _lastName = lastName;
            _IRD = IRD;
            _isMarried = isMarried;
            _noChildren = noChildren;
        }

        public void GetContractorInfo(out string _firstName, out string _lastName, out int _IRD)
        {
            _firstName = firstName;
            _lastName = lastName;
            _IRD = IRD;
        }
        public int GetNumberOfDependents()
        {
            return noDependents;
        }
    }
}
