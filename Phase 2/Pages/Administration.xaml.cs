using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Phase_2.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class Administration : UserControl, IContent
    {
        //variable to indicate if the user made a change to the datagrid and did not save it.
        bool isChanged = false;

        //Data binding
        ContractorDataSet contractorDataSet;
        ContractorDataSetTableAdapters.ContractorTableAdapter contractorDataSetTableAdapter;

        public Administration()
        {
            InitializeComponent();
            //Ignore design mode
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                contractorDataSet = (ContractorDataSet)(this.FindResource("contractorDataSet"));
                contractorDataSetTableAdapter = new ContractorDataSetTableAdapters.ContractorTableAdapter();
            }


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Ignore design mode
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                var window = Window.GetWindow(this);
                window.Closing += Window_Closing;
                RefreshGrid();
            }

        }

        //Need to add and remove the grid to refresh the grid properly 
        //otherwise the row to add the new record isn't displayed if there
        //was an error.
        private void RefreshGrid()
        {
            try
            {
                wrapperGrid.Children.Remove(contractorDataGrid);
                wrapperGrid.Children.Add(contractorDataGrid);
                contractorDataSetTableAdapter.Fill(contractorDataSet.Contractor);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage("Error loading data. " + ex.Message, "Warning", MessageBoxButton.OK, true);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //cancel closing window if user doesn't want to navigate away
            e.Cancel = CheckBeforeNavigating();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < contractorDataGrid.Items.Count; i++)
            {
                var row = ((DataGridRow)contractorDataGrid.ItemContainerGenerator.ContainerFromIndex(i));

                if (Validation.GetHasError(row)
)
                {
                    ModernDialog.ShowMessage("Failed to save changes.", "Warning", MessageBoxButton.OK, true);
                    RefreshGrid();
                    return;
                }
            }
            try
            {
                contractorDataSetTableAdapter.Update(contractorDataSet.Contractor);
                ModernDialog.ShowMessage("Your changes have been saved successfully.", "Confirmation", MessageBoxButton.OK, false);
                isChanged = false;
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage("Failed to save changes.", "Warning", MessageBoxButton.OK, true);
            }
            finally
            {
                RefreshGrid();
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //cancel navigation if user doesn't want to navigate away
            e.Cancel = CheckBeforeNavigating();
        }

        //Check before navigating away to let the user confirm if they have any unsaved changes
        private bool CheckBeforeNavigating()
        {
            if (isChanged)
            {
                if (ModernDialog.ShowMessage("Your changes will not be saved.", "Warning", MessageBoxButton.OKCancel, true) == MessageBoxResult.Cancel)
                {
                    return true;
                }
                else
                {
                    isChanged = false;
                    return false;
                }
            }

            return false;
        }

        private void contractorDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            isChanged = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (contractorDataGrid.SelectedItem != null && contractorDataGrid.SelectedItem != CollectionView.NewItemPlaceholder)
            {
                try
                {
                    DataRowView dataRow = (DataRowView)contractorDataGrid.SelectedItem; //dataRow holds the selection
                    dataRow.Delete();
                    ModernDialog.ShowMessage("Information deleted. Please click the Submit Changes button to save your action.", "Confirmation", MessageBoxButton.OK, false);
                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage("Error deleting data.", "Warning", MessageBoxButton.OK, true);
                }

                isChanged = true;
            }
            else
            {
                ModernDialog.ShowMessage("Please select a row to delete.", "Warning", MessageBoxButton.OK, true);
            }
        }

    }
}
