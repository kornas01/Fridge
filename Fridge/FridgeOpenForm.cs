using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FridgeApp
{
    public partial class FridgeOpenForm : Form
    {
        private Label temperatureLabel;
        private Label maxTemperatureLabel;
        private Button setTemperatureButton;
        private Button closeButton; // Новая кнопка для закрытия формы
        private int maxTemperature;
        Form1 form = new Form1();
        private int currentTemperature; // Объявляем переменную для текущей температуры
        private Button resetSettingsButton; // Объявляем кнопку
        public FridgeOpenForm(int initialTemperature, int maxTemperature)
        {
            this.maxTemperature = maxTemperature; // Устанавливаем максимальную температуру
            InitializeComponent(); // Этот вызов должен быть только здесь
            InitializeFridgeOpen(initialTemperature);
            currentTemperature = 0; // Начальное значение текущей температуры
        }

        private void InitializeFridgeOpen(int initialTemperature)
        {
            this.Text = "Открытый холодильник";
            this.Size = new Size(300, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Настройка метки для текущей температуры
            temperatureLabel = new Label();
            temperatureLabel.Text = $"Температура внутри: {initialTemperature} °C";
            temperatureLabel.Dock = DockStyle.Top;
            temperatureLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Настройка метки для максимальной температуры
            maxTemperatureLabel = new Label();
            maxTemperatureLabel.Text = $"Максимальная температура: {maxTemperature} °C";
            maxTemperatureLabel.Dock = DockStyle.Top;
            maxTemperatureLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Настройка кнопки для установки температуры
            setTemperatureButton = new Button();
            setTemperatureButton.Text = "Установить температуру";
            setTemperatureButton.Dock = DockStyle.Top;
            setTemperatureButton.Click += SetTemperatureButton_Click;

            // Настройка кнопки для закрытия формы
            closeButton = new Button();
            closeButton.Text = "Закрыть";
            closeButton.Dock = DockStyle.Top;
            closeButton.Click += CloseButton_Click; // Привязываем обработчик события

            resetSettingsButton = new Button();
            resetSettingsButton.Text = "Сбросить настройки"; //
            resetSettingsButton.Dock = DockStyle.Bottom; // Прикрепляем кнопку к низу формы
            resetSettingsButton.Click += ResetSettingsButton_Click; // Привязываем обработчик события

            // Добавление элементов на форму
            this.Controls.Add(closeButton); // Добавляем кнопку "Закрыть"
            this.Controls.Add(setTemperatureButton);
            this.Controls.Add(maxTemperatureLabel);
            this.Controls.Add(temperatureLabel);
            this.Controls.Add(resetSettingsButton);
        }

        private void SetTemperatureButton_Click(object sender, EventArgs e)
        {
            using (SetTemperatureForm setTemperatureForm = new SetTemperatureForm(currentTemperature)) // Передаем текущую температуру
            {
                if (setTemperatureForm.ShowDialog() == DialogResult.OK)
                {
                    // Получаем новую температуру и проверяем, что устанавливаем
                    if (setTemperatureForm.IsMaxTemperature)
                    {
                        maxTemperature = setTemperatureForm.NewTemperature; // Устанавливаем новую максимальную температуру
                        maxTemperatureLabel.Text = $"Максимальная температура: {maxTemperature} °C"; // Обновляем метку

                        // Проверка: если новая максимальная температура ниже текущей температуры
                        if (maxTemperature < currentTemperature)
                        {
                            MessageBox.Show("Запускается мотор, так как максимальная температура ниже текущей.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogFridgeMotor();
                        }
                    }
                    else
                    {
                        int newTemperature = setTemperatureForm.NewTemperature;
                        if (newTemperature <= maxTemperature)
                        {
                            currentTemperature = newTemperature; // Обновляем текущую температуру
                            temperatureLabel.Text = $"Температура внутри: {currentTemperature} °C"; // Устанавливаем новую температуру
                            LogFridgeTemp();

                            // Проверка: если новая температура выше максимальной
                            if (newTemperature > maxTemperature)
                            {
                                MessageBox.Show($"Температура не может превышать {maxTemperature} °C.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Температура не может превышать {maxTemperature} °C.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        // Обработчик события для кнопки "Закрыть"
        private void CloseButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Холодильник закрыт!");
            LogFridgeOpening();
            this.Close(); // Закрывает текущую форму
             form.Show();
        }
        private void ResetSettingsButton_Click(object sender, EventArgs e)
        {
          
            MessageBox.Show("Настройки сброшены!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Закрывает текущую форму
            form.Show();
            LogFridgeSett();
        }

        private void LogFridgeOpening()
        {
            string filePath = Path.Combine(Application.StartupPath, "Log.txt");

            // Проверка, существует ли файл
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл Log.txt не найден.");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true)) // true - для добавления в конец файла
            {
                writer.WriteLine($"В {DateTime.Now} дверца закрылась");
            }
            MessageBox.Show("Запись добавлена в Log.txt");
        }

        private void LogFridgeSett()
        {
            string filePath = Path.Combine(Application.StartupPath, "Log.txt");

            // Проверка, существует ли файл
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл Log.txt не найден.");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true)) // true - для добавления в конец файла
            {
                writer.WriteLine($"В {DateTime.Now} настройки были сброшены");
            }
            MessageBox.Show("Запись добавлена в Log.txt");
        }
        private void LogFridgeMotor()
        {
            string filePath = Path.Combine(Application.StartupPath, "Log.txt");

            // Проверка, существует ли файл
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл Log.txt не найден.");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true)) // true - для добавления в конец файла
            {
                writer.WriteLine($"В {DateTime.Now} включился мотор");
            }
            MessageBox.Show("Запись добавлена в Log.txt");
        }
        private void LogFridgeTemp()
        {
            string filePath = Path.Combine(Application.StartupPath, "Log.txt");

            // Проверка, существует ли файл
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл Log.txt не найден.");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true)) // true - для добавления в конец файла
            {
                writer.WriteLine($"В {DateTime.Now} изменили температуру");
            }
            MessageBox.Show("Запись добавлена в Log.txt");
        }
    }
}
