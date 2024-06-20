using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace Lab31
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProcesses();
        }

        // Метод, що завантажує список усіх запущених процесів
        private void LoadProcesses()
        {
            var processes = Process.GetProcesses().OrderBy(p => p.ProcessName).ToList();
            ProcessesDataGrid.ItemsSource = processes;
        }

        // Кнопка для оновлення відображення процесів
        private void RefreshProcesses_Click(object sender, RoutedEventArgs e)
        {
            LoadProcesses();
        }

        // Кнопка для експорту відображуваних процесів
        private void ExportProcesses_Click(object sender, RoutedEventArgs e)
        {
            var processes = Process.GetProcesses().OrderBy(p => p.ProcessName);
            string filePath = "process_list.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var process in processes)
                {
                    writer.WriteLine($"{process.ProcessName} - Ідентифікатор процесу: {process.Id}");
                }
            }

            MessageBox.Show($"Список процесів експортовано в {filePath}", "Експорт завершено", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Метод, за допомогою якого викликається меню
        private void ProcessesDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ProcessesDataGrid.SelectedItem == null)
            {
                ProcessContextMenu.IsEnabled = false;
            }
            else
            {
                ProcessContextMenu.IsEnabled = true;
            }
        }

        // Кнопка для відображення діалогового вікна з інформацією процесу
        private void ShowProcessInfo_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesDataGrid.SelectedItem is Process selectedProcess)
            {
                MessageBox.Show($"Ім'я: {selectedProcess.ProcessName}\nІдентифікатор процесу: {selectedProcess.Id}\nПам'ять: {selectedProcess.WorkingSet64 / 1024 / 1024} MB", "Деталі процесу", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        // Кнопка для зупинки процесу
        private void KillProcess_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesDataGrid.SelectedItem is Process selectedProcess)
            {
                try
                {
                    selectedProcess.Kill();
                    MessageBox.Show("Процес успішно завершено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProcesses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося завершити процес: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Кнопка для виклику  діалогового вікна з інформацією про потоки
        private void ShowThreadsInfo_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesDataGrid.SelectedItem is Process selectedProcess)
            {
                try
                {
                    var threads = selectedProcess.Threads;
                    StringBuilder info = new StringBuilder();
                    info.AppendLine("Потоки:");
                    foreach (ProcessThread thread in threads)
                    {
                        info.AppendLine($"Ідентифікатор потоку: {thread.Id}, Стан: {thread.ThreadState}");
                    }

                    MessageBox.Show(info.ToString(), "Потоки процесу", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося отримати потоки: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Кнопка, що відображає діалогове вікно з інформацією про модулі
        private void ShowModulesInfo_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesDataGrid.SelectedItem is Process selectedProcess)
            {
                try
                {
                    var modules = selectedProcess.Modules;
                    StringBuilder info = new StringBuilder();
                    info.AppendLine("Модулі:");
                    foreach (ProcessModule module in modules)
                    {
                        info.AppendLine($"Ім'я: {module.ModuleName}, Шлях: {module.FileName}");
                    }

                    MessageBox.Show(info.ToString(), "Модулі процесу", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося отримати модулі: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
