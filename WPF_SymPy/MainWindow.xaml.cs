using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Path = System.IO.Path;
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
using System.IO;

namespace WPF_SymPy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pythonInterpreterPath, pythonScriptPath;
        private ProcessStartInfo processStartInfo;

        public MainWindow()
        {
            // Инициализируем рабочие папки
            InitializeDirectories();
            // Инициализируем объект ProcessStartInfo
            InitializeProcessStartInfo();
            // Инициализируем компоненты WPF
            InitializeComponent();
        }

        // Функция, которая инициализирует пути до рабочих папок и файлов
        private void InitializeDirectories()
        {
            // Адрес, который мы скопировали из адресной строки проводника
            string startLocation = "C:\\Users\\User\\source\\repos\\WPF_SymPy";

            // Скомбинировали адрес папки проекта с путём к файлу скрипта
            pythonScriptPath = Path.Combine(startLocation, "WPF_Sympy\\PythonApplication\\main.py");
            // Проверяем файл на существование
            if (!File.Exists(pythonScriptPath))
            {
                // Бросаем ошибку, если его нет
                throw new Exception($"Ошибка: скрипт Python не найден по пути: {pythonScriptPath}");
            }

            // Аналогично
            pythonInterpreterPath = Path.Combine(startLocation, "CSharpPython\\Scripts\\python.exe");
            if (!File.Exists(pythonInterpreterPath))
            {
                throw new Exception($"Ошибка: интерпретатор Python не найден по пути: {pythonInterpreterPath}");
            }
        }

        // Функция, которая инициализирует объект ProcessStartInfo
        private void InitializeProcessStartInfo()
        {
            processStartInfo = new ProcessStartInfo
            {
                FileName = pythonInterpreterPath,
                WorkingDirectory = Path.GetDirectoryName(pythonScriptPath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }

        // Обработчик нажатия на кнопку
        private void ResultBtn_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выражения на вход
            string a = FirstExpr.Text;
            string b = SecondExpr.Text;

            // Передаём их в качестве аргументов вызова Python с консоли
            processStartInfo.Arguments = $"{pythonScriptPath} {a} {b}";

            // Запускаем процесс по настройкам из объекта ProcessStartInfo
            using (Process process = Process.Start(processStartInfo))
            {
                // Запускаем считывание консоли
                using (StreamReader reader = process.StandardOutput)
                {
                    // Получаем результат
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);

                    // Выводим на экран
                    ResultOutput.Text = result;
                }

                // Отлавливаем ошибки в случае их возникновения
                using (StreamReader errorReader = process.StandardError)
                {
                    string error = errorReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                }

                // Ждём завершения процесса
                process.WaitForExit();
            }
        }
    }
}
