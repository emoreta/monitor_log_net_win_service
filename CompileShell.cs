using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorGuaba
{
    public class CompileShell
    {
        public void Main(string path,string urlService,string palabrasClave)
        {
            string scriptPath = path;
            //string scriptPath = @"C:\ScriptShell\script.ps1";//colocar logica para que cree el archivo en la raiz del proyecto

            StringBuilder scriptContent= new StringBuilder();
            scriptContent.AppendLine("Ruta del archivo de registro (cambia la ruta según tu necesidad)");
            scriptContent.AppendLine("function Buscar-PalabraClave {");
            scriptContent.AppendLine("param (");
            scriptContent.AppendLine("[string]$rutaArchivo,");
            scriptContent.AppendLine("[string]$palabrasClave");
            scriptContent.AppendLine(")");
            scriptContent.AppendLine("Get-Content $rutaArchivo -Wait | ForEach-Object {");
            scriptContent.AppendLine("if ($_ -match $palabrasClave) {");
            scriptContent.AppendLine("Write-Host \"Palabra clave encontrada: $_\"");
            scriptContent.AppendLine("$Body = @{");
            scriptContent.AppendLine("type=$palabrasClave;");
            scriptContent.AppendLine("message = $_;");
            scriptContent.AppendLine("code=\"[022]\";");
            scriptContent.AppendLine("dateTime=\"1 / 19 / 2024 5:19:01 PM\"");
            scriptContent.AppendLine("}");
            scriptContent.AppendLine("$JsonBody = $Body | ConvertTo-Json");
            scriptContent.AppendLine("$Uri = “"+ urlService + "”");
            scriptContent.AppendLine("Invoke-RestMethod -ContentType “application/json” -Uri $Uri -Method Post -Body $JsonBody");
            scriptContent.AppendLine("}");
            scriptContent.AppendLine("}");
            scriptContent.AppendLine("}");
            scriptContent.AppendLine("Buscar-PalabraClave -rutaArchivo $archivoLog -palabrasClave \""+ palabrasClave + "\"");

            try
            {
                // Escribir el contenido del script en un archivo .ps1
                File.WriteAllText(scriptPath, scriptContent.ToString());

                // Crear un proceso para ejecutar PowerShell
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{scriptPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // Iniciar el proceso
                Process process = Process.Start(psi);

                // Leer la salida estándar y los errores estándar
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Esperar a que el proceso termine
                process.WaitForExit();

                // Mostrar la salida y los errores
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el script:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Eliminar el archivo del script después de su ejecución
                if (File.Exists(scriptPath))
                {
                    File.Delete(scriptPath);
                }
            }

        }
      
        
    }
            
}