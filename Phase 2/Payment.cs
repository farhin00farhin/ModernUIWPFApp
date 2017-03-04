using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Phase_2
{
    class Payment
    {
        private Contractor theContractor;
      private int noOfHoursWorked;
        private double grossPay;
        private const int hourlyRate = 75;
        private const double membershipRate = 0.13;

        public Payment( Contractor _theContractor, int _noOfHoursWoked )
        {
            theContractor=_theContractor;
            noOfHoursWorked = _noOfHoursWoked;
            grossPay = (double)noOfHoursWorked * hourlyRate;
        }

        public double GetGST()
        {
            return 0.0758 * grossPay;
        }

        public double GetIncomeTax()
        {
            return 0.23 * (grossPay - (grossPay * 0.0575 * theContractor.GetNumberOfDependents()));
        }
        public double GetMembrshipDeduction()
        {
            return grossPay * membershipRate;
        }

        public double GetTotalDeductions()
        {
            double GST = GetGST();
            double IRTax = GetIncomeTax();
            double membershipDeduction = GetMembrshipDeduction();
            return GST + IRTax + membershipDeduction;
        }

        public double GetNetPay()
        {
            return grossPay - GetTotalDeductions();
        }

        public double GetGrossPay()
        {
            return grossPay;
        }
    }
}
