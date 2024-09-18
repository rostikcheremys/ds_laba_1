using System.Windows;

namespace Program
{
    public partial class MainWindow : Window
    {
        static double[] x;
        static double[] y;
        
        static double a1, b1, d1, a2, b2, d2;
        
        static int n;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                x = textBoxX.Text.Split(',').Select(double.Parse).ToArray();
                y = textBoxY.Text.Split(',').Select(double.Parse).ToArray();

                if (x.Length != y.Length)
                {
                    MessageBox.Show("Масиви x і y повинні мати однакову довжину.");
                    return;
                }

                n = x.Length;
                
                Thread threadFirst = new Thread(ThreadFunctionFirst);
                Thread threadSecond = new Thread(ThreadFunctionSecond);

                threadFirst.Start();
                threadSecond.Start();

                threadFirst.Join(); 
                threadSecond.Join(); 
                
                textBoxResult.Text = d1 < d2 ? $"y = {a1} * ln(x) + {b1}" : $"y = {Math.Pow(Math.E, a2)} * x^{b2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        static void ThreadFunctionFirst()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;

            for (int i = 0; i < n; i++)
            {
                x[i] = Math.Log(x[i]);
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }

            a1 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b1 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d1 = Math.Sqrt(((Yi - a1 * Xi - b1) * (Yi - a1 * Xi - b1)) / (Yi * Yi));
        }

        static void ThreadFunctionSecond()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;

            for (int i = 0; i < n; i++)
            {
                y[i] = Math.Log(y[i]);
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }

            a2 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b2 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d2 = Math.Sqrt(((Yi - a2 * Xi - b2) * (Yi - a2 * Xi - b2)) / (Yi * Yi));
        }
    }
}
