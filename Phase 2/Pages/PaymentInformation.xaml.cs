using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phase_2.Pages
{

    public partial class PaymentInformation : UserControl
    {
        Contractor contractor;
        Payment payment;

        public PaymentInformation()
        {
            InitializeComponent();
        }

        public void Hide()
        {
            txtContractorInfo.Visibility = Visibility.Hidden;
            pnlContractorInfo.Visibility = Visibility.Hidden;
            pnlWorkHour.Visibility = Visibility.Hidden;
            pnlResultLabels.Visibility = Visibility.Hidden;
            pnlResult.Visibility = Visibility.Hidden;
            sp1.Visibility = Visibility.Hidden;
            txtSearchedIrd.Text = "";
            txtHoursWorked.Text = "";
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ContractorDataSet contractorDataSet = new ContractorDataSet();
            ContractorDataSetTableAdapters.ContractorTableAdapter contractorDataSetTableAdapter = new ContractorDataSetTableAdapters.ContractorTableAdapter();
            try { 
                contractorDataSetTableAdapter.Fill(contractorDataSet.Contractor);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, true);
            }

            int searchedIrd = 0;
            if (int.TryParse(txtSearchedIrd.Text, out searchedIrd) == false)
            {
                Hide();
                ModernDialog.ShowMessage("Please enter a correct IRD number. It only consists 8 of numbers.", "Warning", MessageBoxButton.OK, true);
                return;
            }

            var searchResults = contractorDataSet.Contractor.Where(c => c.IRD == Convert.ToInt32(txtSearchedIrd.Text));

            if (searchResults.Count() == 1)
            {
                contractor = new Contractor(
                   searchResults.ElementAt(0).FirstName,
                   searchResults.ElementAt(0).LastName,
                   searchResults.ElementAt(0).IRD,
                   searchResults.ElementAt(0).IsMarried,
                   searchResults.ElementAt(0).NoChildren);

                //Note: Only implementing setting the display labels this way because of 
                //the GetContractorInfo method provided in Contractor class.

                //Temporary variables to hold GetContractorInfo method output
                string fname = "";
                string lname = "";
                int ird = 0;
                bool isMarried = false;
                int noChildren = 0;

                contractor.GetContractorInfo(out fname, out lname, out ird, out isMarried, out noChildren);

                // Setting labels from the temporary variables
                lblFname.Content = fname;
                lblLname.Content = lname;
                txtSearchedIrd.Text = ird.ToString();
                checkIsMarried.IsChecked = isMarried;
                lblChildrenNo.Content = noChildren;
                
                // making first section visible
                txtContractorInfo.Visibility = Visibility.Visible;
                pnlContractorInfo.Visibility = Visibility.Visible;
                pnlWorkHour.Visibility = Visibility.Visible;
                txtHoursWorked.Focus(); //Set focus on next input to help the user.
                sp1.Visibility = Visibility.Visible;

                // making second section invisible and getting it ready for user input
                pnlResultLabels.Visibility = Visibility.Hidden;
                pnlResult.Visibility = Visibility.Hidden;
                txtHoursWorked.Text = "";

                btnCalculate.IsDefault = true;  //Make the next button default to allow easy input.
                btnSearch.IsDefault = false;
            }
            else
            {
                Hide();
                ModernDialog.ShowMessage("Sorry, no match for this IRD number was found.", "Warning", MessageBoxButton.OK, true);
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            int hoursWorked = 0;
            if (int.TryParse(txtHoursWorked.Text, out hoursWorked) == false || hoursWorked < 0)
            {
                ModernDialog.ShowMessage("Please enter a valid number of hours.", "Warning", MessageBoxButton.OK, true);
                txtHoursWorked.Text = "";
                pnlResult.Visibility = Visibility.Hidden;
                pnlResultLabels.Visibility = Visibility.Hidden;
                txtHoursWorked.Focus();  //Set focus on next input to help the user.
                return;
            }

            payment = new Payment(contractor, hoursWorked);

            //formatting the numbers using the currency format.
            lblGST.Content = payment.GetGST().ToString("C");
            lblIncomeTax.Content = payment.GetIncomeTax().ToString("C");
            lblMembership.Content = payment.GetMembrshipDeduction().ToString("C");
            lblTotal.Content = payment.GetTotalDeductions().ToString("C");

            lblNet.Content = payment.GetNetPay().ToString("C");
            lblGross.Content = payment.GetGrossPay().ToString("C");

            pnlResultLabels.Visibility = Visibility.Visible;
            pnlResult.Visibility = Visibility.Visible;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtContractorInfo.Visibility = Visibility.Hidden;
            pnlContractorInfo.Visibility = Visibility.Hidden;
            pnlWorkHour.Visibility = Visibility.Hidden;
            sp1.Visibility = Visibility.Hidden;
            pnlResultLabels.Visibility = Visibility.Hidden;
            pnlResult.Visibility = Visibility.Hidden;
            txtSearchedIrd.Text = "";
        }

        private void loaded_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearchedIrd.Focus();  //Set focus on search to help the user.
        }

        private void txtSearchedIrd_GotFocus(object sender, RoutedEventArgs e)
        {
            //when the search textbox is focused, make the default button the 
            //search button right next to it, to help the user.
            btnCalculate.IsDefault = false;
            btnSearch.IsDefault = true; 
            
        }
    }
}

