# Un0-Engine
Resumen
El Un0 Engine es una herramienta poderosa diseñada para convertir imágenes en representaciones de texto y visualizarlas en la consola. Este artículo presenta el diseño, implementación y uso del Motor Un0, que incluye funcionalidades como coincidencia de colores, renderizado de píxeles y conversión de imágenes a texto, todo en la consola de WindowsNT. El motor aprovecha la biblioteca System.Drawing en C# para procesar imágenes y extraer información de color. Al convertir el color de cada píxel en caracteres de texto correspondientes, el motor crea representaciones de imágenes similares a arte ASCII. La representación de texto resultante se puede mostrar en la consola utilizando diversos colores, brindando una experiencia visual nueva para la consola.

Introducción
Visualizar imágenes en el entorno de la consola puede ser un desafío debido a las limitaciones de color y capacidades gráficas. El Un0 Engine aborda esta limitación convirtiendo imágenes en representaciones basadas en texto, lo que permite a los usuarios ver imágenes en la consola utilizando una combinación de caracteres coloreados. Este artículo describe los componentes clave y las funcionalidades del Un0 Engine, proporcionando información sobre su implementación y posibles aplicaciones.

Coincidencia de Colores
El Un0 Engine incluye un algoritmo de coincidencia de colores que encuentra la representación de color más cercana para un color de entrada dado. El algoritmo compara los valores RGB del color de entrada con un diccionario predefinido de colores y calcula la distancia euclidiana entre ellos. El color con la distancia mínima se considera la coincidencia más cercana. Este mecanismo de coincidencia de colores permite una conversión precisa de los píxeles de la imagen a sus representaciones de texto correspondientes.
Conversión de Imágenes a Texto
La funcionalidad principal del Un0 Engine es la conversión de imágenes a representaciones de texto. El motor utiliza la biblioteca System.Drawing en C# para leer imágenes y extraer los colores de los píxeles. Luego, el color de cada píxel se empareja con un carácter de texto utilizando el algoritmo de coincidencia de colores descrito en la Sección 2. La representación de texto resultante se construye concatenando los caracteres coincidentes fila por fila. Este proceso permite una representación compacta y precisa de la imagen original.

Visualización en la Consola
El Un0 Engine ofrece capacidades versátiles de visualización en la consola. El motor proporciona funciones para renderizar píxeles y dibujar imágenes utilizando caracteres coloreados. La función Pixel establece el color de la consola para que coincida con el color del píxel y muestra un par de espacios, creando la ilusión de un píxel del tamaño de un solo carácter. La función uDraw acepta una representación de texto de una imagen, junto con sus dimensiones, y renderiza la imagen en la consola llamando a la función Pixel para cada píxel.

Uso y Ejemplos

Para utilizar el Un0 Engine, los usuarios pueden instanciar la clase Engine y acceder a sus diversas funcionalidades. La función ConvertirImagenATexto convierte un archivo de
imagen en una representación de texto, devolviendo una lista de cadenas donde cada cadena representa una fila de la imagen. Esta lista de cadenas de texto se puede pasar a la función uDraw para visualizar la imagen en la consola. 
La función iDraw acepta la ruta de un archivo de imagen, junto con las coordenadas deseadas, y representa la imagen en la consola. Los usuarios también pueden aprovechar la función Put para mostrar texto en la consola con retrasos opcionales entre caracteres.

A continuación se muestra un ejemplo de uso del Un0 Engine:
Engine engine = new Engine();
List<string> textImage = engine.ConvertirImagenATexto("path/to/image.png");
engine.iDraw("path/to/image.png", 10, 10);
engine.Put("Hello, World!", 20, 5, true);


Conclusión
El Un0 Engine proporciona una forma interesante y única de visualizar imágenes en la consola utilizando representaciones de texto y colores. La capacidad de convertir imágenes en representaciones de texto abre nuevas posibilidades para la visualización artística y creativa en entornos de línea de comandos. El algoritmo de coincidencia de colores y las funciones de renderizado de píxeles permiten una conversión precisa y una experiencia visual atractiva. El Un0 Engine puede ser utilizado en aplicaciones interactivas, proyectos de arte digital y en cualquier contexto donde se requiera una visualización creativa de imágenes en la consola.
