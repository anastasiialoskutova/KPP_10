using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // 1. Оголошення полів класу (використовуємо індексацію 1..N)
        private int N = 3;                                  // Розмірність СНР
        private double[,] Ja = new double[4, 4];            // Матриця Якобі
        private double[] X0 = new double[4];                // Вектор початкових умов
        private double[] X = new double[4];                 // Вектор розв'язку СНР (поточне наближення)
        private double[] F = new double[4];                 // Вектор лівої частини СНР
        private double[] Fp = new double[4];                // Робочий вектор для обчислення Якобі
        private double[] Dx = new double[4];                // Вектор нев'язки (розв'язок СЛАР)

        // Поля для LU-розкладу
        private double[,] JaLU = new double[4, 4];          // Матриця для зберігання LU-факторів J(X0)
        private int[] P = new int[4];                       // Вектор перестановок

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
            int arraySize = N + 1 > 2 ? N + 1 : 2;

            Ja = new double[arraySize, arraySize];
            X0 = new double[arraySize];
            X = new double[arraySize];
            F = new double[arraySize];
            Fp = new double[arraySize];
            Dx = new double[arraySize];

            // Ініціалізація полів LU-розкладу
            JaLU = new double[arraySize, arraySize];
            P = new int[arraySize];
        }

        private void SetupDataGridViews()
        {
            // Налаштування DataGridView1 (для X0)
            dataGridView1.ColumnCount = 1;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns[0].Width = 70;

            // Налаштування DataGridView2 (для X*)
            dataGridView2.ColumnCount = 1;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
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
        /// Метод для обчислення значень вектора F лівої частини СНР F(X)=0.
        /// </summary>
        public void FM(double[] X, ref double[] f)
        {
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
                f[1] = X[1] * X[1] - 4.0D;
            }
        }

        /// <summary>
        /// Метод для чисельного обчислення значень матриці Якобі Ja.
        /// </summary>
        public double[,] Jacob(double[] X)
        {
            const double Q = 0.000001D;

            double[] X_copy = (double[])X.Clone();

            FM(X_copy, ref F);

            for (int j = 1; j <= N; j++)
            {
                X_copy[j] = X_copy[j] + Q;
                FM(X_copy, ref Fp);

                for (int i = 1; i <= N; i++)
                {
                    Ja[i, j] = (Fp[i] - F[i]) / Q;
                }

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

                // --- 2. ЕТАП ПЕРЕД ЦИКЛОМ (Обчислення Якобі та LU-розклад) ---

                // 2.1. Обчислення матриці Якобі J(X0)
                Ja = Jacob(X0);

                // 2.2. Копіюємо J(X0) для LU-розкладу (JaLU міститиме L і U)
                JaLU = (double[,])Ja.Clone();

                // 2.3. LU-розклад матриці Якобі J(X0)
                LUSolver.Decompose(JaLU, N, out P);

                // --- 3. ЦИКЛ ІТЕРАЦІЙ ---
                double dxmax = double.MaxValue;
                int k;

                for (k = 1; k <= KMax; k++)
                {
                    // 3.1. Обчислення F(X(k))
                    FM(X, ref F);

                    // 3.2. Розв'язання СЛАР (JaLU) * Dx = F (тільки прямий/зворотний хід)
                    // F - права частина (нев'язка), Dx - розв'язок СЛАР (поправка)
                    LUSolver.Solve(JaLU, F, ref Dx, P, N);

                    // 3.3. Ітерація Ньютона та обчислення норми
                    dxmax = 0.0D;
                    for (int i = 1; i <= N; i++)
                    {
                        // Ітерація Ньютона: X(k+1) = X(k) - Dx(k)
                        X[i] = X[i] - Dx[i];

                        // Норма: Dxmax = max(|Dx[i]|)
                        if (Math.Abs(Dx[i]) > dxmax)
                            dxmax = Math.Abs(Dx[i]);
                    }

                    // 3.4. Перевірка умови закінчення
                    if (dxmax < Eps)
                    {
                        textBox3.Text = Convert.ToString(k);
                        for (int i = 1; i <= N; i++)
                        {
                            dataGridView2.Rows[i - 1].Cells[0].Value = X[i].ToString("F15");
                        }

                        MessageBox.Show("Розв'язок СНР знайдено");
                        return;
                    }
                }

                // 4. Якщо КМах ітерацій не досягнуто
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

    // Клас для реалізації LU-розкладу (замість Гауса)
    public static class LUSolver
    {
        /// <summary>
        /// LU-розклад матриці A (з частковим вибором)
        /// Зберігає L та U фактори в одній матриці LU.
        /// </summary>
        public static void Decompose(double[,] LU, int size, out int[] p)
        {
            // Ініціалізація вектора перестановок P (1-based)
            p = new int[size + 1];
            for (int i = 1; i <= size; i++)
                p[i] = i;

            for (int k = 1; k <= size; k++)
            {
                // 1. Частковий вибір головного елемента (Pivot)
                int maxRow = k;
                double maxVal = 0;
                for (int i = k; i <= size; i++)
                {
                    if (Math.Abs(LU[i, k]) > maxVal)
                    {
                        maxVal = Math.Abs(LU[i, k]);
                        maxRow = i;
                    }
                }

                if (maxVal == 0)
                    throw new InvalidOperationException("Матриця Якобі вироджена.");

                // 2. Зміна рядків у матриці LU та векторі P
                if (maxRow != k)
                {
                    // Swap P vector elements
                    int tempP = p[k];
                    p[k] = p[maxRow];
                    p[maxRow] = tempP;

                    // Swap rows in LU matrix
                    for (int j = 1; j <= size; j++)
                    {
                        double temp = LU[k, j];
                        LU[k, j] = LU[maxRow, j];
                        LU[maxRow, j] = temp;
                    }
                }

                // 3. LU-факторизація (Doolittle)
                for (int i = k + 1; i <= size; i++)
                {
                    // L[i, k] - множник
                    LU[i, k] = LU[i, k] / LU[k, k];

                    // Оновлення підматриці (формування U)
                    for (int j = k + 1; j <= size; j++)
                    {
                        LU[i, j] = LU[i, j] - LU[i, k] * LU[k, j];
                    }
                }
            }
        }

        /// <summary>
        /// Розв'язує СЛАР L*U*x = P*B (прямий та зворотний хід).
        /// </summary>
        public static void Solve(double[,] LU, double[] B, ref double[] x, int[] p, int size)
        {
            // Вектор Y для проміжного розв'язку L*Y = P*B
            double[] Y = new double[size + 1];

            // 1. Прямий хід (Forward Substitution): L*Y = P*B
            for (int i = 1; i <= size; i++)
            {
                // Y[i] = P[B[i]] - Sum(L[i,k] * Y[k])

                // Permutation P*B
                Y[i] = B[p[i]];

                // Subtraction of the sum
                for (int k = 1; k < i; k++)
                {
                    // L-фактор LU[i, k] знаходиться під діагоналлю
                    Y[i] -= LU[i, k] * Y[k];
                }
            }

            // 2. Зворотний хід (Backward Substitution): U*X = Y
            for (int i = size; i >= 1; i--)
            {
                double sum = 0.0;
                // Sum(U[i,k]*X[k])
                for (int k = i + 1; k <= size; k++)
                {
                    // U-фактор LU[i, k] знаходиться на діагоналі та над нею
                    sum += LU[i, k] * x[k];
                }
                // X[i] = (Y[i] - Sum) / U[i,i]
                x[i] = (Y[i] - sum) / LU[i, i];
            }
        }
    }
}