using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libros_basededatos
{
    public partial class Base_de_datos_libro : Form
    {
        public Base_de_datos_libro()
        {
            InitializeComponent();
        }
        SqlConnection conexion = new SqlConnection(@"server=DESKTOP-DC0P3Q7\SQLEXPRESS; database=Libro de Base de Datos; Integrated Security=true");

        private void bntIngresar_Click(object sender, EventArgs e)
        {
                conexion.Open();
                string insertar = "INSERT INTO TablaLibro (Cod_Libro, NombreLibro, PrecioCompra, FechaCompra )VALUES(@Cod_Libro, @NombreLibro, @PrecioCompra, @FechaCompra )";
                SqlCommand comando = new SqlCommand(insertar, conexion);
                comando.Parameters.Add(new SqlParameter("@Cod_Libro", this.txtCodigo.Text));
                comando.Parameters.Add(new SqlParameter("@NombreLibro", this.txtNombre.Text));
                comando.Parameters.Add(new SqlParameter("@PrecioCompra", this.txtPrecio.Text));
                comando.Parameters.Add(new SqlParameter("@FechaCompra", datoFecha.Value));
                comando.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Filas Insertadas Correctamente");
            textVerCodigo.Clear();
            textVerPrecio.Clear();
            textVerNombre.Clear();
            Base_de_datos_libro_Load(sender, e);
        }

        private void Base_de_datos_libro_Load(object sender, EventArgs e)
        {
            DataTable dt = getpersona();
            this.comboLibro.DataSource = dt;
            this.comboLibro.DisplayMember = "NombreLibro";
            this.comboLibro.ValueMember = "Cod_Libro";
        }
        private DataTable getpersona(string codigo = "")
        {
            string sql = "";
            if (codigo == "")
            {
                sql = "select Cod_Libro, NombreLibro, PrecioCompra, FechaCompra ";
                sql += "from TablaLibro order by nombreLibro, Cod_Libro";
            }
            else
            {
                sql = "select Cod_Libro, NombreLibro, PrecioCompra, FechaCompra ";
                sql += "from TablaLibro where Cod_Libro=@Cod_Libro order by NombreLibro";
            }

            SqlCommand comando = new SqlCommand(sql, conexion);
            if (codigo != "")
            {
                comando.Parameters.Add(new SqlParameter("@Cod_Libro", codigo));
            }
            SqlDataAdapter ad1 = new SqlDataAdapter(comando);

            //pasar los datos del adaptador a un datatable
            DataTable dt = new DataTable();
            ad1.Fill(dt);
            return dt;
        }
        private void Mostrar(object sender, EventArgs e)
        {
            //hola
            DataTable dt = getpersona(this.comboLibro.SelectedValue.ToString());
            //mostrar la informacion
            foreach (DataRow row in dt.Rows)
            {
                this.textVerCodigo.Text = row["Cod_Libro"].ToString();
                this.textVerNombre.Text = row["NombreLibro"].ToString();
                this.textVerPrecio.Text = row["PrecioCompra"].ToString();
                this.VerDatoFecha.Value = Convert.ToDateTime(row["FechaCompra"].ToString());

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("DESE ELIMINAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string eliminar = "DELETE FROM TablaLibro WHERE Cod_Libro = @Cod_Libro";
                SqlCommand cmd3 = new SqlCommand(eliminar, conexion);
                cmd3.Parameters.AddWithValue("@Cod_Libro", this.textVerCodigo.Text);
                cmd3.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("DATO ELIMINADO CON EXITO", "ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dt = getpersona();
                textVerCodigo.Clear();
                textVerPrecio.Clear();
                textVerNombre.Clear();
                Base_de_datos_libro_Load(sender, e);
            }
            else
            {
                MessageBox.Show("NO SE ELIMINO NINGUN DATO", "CANCELAR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" DESEAS ACTUALIZAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string actualizar = "UPDATE TablaLibro SET NombreLibro=@NombreLibro, PrecioCompra=@PrecioCompra, FechaCompra=@FechaCompra WHERE Cod_Libro=@Cod_Libro";
                SqlCommand cmd2 = new SqlCommand(actualizar, conexion);
                cmd2.Parameters.AddWithValue("@Cod_Libro", this.textVerCodigo.Text);
                cmd2.Parameters.AddWithValue("@PrecioCompra", this.textVerPrecio.Text);
                cmd2.Parameters.AddWithValue("@NombreLibro", this.textVerNombre.Text);
                cmd2.Parameters.AddWithValue("@FechaCompra", VerDatoFecha.Value);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Actualizacion de Datos");
                conexion.Close();
                DataTable dt = getpersona();
                textVerCodigo.Clear();
                textVerPrecio.Clear();
                textVerNombre.Clear();
                Base_de_datos_libro_Load(sender, e);
            }
        }
    }
}
