using System;
using System.Windows.Forms;
using System.Linq;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // 1. Оголошення полів класу
        // Розмірність масивів - (N + 1) для використання індексів від 1 до N
        private int N = 3;                                  // Розмірність СНР
        private double[,] Ja = new double[4, 4];            // Матриця Якобі
        private double[] X0 = new double[4];                // Вектор початкових умов
        private double[] X = new double[4];                 // Вектор розв'язку СНР (поточне наближення)
        private double[] F = new double[4];                 // Вектор лівої частини СНР
        private double[] Fp = new double[4];                // Робочий вектор для обчислення Якобі
        private double[] Dx = new double[4];                // Вектор нев'язки (розв'язок СЛАР)

        public Form1()
        {
            InitializeComponent();

            // Налаштування полів форми при запуску
            N = (int)numericUpDown1.Value;
            ResizeArrays(N);
            SetupDataGridViews();

            // Прив'язка обробників подій
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
        }

        // --- Допоміжні методи для інтерфейсу та ініціалізації ---
        private void ResizeArrays(int size)
        {
            N = size;
            // Масиви створюються з мінімальним розміром 2, якщо N=1
            int arraySize = N + 1 > 2 ? N + 1 : 2;

            Ja = new double[arraySize, arraySize];
            X0 = new double[arraySize];
            X = new double[arraySize];
            F = new double[arraySize];
            Fp = new double[arraySize];
            Dx = new double[arraySize];
        }

        private void SetupDataGridViews()
        {
            // Налаштування DataGridView1 (для X0)
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "X0";
            dataGridView1.RowHeadersVisible = false;         // Приховано номери рядків
            dataGridView1.ColumnHeadersVisible = false;      // НОВЕ: Приховано заголовки стовпців ("X0")
            dataGridView1.AllowUserToAddRows = false;        // Прибрано функцію додавання нового рядка
            dataGridView1.Columns[0].Width = 70;

            // Налаштування DataGridView2 (для X*)
            dataGridView2.ColumnCount = 1;
            dataGridView2.Columns[0].Name = "X*";
            dataGridView2.RowHeadersVisible = false;         // Приховано номери рядків
            dataGridView2.ColumnHeadersVisible = false;      // НОВЕ: Приховано заголовки стовпців ("X*")
            dataGridView2.AllowUserToAddRows = false;        // Прибрано функцію додавання нового рядка
            dataGridView2.ReadOnly = true;
            dataGridView2.Columns[0].Width = 150;

            ResizeDataGridViews(N);
        }

        private void ResizeDataGridViews(int size)
        {
            dataGridView1.RowCount = size;
            dataGridView2.RowCount = size;
        }

        // --- Обробник зміни розмірності ---
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int newN = (int)numericUpDown1.Value;
            if (newN >= 1)
            {
                ResizeArrays(newN);
                ResizeDataGridViews(newN);
            }
        }


        // --- Методи класу, що реалізують алгоритм ---

        /// <summary>
        /// Метод для обчислення значень вектора F лівої частини СНР.
        /// </summary>
        public void FM(double[] X, ref double[] f)
        {
            // Тестова система для N=3
            if (N == 3)
            {
                // f₁(x₁, x₂, x₃) = x₁ + e^(x₁-1) + (x₂ + x₃)² - 27
                f[1] = X[1] + Math.Exp(X[1] - 1.0D) + (X[2] + X[3]) * (X[2] + X[3]) - 27.0D;

                // f₂(x₁, x₂, x₃) = x₁ * e^(x₂-2) + x₃² - 10
                f[2] = X[1] * Math.Exp(X[2] - 2.0D) + X[3] * X[3] - 10.0D;

                // f₃(x₁, x₂, x₃) = x₃ + Sin(x₂-2) + x₂² - 7
                f[3] = X[3] + Math.Sin(X[2] - 2.0D) + X[2] * X[2] - 7.0D;
            }
            else if (N == 1)
            {
                // Для N=1 (одне нелінійне рівняння)
                // f₁(x₁) = x₁^2 - 4
                f[1] = X[1] * X[1] - 4.0D;
            }
        }

        /// <summary>
        /// Метод для чисельного обчислення значень матриці Якобі Ja.
        /// </summary>
        public double[,] Jacob(double[] X)
        {
            // Константа збурення Q = 0.000001D
            const double Q = 0.000001D;

            double[] X_copy = (double[])X.Clone();

            // 1. Обчислення F(X) від незбуреного вектора X
            FM(X_copy, ref F);

            for (int j = 1; j <= N; j++)
            {
                // 2. Збурення j-ї компоненти
                X_copy[j] = X_copy[j] + Q;

                // 3. Обчислення Fp(X + Q*ej)
                FM(X_copy, ref Fp);

                for (int i = 1; i <= N; i++)
                {
                    // 4. Обчислення [i, j]-ї компоненти Якобі
                    Ja[i, j] = (Fp[i] - F[i]) / Q;
                }

                // 5. Зняття збурення
                X_copy[j] = X_copy[j] - Q;
            }

            return Ja;
        }

        // --- Обробники подій кнопок ---

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox1.Clear();
            textBox2.Clear();

            for (int i = 0; i < N; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = "";
                dataGridView2.Rows[i].Cells[0].Value = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (N < 1)
                {
                    MessageBox.Show("Розмірність СНР має бути мінімум 1.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1. Зчитування параметрів та ініціалізація
                double Eps = Convert.ToDouble(textBox1.Text); // Точність
                int KMax = Convert.ToInt32(textBox2.Text);    // Макс. кількість ітерацій

                // Зчитування вектора початкових умов X0 та ініціалізація X
                for (int i = 1; i <= N; i++)
                {
                    if (dataGridView1.Rows[i - 1].Cells[0].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[i - 1].Cells[0].Value.ToString()))
                    {
                        throw new FormatException($"Не введено значення для X0[{i}]");
                    }

                    X0[i] = Convert.ToDouble(dataGridView1.Rows[i - 1].Cells[0].Value);
                    X[i] = X0[i];
                    dataGridView2.Rows[i - 1].Cells[0].Value = "";
                }

                // 2. Запуск циклу ітерацій k=1 до KMax
                double dxmax = double.MaxValue;
                int k;

                for (k = 1; k <= KMax; k++)
                {
                    // 2.1. Обчислення F(X)
                    FM(X, ref F);

                    // 2.2. Обчислення матриці Якобі Ja(X)
                    Ja = Jacob(X);

                    // 2.3. Знаходження вектора Dx, розв'язуючи СЛАР Ja * Dx = F
                    GaussSolver.Solve(Ja, F, ref Dx, N);

                    // 2.4. Ітерація Ньютона та обчислення норми
                    dxmax = 0.0D;
                    for (int i = 1; i <= N; i++)
                    {
                        X[i] = X[i] - Dx[i];

                        if (Math.Abs(Dx[i]) > dxmax)
                            dxmax = Math.Abs(Dx[i]);
                    }

                    // 2.5. Перевірка умови закінчення
                    if (dxmax < Eps)
                    {
                        // Розв'язок знайдено
                        textBox3.Text = Convert.ToString(k);

                        // Виведення вектора X* у фіксованому форматі (без степенів)
                        for (int i = 1; i <= N; i++)
                        {
                            dataGridView2.Rows[i - 1].Cells[0].Value = X[i].ToString("F15");
                        }

                        MessageBox.Show("Розв'язок СНР знайдено");
                        return;
                    }
                }

                // 3. Якщо КМах ітерацій не досягнуто
                textBox3.Text = Convert.ToString(KMax);
                MessageBox.Show($"Неможливо розв'язати: Збіжності не досягнуто за максимальну кількість ітерацій ({KMax})", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (FormatException)
            {
                MessageBox.Show("Помилка вхідних даних: Перевірте формат чисел (наприклад, 0.0000001, 15) для Eps/KMax та X0.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Помилка обчислення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла невідома помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Клас для реалізації методу Гауса
    public static class GaussSolver
    {
        public static void Solve(double[,] A, double[] B, ref double[] x, int size)
        {
            // Створення копій для уникнення модифікації вхідних даних
            int arraySize = size + 1 > 2 ? size + 1 : 2;
            double[,] A_copy = new double[arraySize, arraySize];
            double[] B_copy = new double[arraySize];

            for (int i = 1; i <= size; i++)
            {
                B_copy[i] = B[i];
                for (int j = 1; j <= size; j++)
                {
                    A_copy[i, j] = A[i, j];
                }
            }

            // Прямий хід (приведення до трикутного вигляду)
            for (int k = 1; k <= size; k++)
            {
                // Пошук головного елемента
                int maxRow = k;
                for (int i = k + 1; i <= size; i++)
                {
                    if (Math.Abs(A_copy[i, k]) > Math.Abs(A_copy[maxRow, k]))
                    {
                        maxRow = i;
                    }
                }

                // Обмін рядків
                for (int j = k; j <= size; j++)
                {
                    double temp = A_copy[k, j];
                    A_copy[k, j] = A_copy[maxRow, j];
                    A_copy[maxRow, j] = temp;
                }
                double tempB = B_copy[k];
                B_copy[k] = B_copy[maxRow];
                B_copy[maxRow] = tempB;

                // Перевірка на виродженість
                if (A_copy[k, k] == 0)
                {
                    throw new InvalidOperationException("Матриця Якобі вироджена.");
                }

                // Нормалізація рядка та виключення
                for (int i = k + 1; i <= size; i++)
                {
                    double factor = A_copy[i, k] / A_copy[k, k];
                    for (int j = k; j <= size; j++)
                    {
                        A_copy[i, j] -= factor * A_copy[k, j];
                    }
                    B_copy[i] -= factor * B_copy[k];
                }
            }

            // Зворотний хід (знаходження розв'язку x = Dx)
            for (int i = size; i >= 1; i--)
            {
                double sum = 0.0;
                for (int j = i + 1; j <= size; j++)
                {
                    sum += A_copy[i, j] * x[j];
                }
                x[i] = (B_copy[i] - sum) / A_copy[i, i];
            }
        }
    }
}