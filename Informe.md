# INFORME SOBRE AS DIFERENCIAS EN CONECTIVIDADE

Hemos hecho pruebas con tres tipos de autoridades: en cliente, en servidor y en servidor con rebobinar.

## CLIENT AUTHORITY vs SERVER AUTHORITY

Para que la autoridad esté en el cliente, debemos crear un script que herede de la clase NetworkTransform y anule la autoridad innata del servidor que tiene esta. En dicho caso, la respuesta es más rápida que con la autoridad en el servidor, pero el objeto no percibe los límites del campo y por lo tanto puede caer al vacío. Si hacemos pruebas sin interpolación, en ambos casos comprobamos que el movimiento de los objetos, percibido desde el resto de clientes, es más torpón, siendo prácticamente a saltos.

## SERVER AUTHORITY WITH REWIND

Debido a que la única manera de aplicar un rewind real sería cambiando la autoridad innata del servidor que tiene el NetworkTransform entre llamadas, no es posible simularlo en este ejercicio. Si se pudiera, primero se haría una llamada con autoridad en el cliente y posteriormente otra con autoridad en el servidor, que corregiría la posición del objeto que se ha movido en caso de que fuera necesario.