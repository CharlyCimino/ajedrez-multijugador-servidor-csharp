Especificaciones para conectarse al servidor:

1. El socket que debe ser abierto es TCP.

2. Conectarse a la ip del servidor, en estos momentos 192.168.10.95 por el puerto 8888 (que siempre es fijo).

3. Una vez conectado, debe enviarse el nombre del jugador como cadena de caracteres o string.

4. Una vez que ambos jugadores esten conectados el servidor enviará a cada uno un string compuesto por: "(1|0),nombreContrario". Si se recibia el 1 es el que tiene las blancas y le toca jugar, de lo contrario (recibido un 0) se le ha asignado las negras y debe esperar a que el otro mueva.

por ejemplo: 1,pepe => juan empieza a jugar (blancas) contra "pepe" (negras). Por su parte el servidor enviará "0,juan" a pepe para notificarlo que le tocan las negras y le toca mover a juan.

5. El servidor sólo recibe los movimientos con el formato "LNLN", donde:

   * L es una letra [A-H] y N un número [1-8].
   * La primera combinacion "LN" es el origen de la pieza que va a ser movida a las cordenadas "LN" que le siguen.

Por ejemplo: si elijo como primer movimiento de la partida un peón (blanco), debo indicar "D2D4".

IMPORTANTE:

    * No debe enviarse '\n' al final de cada string.
    * La lógica, restricciones de movimientos y todo lo referente a la mecánica del juego debe hacerlo el cliente. El servidor solo retransmite el movimiento del rival de un extremo a otro.

