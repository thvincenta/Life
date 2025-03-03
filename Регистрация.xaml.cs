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
using System.Windows.Shapes;
using System.IO;

namespace Игра_Жизнь
{
    /// <summary>
    /// Логика взаимодействия для Регистрация.xaml
    /// </summary>
    public partial class Регистрация : Window
    {
        public Регистрация()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Переход к игровому окну, если поле "Логин" заполнено
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            string login = tb1.Text;
            try
            {
                if (login == "")
                    throw new Exception("Заполните поле 'Логин'");
                
                MessageBox.Show($"Поздравляем, {login}. Игра начинается!", "Регистрация прошла успешно!");
                Окно_для_игры окно_Для_Игры = new Окно_для_игры(login);
                окно_Для_Игры.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///  Переход к справке приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            Справка справка = new Справка();
            справка.Show();
        }

        /// <summary>
        /// Возвращение на главное окно приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
