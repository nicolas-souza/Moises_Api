namespace api
{
    public class Log
    {
        const string FileLogErro = @"C:\Estudos\TCC\LogErro.txt";
        const string FileLog = @"C:\Estudos\TCC\Log.txt";

        public static void RegistroErro(String usuario, Exception ex)
        {
            string mensagemLog;

            mensagemLog = $"\nUsuario: {usuario} \n" +
                          $"Data: {DateTime.Now} \n" +
                          $"Erro: {ex.Message}\n" +
                          $"___________________________________________________________________ \n";

            File.AppendAllText(FileLogErro, mensagemLog);
        }
        public static void RegistroUso(String usuario, string url)
        {
            string mensagemLog;

            mensagemLog = $"\nUsuario: {usuario} \n" +
                          $"Data: {DateTime.Now} \n" +
                          $"Mensagem: {url}\n" +
                          $"___________________________________________________________________ \n";

            File.AppendAllText(FileLog, mensagemLog);
        }
    }

}
