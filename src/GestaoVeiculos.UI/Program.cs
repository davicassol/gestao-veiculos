using GestaoVeiculos.Business;

namespace GestaoVeiculos.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            //erros não tratados dentro das telas 
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => TratarErroInesperado(e.Exception);

            //erros não tratados fora das telas 
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                TratarErroInesperado(e.ExceptionObject as Exception);

            Application.Run(new Form1());
        }

        private static void TratarErroInesperado(Exception? erro)
        {
            if (erro != null)
            {
                LogErros.Registrar(erro, "Erro inesperado (não tratado)");
            }

            MessageBox.Show(
                "Ocorreu um erro inesperado. O problema foi registrado no log da aplicação.",
                "Erro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}