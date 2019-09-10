using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            /*double amplitud =
                double.Parse(txtAmplitud.Text);
            double fase =
                double.Parse(txtFase.Text);
            double frecuencia =
                double.Parse(txtFrecuencia.Text);*/
            double tiempoInicial =
                double.Parse(txtTiempoInicial.Text);
            double tiempoFinal =
                double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo =
                double.Parse(txtFrecuenciaMuestreo.Text);

            Señal señal;

            switch(cbTipoSenal.SelectedIndex)
            {
                case 0: //parabolica
                    señal = new SenalParabolica();
                    break;
                case 1: //senoidal
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2: //exponencial
                    double alfa = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion.Children[0])).txtAlfa.Text);

                    señal = new SeñalExponencial(alfa);
                    break;
                default:
                    señal = null;
                    break;
            }

            señal.TiempoInicial = tiempoInicial;

            señal.TiempoFinal = tiempoFinal;

            señal.FrecuenciaMuestreo = frecuenciaMuestreo;

            señal.construirSeñal();

            double periodoMuestreo = 1.0 / frecuenciaMuestreo;

            double amplitudMaxima = señal.AmplitudMaxima;

            plnGrafica.Points.Clear();

            foreach(Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

            lblLimiteSuperior.Text = amplitudMaxima.ToString();
            lblLimiteInferior.Text = "-" + amplitudMaxima.ToString();

            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));

        }

        public Point adaptarCoordenadas( double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x - tiempoInicial) * scrGrafica.Width, (-1 *
                (y * (((scrGrafica.Height / 2.0) - 25) / amplitudMaxima))) + (scrGrafica.Height / 2.0) );
        }

        private void CbTipoSenal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch(cbTipoSenal.SelectedIndex)
            {
                case 0: //exponencial
                    break;
                case 1: //senoidal
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //exponencial
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                default:
                    break;
            }
        }
    }
}
