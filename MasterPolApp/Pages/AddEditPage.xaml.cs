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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public Data.PartnerImport CurrentPartner = new Data.PartnerImport();
        public string FlagAddOrEdit = "default";
        public AddEditPage(Data.PartnerImport _partner)
        {
            InitializeComponent();
            if(_partner != null)
            {
                CurrentPartner = _partner;
                FlagAddOrEdit = "edit";
            }
            else
            {
                FlagAddOrEdit = "add";
            }

            DataContext = CurrentPartner;
            Init();
        }

        private void Init()
        {
            TypePartnerComboBox.ItemsSource = Data.DatabaseMasterPolEntities.GetContext().TypePartner.ToList();
            if (FlagAddOrEdit == "add")
            {
                NamePartnerTextBox.Text = string.Empty;
                TypePartnerComboBox.SelectedItem = null;
                RatingTextBox.Text = string.Empty;
                IndexTextBox.Text = string.Empty;
                AreoTextBox.Text = string.Empty;
                CityTextBox.Text = string.Empty;
                StreetTextBox.Text = string.Empty;
                HouseTextBox.Text = string.Empty;
                NameDirectorTextBox.Text = string.Empty;
                NumberPhoneTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
            }
            else if (FlagAddOrEdit == "edit")
            {
                NamePartnerTextBox.Text = CurrentPartner.NamePartner;
                TypePartnerComboBox.SelectedItem = Data.DatabaseMasterPolEntities.GetContext().TypePartner
                    .Where(d => d.Id == CurrentPartner.IdTypePartner).FirstOrDefault();
                RatingTextBox.Text = CurrentPartner.Rating.ToString();
                NameDirectorTextBox.Text = CurrentPartner.NameDirector.Director;
                NumberPhoneTextBox.Text = CurrentPartner.NumberPhone;
                EmailTextBox.Text = CurrentPartner.Email;
                IndexTextBox.Text = CurrentPartner.Address.NumberIndex.NameIndex.ToString();
                AreoTextBox.Text = CurrentPartner.Address.NameArea.Area;
                CityTextBox.Text = CurrentPartner.Address.NameCity.City;
                StreetTextBox.Text = CurrentPartner.Address.NameStreet.Street;
                HouseTextBox.Text = CurrentPartner.Address.Home.ToString();

            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(NamePartnerTextBox.Text))
                {
                    errors.AppendLine("Введите наименование партнера!");
                }
                if(TypePartnerComboBox.SelectedItem == null)
                {
                    errors.AppendLine("Вы не выбрали тип партнера!");
                }
                if (string.IsNullOrEmpty(RatingTextBox.Text))
                {
                    errors.AppendLine("Введите рейтинг партнера!");
                }
                else
                {
                    var count = Int32.TryParse(RatingTextBox.Text, out var result);
                    if(!(count && result >= 0))
                    {
                        errors.AppendLine("Рейтинг не может быть отрицательным и должен быть целым!");
                    }
                }
                if (string.IsNullOrEmpty(IndexTextBox.Text))
                {
                    errors.AppendLine("Введите индекс!");
                }
                if (string.IsNullOrEmpty(AreoTextBox.Text))
                {
                    errors.AppendLine("Введите область!");
                }
                if (string.IsNullOrEmpty(CityTextBox.Text))
                {
                    errors.AppendLine("Введите город!");
                }
                if (string.IsNullOrEmpty(StreetTextBox.Text))
                {
                    errors.AppendLine("Введите улицу!");
                }
                if (string.IsNullOrEmpty(HouseTextBox.Text))
                {
                    errors.AppendLine("Введите номер дома!");
                }
                if (string.IsNullOrEmpty(NameDirectorTextBox.Text))
                {
                    errors.AppendLine("Введите ФИО директора!");
                }
                if (string.IsNullOrEmpty(NumberPhoneTextBox.Text))
                {
                    errors.AppendLine("Введите номер телефона!");
                }
                if (string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    errors.AppendLine("Введите почту!");
                }
                if(errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var address = new Data.Address();
                var IndexText = Convert.ToInt32(IndexTextBox.Text);

                var searchStreet = (from item in Data.DatabaseMasterPolEntities.GetContext().NameStreet
                                    where item.Street == StreetTextBox.Text
                                    select item).FirstOrDefault();
                if(searchStreet != null)
                {
                    address.Id = searchStreet.Id;
                }
                else
                {
                    Data.NameStreet nameStreet = new Data.NameStreet()
                    {
                        Street = StreetTextBox.Text
                    };
                    Data.DatabaseMasterPolEntities.GetContext().NameStreet.Add(nameStreet);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    address.IdStreet = nameStreet.Id;
                }

                var searchCity = (from item in Data.DatabaseMasterPolEntities.GetContext().NameCity
                                    where item.City == CityTextBox.Text
                                    select item).FirstOrDefault();
                if (searchCity != null)
                {
                    address.Id = searchCity.Id;
                }
                else
                {
                    Data.NameCity nameCity = new Data.NameCity()
                    {
                        City = CityTextBox.Text
                    };
                    Data.DatabaseMasterPolEntities.GetContext().NameCity.Add(nameCity);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    address.IdCity = nameCity.Id;
                }

                var searchAreo = (from item in Data.DatabaseMasterPolEntities.GetContext().NameArea
                                  where item.Area == AreoTextBox.Text
                                  select item).FirstOrDefault();
                if (searchAreo != null)
                {
                    address.Id = searchAreo.Id;
                }
                else
                {
                    Data.NameArea nameArea = new Data.NameArea()
                    {
                        Area = AreoTextBox.Text
                    };
                    Data.DatabaseMasterPolEntities.GetContext().NameArea.Add(nameArea);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    address.IdArea = nameArea.Id;
                }

                var searchIndex = (from item in Data.DatabaseMasterPolEntities.GetContext().NumberIndex
                                  where item.NameIndex == IndexText
                                  select item).FirstOrDefault();
                if (searchIndex != null)
                {
                    address.Id = searchIndex.Id;
                }
                else
                {
                    Data.NumberIndex numberIndex = new Data.NumberIndex()
                    {
                        NameIndex = IndexText
                    };
                    Data.DatabaseMasterPolEntities.GetContext().NumberIndex.Add(numberIndex);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    address.IdIndex = numberIndex.Id;
                }

                var searchDirector = (from item in Data.DatabaseMasterPolEntities.GetContext().NameDirector
                                   where item.Director == NameDirectorTextBox.Text
                                   select item).FirstOrDefault();
                if (searchDirector != null)
                {
                    address.Id = searchDirector.Id;
                }
                else
                {
                    Data.NameDirector nameDirector = new Data.NameDirector()
                    {
                        Director = NameDirectorTextBox.Text
                    };
                    Data.DatabaseMasterPolEntities.GetContext().NameDirector.Add(nameDirector);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    address.IdStreet = nameDirector.Id;
                }

                CurrentPartner.Email = EmailTextBox.Text;
                CurrentPartner.NumberPhone = NumberPhoneTextBox.Text;
                CurrentPartner.Rating = Convert.ToInt32(RatingTextBox.Text);
                var selectedType = TypePartnerComboBox.SelectedItem as Data.TypePartner;
                CurrentPartner.IdTypePartner = selectedType.Id;
                CurrentPartner.NamePartner = NamePartnerTextBox.Text;
                address.Home = Convert.ToInt32(HouseTextBox.Text);
                CurrentPartner.IdAddress = address.Id;

                if(FlagAddOrEdit == "add")
                {
                    Data.DatabaseMasterPolEntities.GetContext().PartnerImport.Add(CurrentPartner);
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно добавлено!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if(FlagAddOrEdit == "edit")
                {
                    Data.DatabaseMasterPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно сохранено!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.PartnerListViewPage());
        }
    }
}
