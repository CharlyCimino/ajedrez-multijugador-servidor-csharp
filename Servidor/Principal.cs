using System;
using System.Net.Sockets;

namespace Servidor
{
    class Principal
    {
        static void Main(string[] args)
        {

            string ip;
            Console.Write("IP del servidor (Enter = localhost): ");
            ip = Console.ReadLine();
            if (ip.Equals(""))
                ip = "127.0.0.1";
            TcpListener socketServidor = new TcpListener(System.Net.IPAddress.Parse(ip), 8888);
            socketServidor.Start();

            while (true)     // Hasta que sea apagado el servidor, acepta dos jugadores y los hace jugar...
            {
                bool turno = true;  // Variable booleana que gestiona el cambio de turno.

                Random rnd = new Random();

                Console.WriteLine();
                Console.WriteLine("Servidor en " + ip + ":" + 8888);
                Jugador jugador1 = new Jugador(socketServidor);
                Console.WriteLine("Jugador " + jugador1.Nombre +" conectado desde " + jugador1.Socket.Client.RemoteEndPoint);
                jugador1.enviarle("Esperando que el otro jugador se conecte!");

                Jugador jugador2 = new Jugador(socketServidor);
                Console.WriteLine("Jugador " + jugador2.Nombre + " conectado desde " + jugador2.Socket.Client.RemoteEndPoint);

                // Protocolo:
                // X: se define quien es blancas y quien negras (blancas: 1 | negras: 0)
                // Texto a mostrar en el cliente.
                turno = (rnd.Next(0, 1) == 0) ? false : true;
                jugador1.enviarle("Comenzar"+((turno) ? "1" : "0")+ ";" + jugador2.Nombre);
                jugador2.enviarle("Comenzar"+ ((turno) ? "0" : "1") + ";" + jugador1.Nombre);

                bool partidaEnCurso = true;
                Jugador jActivo;
                Jugador jEspera;
                string movida;

                while (partidaEnCurso)
                {
                    // Cargo el jugador activo y el que está en espera dependiendo del turno.
                    jActivo = (turno) ? jugador1 : jugador2;
                    jEspera = (turno) ? jugador2 : jugador1;

                    // Obtengo la movida del jugador en curso.
                    movida = jActivo.recibir();
                    Console.WriteLine("Llegó la movida: " + movida);

                    // Si alguno gana, termina la partida y se muestra al ganador por consola
                    if (movida.Equals("terminado"))
                    {
                        Console.WriteLine("Ganó " + (turno ? jugador1.Nombre : jugador2.Nombre));
                        partidaEnCurso = false;
                    }
                    else    // Si la partida sigue en curso:
                    {
                        // Se genera un log en la consola de todos los movimientos.
                        Console.WriteLine((turno ? jugador1.Nombre : jugador2.Nombre) + ": " + movida);

                        //Recalcular posición para el otro cliente
                        int desde = 77 - Int32.Parse(movida.Substring(0, 2));
                        int hasta = 77 - Int32.Parse(movida.Substring(2, 2));

                        // Agregar un 0 a la izquierda si el resultado de la resta es de un solo dígito
                        if (desde < 10) { movida = "0" + desde.ToString(); }
                        else { movida = desde.ToString(); }
                        if (hasta < 10) { movida += "0" + hasta.ToString(); }
                        else { movida += hasta.ToString(); }

                        // Envio la movida al otro jugador.
                        jEspera.enviarle(movida);
                    }
                    // cambio el turno:
                    turno = !turno;
                }
            }
        }
    }
}

