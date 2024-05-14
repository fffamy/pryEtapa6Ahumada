using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryEtapa6Ahumada
{
    public partial class frmEtapa6 : Form
    {
        List<clsVehiculo> listaVehiculos = new List<clsVehiculo>();
        private Random rnd = new Random();
        //Creo la variable timer
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public frmEtapa6()
        {
            InitializeComponent();

            //Configuro el temporizador en el evento Tick
            timer.Interval = 100;
            timer.Tick += timer_Tick;
        }


        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Ingrese un valor numérico válido para la cantidad.");
                return;
            }

            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad))
            {
                MessageBox.Show("Ingrese un valor numérico válido para la cantidad.");
                return;
            }

            for (int i = 0; i < cantidad; i++)
            {
                CrearVehiculoAleatorio();
            }
        }

        private void CrearVehiculoAleatorio()
        {
            clsVehiculo vehNuevo = new clsVehiculo();
            vehNuevo.crearAuto();

            int margen = 50; // Margen

            int posX;
            int posY;
            bool espacioOcupado;

            //Hago un do-while ubicando una posicion aleatoria dentro del formulario y verificando que no se pisen
            do
            {
                posX = rnd.Next(margen, this.ClientSize.Width - margen - vehNuevo.Auto.Width);
                posY = rnd.Next(margen, this.ClientSize.Height - margen - vehNuevo.Auto.Height);

                espacioOcupado = false;

                foreach (clsVehiculo vehiculoExistente in listaVehiculos)
                {
                    if (Math.Abs(posX - vehiculoExistente.Auto.Location.X) < vehNuevo.Auto.Width &&
                        Math.Abs(posY - vehiculoExistente.Auto.Location.Y) < vehNuevo.Auto.Height)
                    {
                        espacioOcupado = true;
                        break;
                    }
                }
            } while (espacioOcupado);

            vehNuevo.Auto.Location = new Point(posX, posY);
            listaVehiculos.Add(vehNuevo);
            Controls.Add(vehNuevo.Auto);
        }

        private void btnMover_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            foreach (clsVehiculo vehiculo in listaVehiculos.ToList())
            {
                int dx = rnd.Next(-30, 30); // Mov eje X
                int dy = rnd.Next(-30, 30); // Movimiento aleatorio en el eje Y

                // Calcula la nueva posición sumando los cambios aleatorios
                int nuevaPosX = vehiculo.Auto.Location.X + dx;
                int nuevaPosY = vehiculo.Auto.Location.Y + dy;

                // Verifica que la nueva posición esté dentro de los límites del formulario
                nuevaPosX = Math.Max(0, Math.Min(nuevaPosX, this.ClientSize.Width - vehiculo.Auto.Width));
                nuevaPosY = Math.Max(0, Math.Min(nuevaPosY, this.ClientSize.Height - vehiculo.Auto.Height));

                // Actualiza la posición del vehículo
                vehiculo.Auto.Location = new Point(nuevaPosX, nuevaPosY);

                // Verifica colisiones y elimina los vehículos involucrados
                foreach (clsVehiculo otroVehiculo in listaVehiculos.ToList())
                {
                    if (vehiculo != otroVehiculo && vehiculo.Auto.Bounds.IntersectsWith(otroVehiculo.Auto.Bounds))
                    {
                        // Eliminar vehículos de la lista y del formulario
                        Controls.Remove(otroVehiculo.Auto);
                        listaVehiculos.Remove(otroVehiculo);
                    }
                }
            }
        }
    }
}
