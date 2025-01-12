using System;
using System.Drawing;
using System.Windows.Forms;
using FridgeApp;

namespace FridgeApp
{
    public partial class FridgeOpenForm : Form
    {
        private Label temperatureLabel;
        private Label maxTemperatureLabel;
        private Button setTemperatureButton;
        private Button closeButton; // Новая кнопка для закрытия формы
        private int maxTemperature;

        public FridgeOpenForm(int initialTemperature, int maxTemperature)
        {
            this.maxTemperature = maxTemperature; // Устанавливаем максимальную температуру
            InitializeComponent(); // Этот вызов должен быть только здесь
            InitializeFridgeOpen(initialTemperature);
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

            // Добавление элементов на форму
            this.Controls.Add(closeButton); // Добавляем кнопку "Закрыть"
            this.Controls.Add(setTemperatureButton);
            this.Controls.Add(maxTemperatureLabel);
            this.Controls.Add(temperatureLabel);
        }

        private void SetTemperatureButton_Click(object sender, EventArgs e)
        {
            using (SetTemperatureForm setTemperatureForm = new SetTemperatureForm())
            {
                if (setTemperatureForm.ShowDialog() == DialogResult.OK)
                {
                    // Получаем новую температуру и проверяем, что устанавливаем
                    if (setTemperatureForm.IsMaxTemperature)
                    {
                        maxTemperature = setTemperatureForm.NewTemperature; // Устанавливаем новую максимальную температуру
                        maxTemperatureLabel.Text = $"Максимальная температура: {maxTemperature} °C"; // Обновляем метку
                    }
                    else
                    {
                        int newTemperature = setTemperatureForm.NewTemperature;
                        if (newTemperature <= maxTemperature)
                        {
                            temperatureLabel.Text = $"Температура внутри: {newTemperature} °C"; // Устанавливаем новую температуру
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
            fridgeTimer.Stop();
            MessageBox.Show("Холодильник закрыт!");
            this.Close(); // Закрывает текущую форму
        }
    }
}
