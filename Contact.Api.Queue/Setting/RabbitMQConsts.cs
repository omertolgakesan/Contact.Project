using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Queue
{
    public static class RabbitMQConsts
    {

        /// yaşam süresi
        public static int MessagesTTL { get; set; } = 1000 * 60 * 60 * 2;

        //Aynı anda - Eşzamanlı e-posta gönderimi sayısı, thread açma için sınırı belirleriz
        public static ushort ParallelThreadsCount { get; set; } = 3;
        public enum RabbitMqConstsList
        {
            [Description("ContactReport")]
            ContactReport = 1,
        }
    }
}
