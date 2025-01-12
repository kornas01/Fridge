using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FridgeApp
{
    public partial class Form1 : Form
    {
        private PictureBox fridgePictureBox;
        private Button openButton;
        private Button setTimerButton;
        private int remainingTime; // оставшееся время в секундах
        public Timer fridgeTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeFridge();
        }

        public void InitializeFridge()
        {
            // Настройка формы
            this.Text = "Холодильник";
            this.Size = new Size(300, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Настройка PictureBox для холодильника
            fridgePictureBox = new PictureBox();
            fridgePictureBox.Image = Image.FromFile(Path.Combine(Application.StartupPath, "refrigerator.jpg"));
            fridgePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            fridgePictureBox.Dock = DockStyle.Fill;

            // Настройка кнопки "Открыть"
            openButton = new Button();
            openButton.Text = "Открыть";
            openButton.Dock = DockStyle.Bottom;
            openButton.Click += OpenButton_Click;

            // Настройка кнопки "Установить таймер"
            setTimerButton = new Button();
            setTimerButton.Text = "Установить таймер";
            setTimerButton.Dock = DockStyle.Bottom;
            setTimerButton.Click += SetTimerButton_Click;

            // Инициализация таймера
            fridgeTimer = new Timer();
            fridgeTimer.Interval = 1000; // 1 секунда
            fridgeTimer.Tick += FridgeTimer_Tick;

            // Добавление элементов управления на форму
            this.Controls.Add(fridgePictureBox);
            this.Controls.Add(openButton);
            this.Controls.Add(setTimerButton);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            openButton.Text = "Холодильник открыт!";
            fridgeTimer.Start(); // Запуск таймера при открытии холодильника

            // Создание и открытие формы холодильника
            int initialTemperature = 5; // Укажите начальную температуру
            int maxTemperature = 25; // Укажите максимальную температуру

            FridgeOpenForm fridgeOpenForm = new FridgeOpenForm(initialTemperature, maxTemperature);
            fridgeOpenForm.Show(); // или fridgeOpenForm.ShowDialog(); в зависимости от ваших нужд
        }

        private void SetTimerButton_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите время в секундах:", "Установка таймера", "10");
            if (int.TryParse(input, out remainingTime) && remainingTime > 0)
            {
                MessageBox.Show($"Таймер установлен на {remainingTime} секунд.");
                fridgeTimer.Start();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите действительное число больше нуля.");
            }
        }

        private void FridgeTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
            }
            else
            {
                fridgeTimer.Stop();
                MessageBox.Show("Холодильник открыт больше положенного времени!");
            }
        }
    }
}