//**********************************************************************************
//* Название программы: "Игровое приложение "Жизнь""                               *
//*                                                                                *
//* Назначение программы: программа предназначена для моделирования колонии Жизни. *
//*                                                                                *
//* Разработчик: студентка группы ПР-330/б Исаева В.В.                             *
//*                                                                                *
//* Версия: 1.0                                                                    *
//**********************************************************************************

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
using System.Windows.Threading;

namespace Игра_Жизнь
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Окно_для_игры : Window
    {
        private int GridSize = 15;
        public GameOfLife.Cell[,] cells;
        private DispatcherTimer timer;
        private bool isRunning = true;
        public string Login;
        GameOfLife gameOfLife = new GameOfLife();

        public Окно_для_игры(string login)
        {
            InitializeComponent();
            gameOfLife.InitializeGrid(GridSize, LifeGrid);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            gameOfLife.TimerTick += Timer_Tick;
            timer.Start();

            Clear.Click += Clear_Click;

            Login = login;

        }

        /// <summary>
        /// Метод, обрабатывающий события таймера 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            lb1.Content = gameOfLife.UpdateGrid(Login, LifeGrid);
        }

        /// <summary>
        /// Запуск/остановка игры в ручном режиме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                gameOfLife.StopTimer();
                StartStopButton.Content = "Старт";
            }
            else
            {
                gameOfLife.StartTimer();
                StartStopButton.Content = "Стоп";
            }

            isRunning = !isRunning;
        }

        /// <summary>
        /// Обновление эволюции пошагово
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStepButton_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.UpdateGrid(Login, LifeGrid);
        }

        /// <summary>
        /// Очистка поля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.FillAllCellsWithColor(Colors.LightGray);
        }

       
        /// <summary>
        /// Перезапуск поля 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.ResetGrid(LifeGrid);

            // Если эволюция была остановлена, таймер перезапускается
            if (!isRunning)
            {
                gameOfLife.StartTimer();
                isRunning = true;
            }
        }

        /// <summary>
        /// Ускорение скорости эволюции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUpButton_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.IntervalTimer(200);
        }

        /// <summary>
        /// Замедление скорости эволюции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlowDownButton_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.IntervalTimer(800);
        }

        /// <summary>
        /// Возвращение к скорости, заданной по умолчанию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.IntervalTimer(500);
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

        /// <summary>
        /// Возвращение на главное окно приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Изменение настроек поля, если значения введены корректно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeGridSettings_Click(object sender, RoutedEventArgs e)
        {
            gameOfLife.ChangeGridSettings(LifeGrid, SizeTextBox, SpeedTextBox);
        }

        /// <summary>
        /// Переход к статистике игроков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            Статистика статистика = new Статистика();
            статистика.Show();
        }
    }
}

