using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppEjemploGrafico
{
    public partial class Form1 : Form
    {
        // Aquí guardaremos los puntos que forman la figura seleccionada
        private Point[] puntos;

        // Matriz de ejemplo 
        private int[,] matriz;

        public Form1()
        {
            InitializeComponent(); // Inicializa los componentes  diseñados con el diseñador visual
            InicializarComponentesPersonalizados(); // Agrega elementos al formulario mediante código
        }

        // Agrega controles como TextBox, ComboBox y botón al formulario
        private void InicializarComponentesPersonalizados()
        {
            // Cambia el título de la ventana y su tamaño
            this.Text = "Aplicación Gráfica en .NET";
            this.Size = new Size(800, 600);

            //TextBox para ingresar cuántos puntos tendrá la figura (solo números)
            TextBox txtNumeroPuntos = new TextBox();
            txtNumeroPuntos.Location = new Point(20, 20);
            txtNumeroPuntos.Width = 100;
            txtNumeroPuntos.Name = "txtNumeroPuntos";
            txtNumeroPuntos.KeyPress += TxtNumeroPuntos_KeyPress; // Solo permite números
            this.Controls.Add(txtNumeroPuntos);

            //ComboBox para elegir qué figura se va a dibujar
            ComboBox cmbFigura = new ComboBox();
            cmbFigura.Location = new Point(140, 20);
            cmbFigura.Width = 150;
            cmbFigura.Name = "cmbFigura";
            cmbFigura.Items.Add("Triángulo");
            cmbFigura.Items.Add("Cuadrado");
            cmbFigura.Items.Add("Círculo");
            cmbFigura.SelectedIndexChanged += CmbFigura_SelectedIndexChanged; // Evento al cambiar de opción
            this.Controls.Add(cmbFigura);

            // Botón que genera y dibuja la figura 
            Button btnDibujar = new Button();
            btnDibujar.Location = new Point(310, 20);
            btnDibujar.Text = "Dibujar";


            btnDibujar.Click += (sender, e) =>
            {
                // Verificar si el campo de número de puntos está vacío
                if (string.IsNullOrEmpty(txtNumeroPuntos.Text))
                {
                    MessageBox.Show("Ingrese el número de puntos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si se ha seleccionado una figura
                if (cmbFigura.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione una figura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar que el número ingresado sea numérico
                if (!int.TryParse(txtNumeroPuntos.Text, out int numPuntos))
                {
                    MessageBox.Show("El número de puntos debe ser numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Para formar figuras cerradas (como triángulos o más lados), se necesita mínimo 3 puntos
                if (numPuntos < 3)
                {
                    MessageBox.Show("Se requieren al menos 3 puntos para formar una figura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Centro de la ventana (para dibujar desde el centro)
                int centroX = this.ClientSize.Width / 2;
                int centroY = this.ClientSize.Height / 2;
                int radio = 100; // Tamaño de la figura
                puntos = null; // Limpiamos puntos anteriores

                // Según la figura seleccionada, calculamos los puntos
                switch (cmbFigura.SelectedItem.ToString())
                {
                    case "Triángulo":
                        puntos = new Point[3]; // Solo 3 puntos
                        for (int i = 0; i < 3; i++)
                        {
                            double angulo = i * 2 * Math.PI / 3; // Equidistantes (360° / 3)
                            puntos[i] = new Point(
                                centroX + (int)(radio * Math.Cos(angulo)),
                                centroY + (int)(radio * Math.Sin(angulo)));
                        }
                        break;

                    case "Cuadrado":
                        puntos = new Point[4];
                        puntos[0] = new Point(centroX - radio, centroY - radio);
                        puntos[1] = new Point(centroX + radio, centroY - radio);
                        puntos[2] = new Point(centroX + radio, centroY + radio);
                        puntos[3] = new Point(centroX - radio, centroY + radio);
                        break;

                    case "Círculo":
                        // Simulamos un círculo con 36 puntos en forma de polígono
                        puntos = new Point[36];
                        for (int i = 0; i < 36; i++)
                        {
                            double angulo = i * 2 * Math.PI / 36;
                            puntos[i] = new Point(
                                centroX + (int)(radio * Math.Cos(angulo)),
                                centroY + (int)(radio * Math.Sin(angulo)));
                        }
                        break;
                }

                // Matriz 3x3 que representa la identidad (usada en transformaciones)
                matriz = new int[3, 3]
                {
                    {1, 0, 0},
                    {0, 1, 0},
                    {0, 0, 1}
                };

                // Redibuja el formulario, lo que activa OnPaint()
                this.Invalidate();
            };
            this.Controls.Add(btnDibujar);
        }

        // Evita que se escriban letras en el TextBox (solo números y teclas de control como borrar)
        private void TxtNumeroPuntos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true; // Ignora la tecla
        }

        private void CmbFigura_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Este método se ejecuta automáticamente cuando el formulario necesita dibujarse (como tras hacer Invalidate)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (puntos != null && puntos.Length > 0)
            {
                // Dibuja la figura uniendo todos los puntos (polígono cerrado)
                e.Graphics.DrawPolygon(Pens.Blue, puntos);

                // Dibuja cada punto como un pequeño círculo rojo (para ver claramente los vértices)
                foreach (var punto in puntos)
                {
                    e.Graphics.FillEllipse(Brushes.Red, punto.X - 3, punto.Y - 3, 6, 6);
                }
            }
        }
    }
}

