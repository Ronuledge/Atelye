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
using System.Data;
using System.Data.SqlClient;

namespace Ателье
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateService()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select idService,Name,Price from Service", connection);
                DataTable dtService = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtService);
                dgService.ItemsSource = dtService.DefaultView;
            }
        }

        private void UpdateClient()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select idClient as 'ID',Name as 'ФИО',Addres as 'Адрес',Phone as 'Телефон' from Client", connection);
                DataTable dtClient = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtClient);
                dgClient.ItemsSource = dtClient.DefaultView;
            }
        }

        private void UpdateOrder()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select [Orders].[idOrder] as 'Номер заказа', [Service].[Name] as 'Название услуги', [Orders].[Date] as 'Дата', [Orders].[Value] as 'Кол-во', [Orders].[Status] as 'Статус' from [Orders],[Service] where [Orders].[idService] = [Service].[idService]", connection);
                DataTable dtOrder = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtOrder);
                dgOrder.ItemsSource = dtOrder.DefaultView;
            }
        }

        //Таблица Услуг//
        private void dgService_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateService();
        }

        //Добавление новых услуг
        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into Service (Name,Price) values ('" + txtService.Text + "','" + txtPrice.Text + "')", connection);
                command.ExecuteNonQuery();
                UpdateService();
            }
        }

        //Редактирование услуг
        private void btnRedactService_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgService.SelectedItem as DataRowView;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("update Service set Name='"+txtService.Text+"', Price = '"+txtPrice.Text+"' where idService="+ row.Row.ItemArray[0]+"", connection);
                command.ExecuteNonQuery();
                UpdateService();
            }
        }

        //Удаление услуг
        private void btnDelService_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgService.SelectedItem as DataRowView;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete from Service where idService" + row.Row.ItemArray[0] + "", connection);
                command.ExecuteNonQuery();
                UpdateService();
            }
        }

        //Таблица Клиентов//
        private void dgClient_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClient();
        }

        //Добавление клиентов
        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into Client (Name,Addres,Phone) values ('" + txtNameClient.Text + "','" + txtAddres.Text + "','" + txtPhone.Text + "')", connection);
                command.ExecuteNonQuery();
                UpdateService();
            }
        }

        //Редактирование клиентов
        private void btnRedactClient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgService.SelectedItem as DataRowView;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("update Client set Name='" + txtNameClient.Text + "', Phone = '" + txtPhone.Text + "' where idClient=" + row.Row.ItemArray[0] + "", connection);
                command.ExecuteNonQuery();
                UpdateClient();
            }
        }

        //Удаление клиентов
        private void btnDelClient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgService.SelectedItem as DataRowView;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete from Client where idClient" + row.Row.ItemArray[0] + "", connection);
                command.ExecuteNonQuery();
                UpdateService();
            }
        }

        //Таблица Заказов//
        private void dgOrder_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        private void dgOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = dgOrder.SelectedItem as DataRowView;
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbAtelye))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select [Client].[Name],[Client].[Phone] from [Client],[Orders] where [Client].[idClient] = [Orders].[idClient] and [Orders].[idOrder]=" + row.Row.ItemArray[0]+"", connection);
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                dgClientInfo.ItemsSource = dt.DefaultView;
            }
        }
    }
}
