using System.Net.Sockets;

namespace Servidor
{
    class Jugador
    {
        private TcpClient socket;
        private string nombre;
        private string puerto;

        public TcpClient Socket
        {
            get
            {
                return socket;
            }

            set
            {
                socket = value;
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public string Puerto
        {
            get
            {
                return puerto;
            }

            set
            {
                puerto = value;
            }
        }

        public Jugador(TcpListener socketServidor)
        {
            byte[] bytesFrom = new byte[100000];
            socket = socketServidor.AcceptTcpClient();
            socket.GetStream().Read(bytesFrom, 0, (int)socket.ReceiveBufferSize);
            nombre = System.Text.Encoding.ASCII.GetString(bytesFrom);
            nombre = nombre.Substring(0, nombre.IndexOf("\0"));
            puerto = "" + socket.Client.RemoteEndPoint;
            puerto = puerto.Substring(puerto.LastIndexOf(":") + 1);
        }

        public void enviarle(string msg)
        {
            NetworkStream destinationStream = socket.GetStream();
            byte[] destinationBytes = System.Text.Encoding.ASCII.GetBytes(msg + '\0');
            destinationStream.Write(destinationBytes, 0, destinationBytes.Length);
            destinationStream.Flush();
        }

        public string recibir()
        {
            byte[] bytesFrom = new byte[100000];
            socket.GetStream().Read(bytesFrom, 0, (int)socket.ReceiveBufferSize);
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
            return dataFromClient.Substring(0, dataFromClient.IndexOf("\0"));
        }


    }
}
