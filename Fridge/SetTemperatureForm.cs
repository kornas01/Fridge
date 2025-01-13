using System;
using System.Drawing;
using System.Windows.Forms;

namespace FridgeApp
{
    public partial class SetTemperatureForm : Form
    {
        private Label label;
        private RadioButton setCurrentTemperatureRadio;
        private RadioButton setMaxTemperatureRadio;
        private TextBox temperatureInput;
        private Button okButton;
        private Button cancelButton;

        public int NewTemperature { get; private set; }
        public bool IsMaxTemperature { get; private set; }

        public SetTemperatureForm(int currentTemperature)
        {
            InitializeComponent();
            InitializeSetTemperatureForm();
        }

        private void InitializeSetTemperatureForm()
        {
            this.Text = "Установка температуры";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            // Настройка метки
            label = new Label();
            label.Text = "Введите новую температуру:";
            label.Dock = DockStyle.Top;
            label.TextAlign = ContentAlignment.MiddleCenter;

            // Настройка радиокнопок
            setCurrentTemperatureRadio = new RadioButton();
            setCurrentTemperatureRadio.Text = "Установить текущую температуру";
            setCurrentTemperatureRadio.Dock = DockStyle.Top;

            setMaxTemperatureRadio = new RadioButton();
            setMaxTemperatureRadio.Text = "Установить максимальную температуру";
            setMaxTemperatureRadio.Dock = DockStyle.Top;

            // Настройка текстового поля
            temperatureInput = new TextBox();
            temperatureInput.Dock = DockStyle.Top;
            temperatureInput.TextAlign = HorizontalAlignment.Center;

            // Настройка кнопки OK
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Dock = DockStyle.Bottom;
            okButton.Click += OkButton_Click;

            // Настройка кнопки Cancel
            cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Dock = DockStyle.Bottom;
            cancelButton.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Добавление элементов на форму
            this.Controls.Add(cancelButton);
            this.Controls.Add(okButton);
            this.Controls.Add(temperatureInput);
            this.Controls.Add(setMaxTemperatureRadio);
            this.Controls.Add(setCurrentTemperatureRadio);
            this.Controls.Add(label);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(temperatureInput.Text, out int newTemperature))
            {
                NewTemperature = newTemperature;
                IsMaxTemperature = setMaxTemperatureRadio.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное значение температуры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}