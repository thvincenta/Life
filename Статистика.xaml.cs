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
    /// Логика взаимодействия для Статистика.xaml
    /// </summary>
    public partial class Статистика : Window
    {
        public Статистика()
        {
            InitializeComponent();

            string[] lines = File.ReadAllLines("Статистика.txt");
            foreach (string line in lines)
            {
                lbxStatistics.Items.Add(line);
            }
        }

        /// <summary>
        /// Переход к справке приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            Справка справка = new Справка();
            справка.Show();
        }
    }
}
