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

namespace MasterPolApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartnerListViewPage.xaml
    /// </summary>
    public partial class PartnerListViewPage : Page
    {
        public PartnerListViewPage()
        {
            InitializeComponent();
            LoadData();
            PartnerListView.ItemsSource = Data.DatabaseMasterPolEntities.GetContext().PartnerImport.ToList();
        }

        private void LoadData()
        {
            var context = Data.DatabaseMasterPolEntities.GetContext();

            var partners = context.PartnerImport.ToList();

            foreach (var partner in partners)
            {
                var totalSales = context.PartnerProductImport
                    .Where(p => p.IdNamePartner == partner.Id)
                    .Select(p => (decimal)p.QuantityProduct)
                    .DefaultIfEmpty(0)
                    .Sum();

                int newDiscount = CalculateDiscount(totalSales);

                if (partner.Discount != newDiscount)
                {
                    partner.Discount = newDiscount;
                }
            }

            context.SaveChanges(); 

            PartnerListView.ItemsSource = partners;
        }
        

        private static int CalculateDiscount(decimal totalSales)
        {
            if (totalSales < 10000)
                return 0;
            if (totalSales >= 10000 && totalSales <= 50000)
                return 5;
            if (totalSales > 50000 && totalSales <= 300000)
                return 10;
            return 15;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage((sender as Button).DataContext as Data.PartnerImport));
        }

        private void DataGridButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.HistoryDataGridPage());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage(null));
        }
    }
}
