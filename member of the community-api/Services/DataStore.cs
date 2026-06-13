using DuesApi.Models;

namespace DuesApi.Services
{
    public class DataStore
    {
        public List<MiembroDeLaComunidad> Miembros { get; } = new();
        public List<Empleado> Empleados { get; } = new();

        private int _miembroId = 1;
        private int _empleadoId = 1;

        public int NextMiembroId() => _miembroId++;
        public int NextEmpleadoId() => _empleadoId++;

        public DataStore()
        {
            Miembros.Add(new Estudiante { Id = NextMiembroId(), Nombre = "Ana Perez", Tipo = "Estudiante", Matricula = "EST-001" });
            Miembros.Add(new ExAlumno { Id = NextMiembroId(), Nombre = "Luis Gomez", Tipo = "ExAlumno", AnioGraduacion = 2022 });

            Empleados.Add(new Empleado { Id = NextEmpleadoId(), Nombre = "Carlos Diaz", Tipo = "Empleado", Departamento = "General" });
            Empleados.Add(new Docente { Id = NextEmpleadoId(), Nombre = "Maria Rosario", Tipo = "Docente", Departamento = "Academico", Materia = "Matematicas" });
            Empleados.Add(new Administrativo { Id = NextEmpleadoId(), Nombre = "Jose Rivera", Tipo = "Administrativo", Departamento = "Administracion", Area = "Registro" });
            Empleados.Add(new Administrador { Id = NextEmpleadoId(), Nombre = "Laura Mendez", Tipo = "Administrador", Departamento = "Direccion", Area = "Gestion", NivelAcceso = "Alto" });
            Empleados.Add(new Maestro { Id = NextEmpleadoId(), Nombre = "Pedro Santos", Tipo = "Maestro", Departamento = "Academico", Materia = "Historia", Aula = "A-101" });
        }
    }
}
