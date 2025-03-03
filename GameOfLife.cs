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
using System.IO;

namespace Игра_Жизнь
{
    public class GameOfLife
    {
        private int GridSize = 15;
        public Cell[,] cells;
        private Random random = new Random();
        private DispatcherTimer timer;
        public int generationCount;
        public int stableGenerationCount = 0;
        public int finalGenerationCount;
        public event EventHandler TimerTick;

        public GameOfLife()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();

            cells = new Cell[GridSize, GridSize];
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (cells[i, j] == null)
                    {
                        int colony = random.Next(1, 6);
                        bool isAlive = random.Next(2) == 1;
                        var cell = new Cell(colony, isAlive);
                        cells[i, j] = cell;
                    }
                }
            }
        }

        /// <summary>
        /// Метод, обрабатывающий события таймера 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerTick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Метод, останавливающий таймер
        /// </summary>
        public void StopTimer()
        {
            timer.Stop();
        }

        /// <summary>
        /// Метод, запускающий таймер
        /// </summary>
        public void StartTimer()
        {
            timer.Start();
        }

        /// <summary>
        /// Метод, задающий интервал обновления клеток
        /// </summary>
        public void IntervalTimer(int ms)
        {
            timer.Interval = TimeSpan.FromMilliseconds(ms);
        }

        /// <summary>
        /// Класс, задающий размеры и свойства ячеек
        /// </summary>
        public class Cell : Border
        {
            public int Colony { get; set; }
            public bool IsAlive { get; set; }

            public Cell(int colony, bool isAlive)
            {
                Colony = colony;
                IsAlive = isAlive;

                Width = 30;
                Height = 30;
                BorderBrush = Brushes.Black;
                BorderThickness = new Thickness(1);
                Background = Brushes.LightGray;
                Colors();

                MouseLeftButtonDown += Cell_MouseLeftButtonDown;
            }

            /// <summary>
            /// Метод, устанавливающий цвет клетки в зависимости от номера колонии
            /// </summary>
            public void Colors()
            {
                if (Colony == 1)
                {
                    Background = IsAlive ? Brushes.SeaGreen : Brushes.LightGray;
                }
                else if (Colony == 2)
                {
                    Background = IsAlive ? Brushes.SteelBlue : Brushes.LightGray;
                }
                else if (Colony == 3)
                {
                    Background = IsAlive ? Brushes.Coral : Brushes.LightGray;
                }
                else if (Colony == 4)
                {
                    Background = IsAlive ? Brushes.DarkSalmon : Brushes.LightGray;
                }
                else if (Colony == 5)
                {
                    Background = IsAlive ? Brushes.Khaki : Brushes.LightGray;
                }
            }

            /// <summary>
            /// Обработчик события для отслеживания выбранной ячейки и присвоения ей цвета
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                IsAlive = !IsAlive;
                Colors();
            }
        }

        /// <summary>
        /// Метод, создающий барьер для изоляции колоний
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="colony"></param>
        public void CreateColonyBarrier(int startX, int startY, int endX, int endY, int colony)
        {
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (cells[x, y] != null)
                    {
                        cells[x, y].Colony = colony;
                    }
                }
            }
        }

        /// <summary>
        /// Метод, инициализирующий игровое поле
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="LifeGrid"></param>
        public void InitializeGrid(int newSize, Grid LifeGrid)
        {
            cells = new Cell[newSize, newSize];
            LifeGrid.Children.Clear();
            LifeGrid.RowDefinitions.Clear();
            LifeGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < newSize; i++)
            {
                LifeGrid.RowDefinitions.Add(new RowDefinition());
                LifeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            CreateBlinker(GridSize / 4, GridSize / 4);
            CreateToad(GridSize / 2, GridSize / 2);
            CreateBlock(GridSize / 3, GridSize / 3);
            CreateCone(GridSize / 6, GridSize / 6);
            CreateBoat(GridSize / 3, GridSize / 3);
            CreateGlider(GridSize / 5, GridSize / 5);
            CreateFigureEight(GridSize / 2, GridSize / 2);
            CreatePentadecathlon(GridSize / 15, GridSize / 15);
           
            for (int i = 0; i < newSize; i++)
            {
                for (int j = 0; j < newSize; j++)
                {
                    if (cells[i, j] == null)
                    {
                        int colony = random.Next(1, 6);
                        bool isAlive = random.Next(2) == 1;
                        var cell = new Cell(colony, isAlive);
                        cells[i, j] = cell;
                        LifeGrid.Children.Add(cell);
                        Grid.SetRow(cell, i);
                        Grid.SetColumn(cell, j);
                    }
                }
            }

            int verticalBarrier = GridSize / 2;
            int horizontalBarrier = GridSize / 2;

            // Создание барьеров, чтобы разделить поле на 4 равные части
            CreateColonyBarrier(0, 0, verticalBarrier, horizontalBarrier, 1);
            CreateColonyBarrier(verticalBarrier + 1, 0, GridSize - 1, horizontalBarrier, 2);
            CreateColonyBarrier(0, horizontalBarrier + 1, verticalBarrier, GridSize - 1, 3);
            CreateColonyBarrier(verticalBarrier + 1, horizontalBarrier + 1, GridSize - 1, GridSize - 1, 4);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Блинкер
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateBlinker(int x, int y)
        {
            cells[x, y] = new Cell(1, true);
            cells[x, y - 1] = new Cell(1, true);
            cells[x, y + 1] = new Cell(1, true);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Труба
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateToad(int x, int y)
        {
            cells[x, y] = new Cell(1, true);
            cells[x, y + 1] = new Cell(1, true);
            cells[x, y + 2] = new Cell(1, true);
            cells[x + 1, y - 1] = new Cell(1, true);
            cells[x + 1, y] = new Cell(1, true);
            cells[x + 1, y + 1] = new Cell(1, true);
        }
        /// <summary>
        /// Метод, реализующий осциллятор Блок
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateBlock(int x, int y)
        {
            cells[x, y] = new Cell(1, true);
            cells[x + 1, y] = new Cell(1, true);
            cells[x, y + 1] = new Cell(1, true);
            cells[x + 1, y + 1] = new Cell(1, true);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Конус
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateCone(int x, int y)
        {
            cells[x + 2, y] = new Cell(1, true);

            cells[x + 1, y + 1] = new Cell(1, true);
            cells[x + 2, y + 1] = new Cell(1, true);
            cells[x + 3, y + 1] = new Cell(1, true);

            cells[x, y + 2] = new Cell(1, true);
            cells[x + 1, y + 2] = new Cell(1, true);
            cells[x + 2, y + 2] = new Cell(1, true);
            cells[x + 3, y + 2] = new Cell(1, true);
            cells[x + 4, y + 2] = new Cell(1, true);

            cells[x + 1, y + 3] = new Cell(1, true);
            cells[x + 2, y + 3] = new Cell(1, true);
            cells[x + 3, y + 3] = new Cell(1, true);

            // Остальные клетки "Конуса"
            cells[x + 2, y + 4] = new Cell(1, true);
        }

               /// <summary>
        /// Метод, реализующий осциллятор Лодка
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateBoat(int x, int y)
        {
            cells[x, y] = new Cell(1, true);
            cells[x + 1, y] = new Cell(1, true);
            cells[x, y + 1] = new Cell(1, true);
            cells[x + 2, y + 1] = new Cell(1, true);
            cells[x + 1, y + 2] = new Cell(1, true);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Планер
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateGlider(int x, int y)
        {
            cells[x, y] = new Cell(1, true);
            cells[x + 1, y + 1] = new Cell(1, true);
            cells[x + 2, y - 1] = new Cell(1, true);
            cells[x + 2, y] = new Cell(1, true);
            cells[x + 2, y + 1] = new Cell(1, true);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Восьмерка
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateFigureEight(int x, int y)
        {
            CreateBlock(x, y);
            CreateBlock(x + 4, y);
        }

        /// <summary>
        /// Метод, реализующий осциллятор Пентадекатлон
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreatePentadecathlon(int x, int y)
        {
            // Верхний ряд
            cells[x, y] = new Cell(1, true);
            cells[x + 2, y] = new Cell(1, true);

            // Середина
            cells[x + 1, y + 1] = new Cell(1, true);
            cells[x + 3, y + 1] = new Cell(1, true);
            cells[x + 4, y + 1] = new Cell(1, true);

            // Нижний ряд
            cells[x, y + 2] = new Cell(1, true);
            cells[x + 2, y + 2] = new Cell(1, true);
        }

        /// <summary>
        /// Метод, обновляющий игровое поле
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="LifeGrid"></param>
        /// <returns></returns>
        public string UpdateGrid(string Login, Grid LifeGrid)
        {
            generationCount++; // Счетчик поколений
            var newCells = new Cell[GridSize, GridSize];
            bool isStable = true;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    int liveNeighbors = GetLiveNeighborCount(i, j);
                    int currentColony = cells[i, j].Colony;
                    bool isAlive = cells[i, j].IsAlive;

                    if (isAlive && (liveNeighbors < 2 || liveNeighbors > 3))
                    {
                        newCells[i, j] = new Cell(currentColony, false);
                    }
                    else if (!isAlive && liveNeighbors == 3)
                    {
                        newCells[i, j] = new Cell(currentColony, true);
                    }
                    else
                    {
                        newCells[i, j] = new Cell(currentColony, isAlive);
                    }

                    if (newCells[i, j].IsAlive != cells[i, j].IsAlive)
                    {
                        isStable = false;
                    }
                }
            }

            if (isStable)
            {
                stableGenerationCount++;
            }
            else
            {
                stableGenerationCount = 0;
            }

            if (stableGenerationCount >= 1)
            {
                
                finalGenerationCount = generationCount;

                string fileName = "Статистика.txt";

                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    string entryDateTime = DateTime.Now.ToString();
                    writer.WriteLine($"Дата и время входа: {entryDateTime}" + "\n" + $"Логин: {Login}" + "\n" + $"Количество созданных поколений: {finalGenerationCount}");
                    writer.WriteLine();
                }
                timer.Stop();
            }

            cells = newCells;
            LifeGrid.Children.Clear();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    LifeGrid.Children.Add(cells[i, j]);
                    Grid.SetRow(cells[i, j], i);
                    Grid.SetColumn(cells[i, j], j);
                }
            }

            return $"Количество поколений: {generationCount}";
        }

        /// <summary>
        /// Метод, ведущий подсчет живых соседей
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int GetLiveNeighborCount(int row, int col)
        {
            int liveCount = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int neighborRow = row + i;
                    int neighborCol = col + j;

                    if (neighborRow >= 0 && neighborRow < GridSize &&
                        neighborCol >= 0 && neighborCol < GridSize &&
                        cells[neighborRow, neighborCol].IsAlive)
                    {
                        liveCount++;
                    }
                }
            }

            return liveCount;
        }

        /// <summary>
        /// Метод, перезапускающий настройки игрового поля
        /// </summary>
        /// <param name="LifeGrid"></param>
        public void ResetGrid(Grid LifeGrid)
        {
            LifeGrid.Children.Clear();
            generationCount = 0;
            InitializeGrid(GridSize, LifeGrid);
        }

        /// <summary>
        /// Метод, реализующий заливку Grid, необходимую при очистке поля
        /// </summary>
        /// <param name="color"></param>
        public void FillAllCellsWithColor(Color color)
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    if (cells[x, y] != null)
                    {
                        cells[x, y].IsAlive = false;
                        cells[x, y].Colors();
                    }
                    generationCount = -1;
                }
            }
        }

        /// <summary>
        /// Метод, устанавливающий новые настройки поля
        /// </summary>
        /// <param name="LifeGrid"></param>
        /// <param name="SizeTextBox"></param>
        /// <param name="SpeedTextBox"></param>
        public void ChangeGridSettings(Grid LifeGrid, TextBox SizeTextBox, TextBox SpeedTextBox)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SizeTextBox.Text) || !string.IsNullOrWhiteSpace(SpeedTextBox.Text))
                {
                    if (!string.IsNullOrWhiteSpace(SizeTextBox.Text))
                    {
                        if (int.TryParse(SizeTextBox.Text, out int newSize))
                        {
                            if (newSize < 14)
                                throw new Exception("Размер поля не может быть меньше 14. Введите значение еще раз.");
                            GridSize = newSize;
                            InitializeGrid(newSize, LifeGrid);
                        }
                        else
                            throw new Exception("Пожалуйста, введите корректный размер (целое число) для ширины и высоты.");
                    }
                    else
                    {
                        GridSize = 15;
                    }

                    if (!string.IsNullOrWhiteSpace(SpeedTextBox.Text))
                    {
                        if (int.TryParse(SpeedTextBox.Text, out int newSpeed))
                        {
                            timer.Interval = TimeSpan.FromMilliseconds(newSpeed);
                        }
                        else
                            throw new Exception("Введено некорректное значение. Введите целое число для скорости таймера.");
                    }
                    else
                    {
                        timer.Interval = TimeSpan.FromMilliseconds(500);
                    }
                }
                else
                    throw new Exception("Оба поля для ввода пусты. Пожалуйста, введите значения для размера или скорости.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
