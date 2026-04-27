using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace TiendaVirtualDelgado.Helpers
{
    public class HashHelper
    {
        public static string ObtenerHash(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
    